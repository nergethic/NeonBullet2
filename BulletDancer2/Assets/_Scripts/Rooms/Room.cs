using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] public RoomType roomType;
    [SerializeField] public DoorData[] doorsData;
    [SerializeField] private BoxCollider collider;

    public Bounds GetBounds() => collider.bounds;
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
