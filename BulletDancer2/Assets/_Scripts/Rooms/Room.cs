using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] RoomType roomType;
    [SerializeField] DoorData[] doorsData;
    [SerializeField] BoxCollider2D collider;

    public Vector2 GetSize() => collider.size;
    public Bounds GetBounds() => collider.bounds;
    public IList<DoorData> DoorsData => doorsData;
}

[Serializable]
public struct DoorData {
    public Transform location;
    public DoorDirection direction;
}

public enum DoorDirection {
    Top,
    Down,
    Left,
    Right
}
