using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemSceneManager : SceneManager {
    [SerializeField] List<Item> items;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);
        
        CollectSceneItems();
        foreach (var item in items) {
            item.Initialize(data.player, data.playerController);
        }
        
        Debug.Log("[ItemSceneSystem]: Item system initialized");
        initializationState = ManagerInitializationState.INITIALIZED;
    }

    public override void Tick(float dt) {
    }

#if UNITY_EDITOR
    [ContextMenu("Collect Scene Items")]
    public void CollectSceneItems() {
        // Undo.RecordObject(items, "[ItemSceneManager]: Collecting Items");
        var foundSceneItems = FindObjectsOfType<Item>();
        if (foundSceneItems != null && foundSceneItems.Length > 0)
            items = foundSceneItems.ToList();
    }
#endif
}
