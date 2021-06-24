using System.Collections.Generic;
using UnityEngine;

public class RoomStructureSpawner : MonoBehaviour {
    [SerializeField] Transform spawnLocation;
    [SerializeField] List<RoomStructure> possibleRoomStructures;
    
    void Start() {
        if (possibleRoomStructures?.Count == 0)
            return;

        int randIdx = Random.Range(0, possibleRoomStructures.Count);
        var randomStructure = possibleRoomStructures[randIdx];
        var structure = Instantiate(randomStructure);
        structure.transform.position = spawnLocation.position;
    }
}
