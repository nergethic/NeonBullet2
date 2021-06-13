using System;
using System.Collections;
using Assets._Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    const float ROOM_SCALE_MULTIPLIER = 0.7f;
    
    [SerializeField] Room mainRoomBlueprint;
    [SerializeField] List<Room> rooms;
    [SerializeField] List<Room> corridors;
    [SerializeField] GameObject testRoom;
    [SerializeField] BoxCollider testRoomCollider;
    [SerializeField] int numberOfRooms;
    
    [SerializeField] bool DEBUG_SlowDownGeneration;
    
    RoomType nextRoomType;
    Room currentRoom;
    RoomData currentRoomData;
    Vector2 roomSize;
    
    List<Room> availableRooms = new List<Room>();
    List<Room> availableCorridors = new List<Room>();
    List<Room> alreadyGeneratedRooms = new List<Room>();
    List<Room> selectedRoomList;
    Coroutine debugCor;

    void Awake() {
        foreach (var cor in corridors)
            Assert.IsTrue(cor is Corridor);

        Room mainRoom = Instantiate(mainRoomBlueprint);
        roomSize = mainRoom.GetSize();
        alreadyGeneratedRooms.Add(mainRoom);
        currentRoom = mainRoom;
        currentRoomData.entryDoors = mainRoom.DoorsData[0];
        nextRoomType = RoomType.MainRoom;
        GenerateLevel();
    }

    void GenerateLevel() {
        if (DEBUG_SlowDownGeneration) {
            SlowSpawnRooms();
            return;
        }
        
        SpawnRooms();
    }

    void SlowSpawnRooms() {
        if (debugCor != null) {
            StopCoroutine(debugCor);
            debugCor = null;
        }
        debugCor = StartCoroutine(GenerateLevelSlowly(10f));
    }

    void SpawnRooms() {
        bool spawnCorridor = true;
        for (int i = 0; i < numberOfRooms; i++) {
            foreach (var doorEntry in currentRoom.DoorsData) {
                if (doorEntry.direction == currentRoomData.entryDoors.direction)
                    continue;
                
                var nextDoorsDirection = doorEntry.direction.GetOppositeDirection();
                if (TryFindNextRoom(nextDoorsDirection, spawnCorridor, out var roomData)) {
                    SpawnRoom(roomData);
                    // TODO: if a room will have multiple exits this will explode (currentRoom/currentRoomData will be wrong for a second exit)
                    break;
                }
                    
            }
            
            if (Random.Range(0f, 1f) < 0.5f)
                spawnCorridor = !spawnCorridor;
        }
    }

    bool TryFindNextRoom(DoorDirection nextDoorsDirection, bool spawnCorridor, out RoomData newRoomData) {
        var currentRoomPos = currentRoom.transform.position;
        newRoomData = new RoomData {
            spawnPosition = GetNextSpawnLocation(nextDoorsDirection.GetOppositeDirection(), currentRoomPos.x, currentRoomPos.y),
            isCorridor = spawnCorridor
        };

        availableRooms = new List<Room>(rooms);
        availableCorridors = new List<Room>(corridors);
        selectedRoomList = spawnCorridor ? availableCorridors : availableRooms;
        
        for (int triesCount = 0; triesCount < 200; triesCount++) {
            if (selectedRoomList.Count == 0)
                return false;
            
            var randomIndex = Random.Range(0, selectedRoomList.Count);
            var potentialRoomToSpawn = selectedRoomList[randomIndex];
            newRoomData.room = potentialRoomToSpawn;

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

        return false;
    }

    void SpawnRoom(RoomData roomData) {
        Room newRoom = Instantiate(roomData.room);
        newRoom.transform.position = roomData.spawnPosition;
        alreadyGeneratedRooms.Add(newRoom);
        currentRoom = newRoom;
        currentRoomData = roomData;
    }

    void HandleWrongRoom(Room roomToCreate) {
        selectedRoomList.Remove(roomToCreate);
    }

    bool CouldBePlaced(RoomData roomData) {
        Vector3 spawnPos = roomData.spawnPosition;
        if (WouldCollideWithSomeRoom(roomData.room, spawnPos))
            return false;

        // NOTE: room could be placed but let's check if its doors aren't blocked
        var doors = roomData.room.DoorsData;
        if (doors.Count == 1)
            return true;
        
        int correctExitDoorsCount = 0;
        for (int i = 0; i < doors.Count; i++) {
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
    
    IEnumerator GenerateLevelSlowly(float delay) {
        bool spawnCorridor = true;
        for (int i = 0; i < numberOfRooms; i++) {
            yield return new WaitForSeconds(delay);
            
            foreach (var doorEntry in currentRoom.DoorsData) {
                if (doorEntry.direction == currentRoomData.entryDoors.direction)
                    continue;
                
                var nextDoorsDirection = doorEntry.direction.GetOppositeDirection();
                if (TryFindNextRoom(nextDoorsDirection, spawnCorridor, out var roomData))
                    SpawnRoom(roomData);
            }
            
            if (Random.Range(0f, 1f) < 0.5f)
                spawnCorridor = !spawnCorridor;
        }
    }
}

struct RoomData {
    public Room room;
    public DoorData entryDoors;
    public Vector3 spawnPosition;
    public bool isCorridor;
}