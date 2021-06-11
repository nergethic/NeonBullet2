using Assets._Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] Room mainRoom;
    [SerializeField] List<Room> rooms;
    [SerializeField] List<Corridor> corridors;
    [SerializeField] int numberOfRooms;

    private const int LEVEL_ROOM_COUNT = 5;
    private RoomType nextRoomType = RoomType.MainRoom;
    private Room currentRoom;
    private Vector2 roomSize;
    private List<Room> alreadyGeneratedRooms = new List<Room>();
    private List<Room> wrongChosenRooms = new List<Room>();
    private List<Corridor> wrongChosenCorridors = new List<Corridor>();
    private bool isCorridorTurn = true;
    
    private void Awake() {
        Instantiate(mainRoom);
        roomSize = mainRoom.GetSize();
        alreadyGeneratedRooms.Add(mainRoom);
        currentRoom = mainRoom;
        GenerateLevel();
    }

    private void GenerateLevel() {

        for (int i = 1; i < numberOfRooms; i++)
        {
            var currentDoorDirection = currentRoom.doorsData[1].direction;
            PlaceRoom(currentDoorDirection);
        }
    }

    private void PlaceRoom(DoorDirection direction)
    {
        var isCorrectRoomFound = false;
        var currentX = currentRoom.transform.position.x;
        var currentY = currentRoom.transform.position.y;
        var nextDirection = GetNextDirection(direction, currentX, currentY);

        while (!isCorrectRoomFound)
        {
            Room roomToCreate;
            int randomIndex;

            if (isCorridorTurn)
            {
                randomIndex = Random.Range(0, corridors.Count);
                roomToCreate = corridors[randomIndex];
            }
            else
            {
                randomIndex = Random.Range(0, rooms.Count);
                roomToCreate = rooms[randomIndex];
            }


            if (roomToCreate.doorsData[0].direction == direction.GetOppositeDirection())
            {
                if (!CheckIfCollideWithOtherRoom(roomToCreate.doorsData[1].direction, nextDirection, roomToCreate))
                {
                    var room = Instantiate(roomToCreate);
                    room.transform.position = nextDirection;
                    isCorrectRoomFound = true;

                    if (isCorridorTurn)
                    {
                        corridors.RemoveAt(randomIndex);
                    }
                    else
                    {
                        rooms.RemoveAt(randomIndex);
                    }
                    currentRoom = room;
                    alreadyGeneratedRooms.Add(room);
                }
                else
                {
                    isCorrectRoomFound = HandleWrongRoom(isCorrectRoomFound, roomToCreate);
                }
            }
            else
            {
                isCorrectRoomFound = HandleWrongRoom(isCorrectRoomFound, roomToCreate);
            }
        }

        if (isCorridorTurn)
        {
            corridors.AddRange(wrongChosenCorridors);
            wrongChosenCorridors.Clear();
            isCorridorTurn = false;
        }
        else
        {
            rooms.AddRange(wrongChosenRooms);
            wrongChosenRooms.Clear();
            isCorridorTurn = true;
        }
    }

    private bool HandleWrongRoom(bool isCorrectRoomFound, Room roomToCreate)
    {
        if (isCorridorTurn)
        {
            corridors.AddRange(wrongChosenCorridors);
            corridors.Remove((Corridor)roomToCreate);
        }
        else
        {
            wrongChosenRooms.Add(roomToCreate);
            rooms.Remove(roomToCreate);
        }

        if (rooms.Count == 0 && !isCorridorTurn || corridors.Count == 0 && isCorridorTurn)
        {
            isCorrectRoomFound = true;
        }

        return isCorrectRoomFound;
    }

    private Vector2 GetNextDirection(DoorDirection direction, float currentX, float currentY)
    {
        switch (direction)
        {
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

    private bool CheckIfCollideWithOtherRoom(DoorDirection direction, Vector2 nextDirection, Room room)
    {
        switch (direction)
        {
            case DoorDirection.Top:
                return false;
            case DoorDirection.Down:
                return false;
            case DoorDirection.Left:
                var vectorToCheck = new Vector2(nextDirection.x - 0.7f * roomSize.x, nextDirection.y);
                var collidingRoom = alreadyGeneratedRooms.Where(r => (Vector2)r.transform.position == vectorToCheck).FirstOrDefault();
                if (collidingRoom != null)
                    return true;
                return false;
            case DoorDirection.Right:
                return false;
            default:
                return false;
        }
    }
}
