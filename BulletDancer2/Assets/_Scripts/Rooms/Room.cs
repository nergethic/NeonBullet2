using System;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] RoomType roomType;
    [SerializeField] public DoorData[] doorsData;
    [SerializeField] BoxCollider collider;

    public Vector3 GetSize() => collider.size;
    public BoxCollider BoxCollider => collider;
    public Bounds GetBounds() => collider.bounds;
    //public DoorData[] DoorsData() {
        //var data = doorsData;
        //for (int i = 0; i < doorsData.Length; i++)
            //data[i].doorTransform = doorsData[i].doorTransform;

        //return data;
    //}

    public (bool, DoorData) FindDoorsWithDir(DoorDirection dir) {
        foreach (var doorEntry in doorsData)
            if (doorEntry.direction == dir)
                return (true, doorEntry);
        
        return (false, default);
    }
}

[Serializable]
public struct DoorData {
    public Transform doorTransform;
    public DoorDirection direction;
}

public enum DoorDirection {
    Top,
    Down,
    Left,
    Right
}
