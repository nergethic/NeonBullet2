using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EntitySceneManager : SceneManager {
    [SerializeField] List<Entity> entites;
    [SerializeField] ProjectileManager projectileManager;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);
        
        CollectSceneItems();
        foreach (var entity in entites) {
            entity.Initialize(data.player, projectileManager);
        }
        
        Debug.Log("[ItemSceneSystem]: Item system initialized");
        initializationState = ManagerInitializationState.INITIALIZED;
    }

    public override void Tick() {
    }

#if UNITY_EDITOR
    [ContextMenu("Collect Scene Items")]
    public void CollectSceneItems() {
        // Undo.RecordObject(items, "[ItemSceneManager]: Collecting Items");
        var foundSceneItems = FindObjectsOfType<Entity>();
        if (foundSceneItems != null && foundSceneItems.Length > 0)
            entites = foundSceneItems.ToList();
    }
#endif
}
