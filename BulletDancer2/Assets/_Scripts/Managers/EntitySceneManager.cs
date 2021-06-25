using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EntitySceneManager : SceneManager {
    [SerializeField] List<Entity> entites;
    [SerializeField] ProjectileManager projectileManager;
    [SerializeField] Ore ore;
    [SerializeField] Iron iron;
    [SerializeField] Gold gold;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);

        CollectSceneEntities();
        foreach (var entity in entites) {
            if (entity == null) {
                Debug.LogError("[EntitySceneManager]: one of the entities is null");
                return;
            }
            
            InitEntity(entity);
        }
        
        ChangeInitializationState(ManagerInitializationState.COMPLETED);
    }

    public void AddEntity(Entity entity) {
        InitEntity(entity);
        entites.Add(entity);
    }
    
    void InitEntity(Entity entity) {
        entity.Initialize(data.player, projectileManager);

        if (entity is Enemy enemy)
            HandleDrop(enemy);
    }

    void HandleDrop(Enemy enemy) {
        switch (enemy.drop) {
            case ResourceTypeDrop.Ore:
                enemy.InitializeResource(ore);
                break;
            case ResourceTypeDrop.Iron:
                enemy.InitializeResource(iron);
                break;
            case ResourceTypeDrop.Gold:
                enemy.InitializeResource(gold);
                break;
        }
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
        var foundSceneEntities = FindObjectsOfType<Entity>(false);
        if (foundSceneEntities != null && foundSceneEntities.Length > 0)
            entites = foundSceneEntities.ToList();
    }
#endif
}
