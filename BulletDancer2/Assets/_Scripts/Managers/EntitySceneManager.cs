using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EntitySceneManager : SceneManager {
    [SerializeField] List<Entity> entites;
    [SerializeField] ProjectileManager projectileManager;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);
        type = SceneManagerType.Entity;
        
        CollectSceneEntities();
        foreach (var entity in entites) {
            entity.Initialize(data.player, projectileManager);
        }
        
        initializationState = ManagerInitializationState.COMPLETED;
    }

    public override void Tick(float dt) {
        foreach (var entity in entites) {
            if (entity.isDead) {
                continue; // TODO
            }
            entity.Tick(dt);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Collect Scene Items")]
    public void CollectSceneEntities() {
        Undo.RecordObject(this, "[EntitySceneManager]: Collecting entities");
        var foundSceneEntities = FindObjectsOfType<Entity>();
        if (foundSceneEntities != null && foundSceneEntities.Length > 0)
            entites = foundSceneEntities.ToList();
    }
#endif
}
