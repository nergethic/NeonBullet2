using System.Collections;
using Assets._Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    const float ROOM_SCALE_MULTIPLIER = 0.7f;
    
    [SerializeField] Room mainRoom;
    [SerializeField] List<Room> rooms;
    [SerializeField] List<Room> corridors;
    [SerializeField] GameObject testRoom;
    [SerializeField] BoxCollider testRoomCollider;
    [SerializeField] int numberOfRooms;
    
    [SerializeField] bool DEBUG_SlowDownGeneration;
    
    RoomType nextRoomType;
    Room currentRoom;
    Vector2 roomSize;
    
    List<Room> availableRooms = new List<Room>();
    List<Room> availableCorridors = new List<Room>();
    List<Room> alreadyGeneratedRooms = new List<Room>();
    List<Room> selectedRoomList;

    bool isCorridorTurn = true;
    
    void Awake() {
        foreach (var cor in corridors)
            Assert.IsTrue(cor is Corridor);

        Instantiate(mainRoom);
        roomSize = mainRoom.GetSize();
        alreadyGeneratedRooms.Add(mainRoom);
        currentRoom = mainRoom;
        nextRoomType = RoomType.MainRoom;
        GenerateLevel();
    }

    Coroutine debugCor;
    void GenerateLevel() {
        if (DEBUG_SlowDownGeneration) {
            if (debugCor != null) {
                StopCoroutine(debugCor);
                debugCor = null;
            }
            debugCor = StartCoroutine(GenerateLevelSlowly());
            return;
        }
        
        for (int i = 0; i < numberOfRooms; i++) {
            var currentDoorDirection = currentRoom.DoorsData[1].direction;
            PlaceRoom(currentDoorDirection);
            if (Random.Range(0f, 1f) < 0.5f)
                isCorridorTurn = !isCorridorTurn;
        }
    }

    void PlaceRoom(DoorDirection direction) {
        Vector2 currentRoomPos = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y);
        var nextSpawnLocation = GetNextSpawnLocation(direction, currentRoomPos.x, currentRoomPos.y);

        availableRooms = new List<Room>(rooms);
        availableCorridors = new List<Room>(corridors);
        selectedRoomList = isCorridorTurn ? availableCorridors : availableRooms;
        
        for (int triesCount = 0; triesCount < 200; triesCount++) {
            if (selectedRoomList.Count == 0)
                return;
            
            var randomIndex = Random.Range(0, selectedRoomList.Count);
            var potentialRoomToSpawn = selectedRoomList[randomIndex];

            var doorDir = potentialRoomToSpawn.DoorsData[0].direction; // TODO: why u check only the first entry, not like this
            if (doorDir != direction.GetOppositeDirection()) {
                HandleWrongRoom(potentialRoomToSpawn);
                continue;
            }

            if (WouldCollideWithSomeRoom(potentialRoomToSpawn, nextSpawnLocation)) {
                HandleWrongRoom(potentialRoomToSpawn);
                continue;
            }
            
            SpawnRoom(potentialRoomToSpawn, nextSpawnLocation);
            return;
        }
    }

    void SpawnRoom(Room room, Vector3 position) {
        Room newRoom = Instantiate(room);
        newRoom.transform.position = position;
        currentRoom = newRoom;
        alreadyGeneratedRooms.Add(newRoom);
    }

    void HandleWrongRoom(Room roomToCreate) {
        selectedRoomList.Remove(roomToCreate);
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
                return new Vector2(currentX, currentY);
        }
    }
    
    bool WouldCollideWithSomeRoom(Room room, Vector3 spawnPosition) {
        PrepareTestRoomCollider(room, spawnPosition);
        Bounds roomBounds = testRoomCollider.bounds;
        bool foundCollision = false;
        
        // check potential spawn location
        foreach (var generatedRoom in alreadyGeneratedRooms) {
            var bounds = generatedRoom.GetBounds();
            if (bounds.Intersects(roomBounds)) {
                foundCollision = true;
                break;
            }
        }
        
        // TODO: check location of even next room going from potential room
        return foundCollision;
    }

    void PrepareTestRoomCollider(Room room, Vector3 spawnPosition) {
        testRoom.transform.position = spawnPosition;
        testRoomCollider.size = new Vector3(0, 0);
        testRoomCollider.center = new Vector3(0, 0);
        testRoomCollider.bounds.Encapsulate(room.GetBounds());
        testRoomCollider.bounds.Encapsulate(new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z - 100f));
        testRoomCollider.bounds.Encapsulate(new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z + 100f));
    }
    
    IEnumerator GenerateLevelSlowly() {
        for (int i = 0; i < numberOfRooms; i++) {
            var currentDoorDirection = currentRoom.DoorsData[1].direction;
            PlaceRoom(currentDoorDirection);
            if (Random.Range(0f, 1f) < 0.5f)
                isCorridorTurn = !isCorridorTurn;
            yield return new WaitForSeconds(3f);
        }
    }
}
