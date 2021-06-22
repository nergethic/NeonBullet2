using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemSceneManager : SceneManager {
    [SerializeField] List<Item> items;
    private ProjectileManager projectileManager;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);
        type = SceneManagerType.Item;

        projectileManager = masterSystem.TryGetManager<ProjectileManager>(SceneManagerType.Projectile);
        if (projectileManager == null) {
            Debug.LogError("[ItemSceneSystem]: tried to get projectileManager but it wasn't initialized");
            return;
        }
        
        CollectSceneItems();
        foreach (var item in items) {
            item.Initialize(data.player, data.playerController, projectileManager);
        }
        
        ChangeInitializationState(ManagerInitializationState.COMPLETED);
    }

    public void AddItem(Item item)
    {
        item.Initialize(data.player, data.playerController, projectileManager);
    }

    public override void Tick(float dt) {
    }

#if UNITY_EDITOR
    [ContextMenu("Collect Scene Items")]
    public void CollectSceneItems() {
        Undo.RecordObject(this, "[ItemSceneManager]: Collecting Items");
        var foundSceneItems = FindObjectsOfType<Item>();
        if (foundSceneItems != null && foundSceneItems.Length > 0)
            items = foundSceneItems.ToList();
    }
#endif
}
