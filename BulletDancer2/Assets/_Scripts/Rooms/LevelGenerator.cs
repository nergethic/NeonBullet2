using System;
using System.Collections;
using Assets._Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    public event Action OnLevelGenerated;
    
    const float ROOM_SCALE_MULTIPLIER = 0.7f;
    const float SLOW_ROOM_GENERATE_DELAY = 1f;
    readonly WaitForSeconds waitDelay = new WaitForSeconds(SLOW_ROOM_GENERATE_DELAY);
    
    [SerializeField] Transform roomsParent;
    [SerializeField] Room mainRoomBlueprint;
    [SerializeField] Room bossRoomBlueprint;
    [SerializeField] List<Room> rooms;
    [SerializeField] List<Room> corridors;
    [SerializeField] GameObject testRoom;
    [SerializeField] BoxCollider testRoomCollider;
    [SerializeField] int numberOfRooms;
    [SerializeField] List<Room> nextRoomsBlueprints;
    [SerializeField] Portal portal;
    [SerializeField] bool DEBUG_SlowDownGeneration;

    MasterSystem masterSystem;
    Room currentRoom;
    RoomData currentRoomData;
    Room mainRoom;
    Vector2 roomSize;

    List<Room> availableRooms = new List<Room>();
    List<Room> availableCorridors = new List<Room>();
    List<Room> alreadyGeneratedRooms = new List<Room>();
    List<Room> selectedRoomList;
    List<Room> nextRoomsBlueprintsCopy;
    Coroutine generateRoomsCor;

    public void Initialize(MasterSystem masterSystem) {
        this.masterSystem = masterSystem;
        foreach (var cor in corridors)
            Assert.IsTrue(cor is Corridor);

        AssertRoomDataIsInitialized(mainRoomBlueprint);
        foreach (var room in rooms)
            AssertRoomDataIsInitialized(room);
        foreach (var corridor in corridors)
            AssertRoomDataIsInitialized(corridor);

        mainRoom = Instantiate(mainRoomBlueprint);
        ResetState();

        masterSystem.OnSceneManagersInitialized += GenerateLevel;
    }

    [ContextMenu("Generate")]
    public void GenerateLevel() {
        RemoveRooms();
        GenerateLevel(mainRoom);
    }

    void RemoveRooms() {
        var entityManager = masterSystem.TryGetManager<EntitySceneManager>(SceneManagerType.Entity);
        entityManager.Reset();
        
        for (int i = roomsParent.childCount - 1; i >= 0; i--)
            Destroy(roomsParent.GetChild(i).gameObject);

        ResetState();
    }

    void GenerateLevel(Room firstRoom) {
        nextRoomsBlueprintsCopy = nextRoomsBlueprints.ToList();
        if (generateRoomsCor != null) {
            StopCoroutine(generateRoomsCor);
            generateRoomsCor = null;
        }
        generateRoomsCor = StartCoroutine(GenerateLevelSlowly(firstRoom));
    }

    bool TryFindNextRoom(DoorDirection nextDoorsDirection, bool spawnCorridor, out RoomData newRoomData, bool spawnBossRoom) {
        var currentRoomPos = currentRoom.transform.position;
        newRoomData = new RoomData {
            spawnPosition = GetNextSpawnLocation(nextDoorsDirection.GetOppositeDirection(), currentRoomPos.x, currentRoomPos.y),
            isCorridor = spawnCorridor
        };
        
#if UNITY_EDITOR // NOTE: debug code
        if (nextRoomsBlueprintsCopy.Count != 0) {
            var roomBlueprint = nextRoomsBlueprintsCopy[0];
            nextRoomsBlueprintsCopy.RemoveAt(0);
            newRoomData.room = roomBlueprint;
            var (doorsFound, doorData) = roomBlueprint.FindDoorsWithDir(nextDoorsDirection);
            if (!doorsFound) {
                Debug.LogError("DOORS NOT FOUND");
                return false;
            }
            newRoomData.entryDoors = doorData;
            
            return true;
        }
#endif

        availableRooms = new List<Room>(rooms);
        availableCorridors = new List<Room>(corridors);
        selectedRoomList = spawnCorridor ? availableCorridors : availableRooms;
        
        for (int triesCount = 0; triesCount < selectedRoomList.Count; triesCount++) {
            if (selectedRoomList.Count == 0)
                return false;

            var randomIndex = Random.Range(0, selectedRoomList.Count);
            Room potentialRoomToSpawn = selectedRoomList[randomIndex];
            newRoomData.room = potentialRoomToSpawn;
            
            if (spawnBossRoom) {
                potentialRoomToSpawn = bossRoomBlueprint;
                newRoomData.room = potentialRoomToSpawn;
            }

            var (doorsFound, doorData) = potentialRoomToSpawn.FindDoorsWithDir(nextDoorsDirection);
            newRoomData.entryDoors = doorData;
            if (!doorsFound) {
                HandleWrongRoom(potentialRoomToSpawn);
                continue;
            }

            if (!CouldBePlaced(newRoomData)) {
                HandleWrongRoom(potentialRoomToSpawn);
                continue;
            }
            
            return true;
        }

        newRoomData.room = null;
        return false;
    }

    Room SpawnRoom(RoomData roomData) {
        Room newRoom = Instantiate(roomData.room);
        newRoom.transform.position = roomData.spawnPosition;
        newRoom.transform.SetParent(roomsParent);
        return newRoom;
    }

    void HandleWrongRoom(Room roomToCreate) {
        selectedRoomList.Remove(roomToCreate);
    }

    bool CouldBePlaced(RoomData roomData) {
        Vector3 spawnPos = roomData.spawnPosition;
        if (WouldCollideWithSomeRoom(roomData.room, spawnPos))
            return false;

        // NOTE: room could be placed but let's check if its doors aren't blocked
        var doors = roomData.room.doorsData;
        if (doors.Length == 1)
            return true;
        
        int correctExitDoorsCount = 0;
        for (int i = 0; i < doors.Length; i++) {
            var door = doors[i];
            if (door.direction == roomData.entryDoors.direction)
                continue;

            Vector3 nextSpawnLocation = GetNextSpawnLocation(door.direction, spawnPos.x, spawnPos.y);
            if (!WouldCollideWithSomeRoom(roomData.room, nextSpawnLocation))
                correctExitDoorsCount++;
        }
        
        return correctExitDoorsCount > 0;
    }

    bool WouldCollideWithSomeRoom(Room room, Vector3 roomPosition) {
        bool foundCollision = false;
        PrepareTestRoomCollider(room, roomPosition);
        Bounds roomBounds = testRoomCollider.bounds;
        
        foreach (var generatedRoom in alreadyGeneratedRooms) {
            var bounds = generatedRoom.GetBounds();
            if (bounds.Intersects(roomBounds)) {
                foundCollision = true;
                break;
            }
        }

        return foundCollision;
    }

    void PrepareTestRoomCollider(Room room, Vector3 spawnPosition) {
        testRoom.transform.position = spawnPosition;
        testRoomCollider.center = Vector3.zero;
    }
    
    Vector2 GetNextSpawnLocation(DoorDirection direction, float currentX, float currentY) {
        Vector2 scaledRoomSize = roomSize * ROOM_SCALE_MULTIPLIER;
        switch (direction) {
            case DoorDirection.Top:
                return new Vector2(currentX, currentY + scaledRoomSize.y);
            case DoorDirection.Down:
                return new Vector2(currentX, currentY - scaledRoomSize.y);
            case DoorDirection.Left:
                return new Vector2(currentX - scaledRoomSize.x, currentY);
            case DoorDirection.Right:
                return new Vector2(currentX + scaledRoomSize.x, currentY);
            default:
                Debug.LogError("[LevelGenerator]: Unhandled door direction");
                return new Vector2(currentX, currentY);
        }
    }
    
    void ResetState() {
        availableRooms.Clear();
        availableCorridors.Clear();
        alreadyGeneratedRooms.Clear();
        alreadyGeneratedRooms.Add(mainRoom);
        
        roomSize = mainRoom.GetSize();
    }
    
    bool generateFromRoomWasSuccessful;
    IEnumerator GenerateLevelSlowly(Room firstRoom) {
        var firstRoomDoorsData = firstRoom.doorsData;
        int generateBossRoomIndex = Random.Range(0, 2);
        
        for (int i = 0; i < firstRoomDoorsData.Length; i++) {
            var doors = firstRoomDoorsData[i];
            
            generateFromRoomWasSuccessful = false;
            yield return GenerateFromRoom(firstRoom, doors, generateBossRoomIndex == i);

            if (!generateFromRoomWasSuccessful) {
                RemoveRooms();
                yield return null;
                i = -1;
            }
            nextRoomsBlueprintsCopy = nextRoomsBlueprints.ToList();
        }
        
        OnLevelGenerated?.Invoke();
        yield return null;
    }
    
    IEnumerator GenerateFromRoom(Room firstRoom, DoorData firstDoorData, bool generateBossRoom) {
        currentRoom = firstRoom;
        currentRoomData.entryDoors = firstDoorData;
        int roomsToGenerate = numberOfRooms * 2; // rooms+corridors
        
        bool spawnCorridor = true;
        int numberOfGeneratedRooms = 0;
        int tryCount = 0;
        int maxTryCount = roomsToGenerate * 3;
        while (numberOfGeneratedRooms != roomsToGenerate) {
            tryCount++;
            if (tryCount > maxTryCount)
                break;
            if (DEBUG_SlowDownGeneration)
                yield return waitDelay;
            else
                yield return null;

            var exitRoomDoors = currentRoom.doorsData;
            for (int exitDoorsIdx = 0; exitDoorsIdx < exitRoomDoors.Length; exitDoorsIdx++) {
                var exitDoors = exitRoomDoors[exitDoorsIdx];
                if (exitDoors.direction == currentRoomData.entryDoors.direction)
                    continue;
                
                var nextEntryDoorsDirection = exitDoors.direction.GetOppositeDirection();
                bool roomWasGenerated = false;
                var spawnBossRoom = generateBossRoom && numberOfGeneratedRooms == roomsToGenerate - 1;
                var spawnLastRoom = !generateBossRoom && numberOfGeneratedRooms == roomsToGenerate - 1;
                for (int j = 0; j < 5; j++) {
                    if (TryFindNextRoom(nextEntryDoorsDirection, spawnCorridor, out var roomData, spawnBossRoom)) {
                        var spawnedRoom = SpawnRoom(roomData);
                        alreadyGeneratedRooms.Add(spawnedRoom);
                        currentRoom = spawnedRoom;
                        currentRoomData = roomData;
                        if (spawnLastRoom)
                            Instantiate(portal, currentRoom.transform);

                        OpenDoors(exitDoors.doorTransform);
                        OpenDoors(roomData.entryDoors.doorTransform);
                        numberOfGeneratedRooms++;
                        roomWasGenerated = true;
                        if (numberOfGeneratedRooms == roomsToGenerate) {
                            for (int k = 0; k < currentRoom.doorsData.Length; k++) {
                                if (currentRoom.doorsData[k].direction == currentRoomData.entryDoors.direction)
                                    continue;
                                CloseDoors(currentRoom.doorsData[k].doorTransform);
                            }
                        }
                        break;
                    }
                }

                if (roomWasGenerated) {
                    spawnCorridor = !spawnCorridor;
                    break;
                }
            }
        }
        
        generateFromRoomWasSuccessful = numberOfGeneratedRooms == roomsToGenerate;
        yield return null;
    }

    void OpenDoors(Transform doors) {
        doors.gameObject.SetActive(false);
    }
    
    void CloseDoors(Transform doors) {
        doors.gameObject.SetActive(true);
    }
    
    void AssertRoomDataIsInitialized(Room room) {
        if (room == null) {
            Assert.IsTrue(false, "[LevelGenerator]: room is null");
            return;
        }
            
        foreach (var data in room.doorsData)
            Assert.IsTrue(data.doorTransform != null, $"[LevelGenerator]: doorsData transform is null in '{room.name}' prefab");
    }
}

struct RoomData {
    public Room room;
    public DoorData entryDoors;
    public Vector3 spawnPosition;
    public bool isCorridor;
}