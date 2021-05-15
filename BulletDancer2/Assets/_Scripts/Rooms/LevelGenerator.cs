using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] Room mainRoom;
    [SerializeField] List<Room> rooms;

    private const int LEVEL_ROOM_COUNT = 5;
    private RoomType nextRoomType = RoomType.MainRoom;
    private Vector3 nextRoomEntrancePosition = Vector3.zero;
    private Room currentRoom;

    Room GetRandomRoom(IReadOnlyList<Room> rooms) {
        int randIndex = Random.Range(0, rooms.Count - 1);
        return rooms[randIndex];
    }
    
    private void Awake() {
        Instantiate(mainRoom);
        currentRoom = mainRoom;
        nextRoomEntrancePosition = mainRoom.doorsData[0].location.position;
        GenerateRoom();
        //GenerateCorridor(nextRoomEntrancePosition);

        /*
        int generatedRoomsCount = 0;
        Room currentRoom = null;
        for (int i = 0; generatedRoomsCount < LEVEL_ROOM_COUNT; i++) {
            switch (nextRoomType) {
                case RoomType.MainRoom:
                    break;
                case RoomType.BossRoom:
                    break;
                case RoomType.Corridor:
                    break;
                case RoomType.Room:
                    var randomRoom = GetRandomRoom(rooms);
                    currentRoom = Instantiate(randomRoom);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            nextRoomEntrancePosition = currentRoom.doors.position;
            generatedRoomsCount++;
        }
        */
    }

    private void GenerateRoom() {
        var roomBounds = currentRoom.GetBounds();
        var firstDoor = currentRoom.doorsData[0];
        var maxX = roomBounds.max.x;
        var maxY = roomBounds.max.y;
        
        switch (firstDoor.direction) {
            case DoorDirection.Top:
                
                break;
            case DoorDirection.Down:
                break;
            case DoorDirection.Left:
                break;
            case DoorDirection.Right:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        var from = currentRoom.transform.position.x / 2;
        var to = currentRoom.transform.position.y / 2;
    }

    public void GenerateCorridor(Vector3 from, Vector3 to) {
        
    }
}
