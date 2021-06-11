using Assets._Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] Room mainRoom;
    [SerializeField] List<Room> rooms;
    [SerializeField] List<Room> corridors;
    [SerializeField] int numberOfRooms;
    
    RoomType nextRoomType;
    Room currentRoom;
    Vector2 roomSize;
    List<Room> alreadyGeneratedRooms = new List<Room>();
    List<Room> wrongChosenRooms = new List<Room>();
    List<Corridor> wrongChosenCorridors = new List<Corridor>();
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

    void GenerateLevel() {
        for (int i = 0; i < numberOfRooms; i++) {
            var currentDoorDirection = currentRoom.doorsData[1].direction;
            PlaceRoom(currentDoorDirection);
            
            if (isCorridorTurn) {
                corridors.AddRange(wrongChosenCorridors);
                wrongChosenCorridors.Clear();
            } else {
                rooms.AddRange(wrongChosenRooms);
                wrongChosenRooms.Clear();
            }
            isCorridorTurn = !isCorridorTurn;
        }
    }

    void PlaceRoom(DoorDirection direction) {
        var isCorrectRoomFound = false;
        Vector2 currentRoomPos = new Vector2(currentRoom.transform.position.x, currentRoom.transform.position.y);
        var nextDirection = GetNextDirection(direction, currentRoomPos.x, currentRoomPos.y);

        var selectedRoomList = isCorridorTurn ? corridors : rooms;
        for (int triesCount = 0; triesCount < 10 && !isCorrectRoomFound; triesCount++) {
            if (rooms.Count == 0)
                Debug.LogError("Wants to generate more rooms but all available were spawned!");

            if (selectedRoomList.Count == 0)
                return;
            
            var randomIndex = Random.Range(0, selectedRoomList.Count-1);
            var roomToCreate = selectedRoomList[randomIndex];

            var doorDir = roomToCreate.doorsData[0].direction;
            if (doorDir != direction.GetOppositeDirection()) {
                isCorrectRoomFound = HandleWrongRoom(roomToCreate);
                continue;
            }

            if (CheckIfCollidesWithOtherRoom(roomToCreate.doorsData[1].direction, nextDirection, roomToCreate)) {
                isCorrectRoomFound = HandleWrongRoom(roomToCreate);
                continue;
            }
            
            isCorrectRoomFound = true;
            SpawnRoom(roomToCreate, nextDirection);
            if (!isCorridorTurn)
                rooms.RemoveAt(randomIndex);
        }
    }

    void SpawnRoom(Room room, Vector3 position) {
        Room newRoom = Instantiate(room);
        newRoom.transform.position = position;
        currentRoom = newRoom;
        alreadyGeneratedRooms.Add(newRoom);
    }

    bool HandleWrongRoom(Room roomToCreate) {
        if (isCorridorTurn) {
            corridors.AddRange(wrongChosenCorridors);
            corridors.Remove(roomToCreate);
        } else {
            wrongChosenRooms.Add(roomToCreate);
            rooms.Remove(roomToCreate);
        }

        bool correctRoomFound = rooms.Count == 0 && !isCorridorTurn || corridors.Count == 0 && isCorridorTurn;
        return correctRoomFound;
    }

    Vector2 GetNextDirection(DoorDirection direction, float currentX, float currentY) {
        switch (direction) {
            case DoorDirection.Top:
                return new Vector2(currentX, currentY + (roomSize.y * 0.7f));
            case DoorDirection.Down:
                return new Vector2(currentX, currentY - (roomSize.y * 0.7f));
            case DoorDirection.Left:
                return new Vector2(currentX - (roomSize.x * 0.7f), currentY);
            case DoorDirection.Right:
                return new Vector2(currentX + (roomSize.x * 0.7f), currentY);
            default:
                return new Vector2(currentX, currentY);
        }
    }

    bool CheckIfCollidesWithOtherRoom(DoorDirection direction, Vector2 nextDirection, Room room) {
        switch (direction) {
            case DoorDirection.Top:
            case DoorDirection.Down:
            case DoorDirection.Right:
                return false;

            case DoorDirection.Left:
                var vectorToCheck = new Vector2(nextDirection.x - 0.7f * roomSize.x, nextDirection.y);
                var collidingRoom = alreadyGeneratedRooms.FirstOrDefault(r => (Vector2)r.transform.position == vectorToCheck);
                return collidingRoom != null;
            
            default:
                Debug.LogError("Unhandled switch case");
                return false;
        }
    }
}
