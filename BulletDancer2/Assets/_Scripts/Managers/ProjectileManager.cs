using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : SceneManager {
    [SerializeField] GameObject[] projectileGameObjects;
    Dictionary<ProjectileType, GameObject> projectiles;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);

        projectiles = new Dictionary<ProjectileType, GameObject>();
        for (int i = 0; i < projectileGameObjects.Length; i++) {
            var p = projectileGameObjects[i];
            var projectileComponent = p.GetComponent<Projectile>();
            if (projectileComponent == null) {
                Debug.LogError("[ProjectileManager]: projectile component not found in game object");
                ChangeInitializationState(ManagerInitializationState.FAILED);
                return;
            }

            for (int j = 0; j < projectileGameObjects.Length; j++) {
                if (i == j)
                    continue;

                var otherProjectileComponent = projectileGameObjects[j].GetComponent<Projectile>();
                if (otherProjectileComponent == null)
                    continue;
                
                if (projectileComponent.Type() == otherProjectileComponent.Type()) {
                    Debug.LogError($"[ProjectileManager]: two projectiles with same projectile type found '{projectileComponent.Type().ToString()}' - this should not happen");
                    ChangeInitializationState(ManagerInitializationState.FAILED);
                    return;
                }
            }
            projectiles.Add(projectileComponent.Type(), p);
        }
        
        ChangeInitializationState(ManagerInitializationState.COMPLETED);
    }

    public Projectile SpawnProjectile(Vector3 position, ProjectileType type, bool ownedByPlayer, float speed) {
        bool found = projectiles.TryGetValue(type, out var projectileGameObject);
        if (!found)
            return null;
        
        var spawnedProjectileGameObject = Instantiate(projectileGameObject);
        var projectile = spawnedProjectileGameObject.GetComponent<Projectile>();
        if (projectile == null) {
            Debug.LogError("[ProjectileManager]: projectile is null");
            return null;
        }
        
        projectile.Initialize(Vector2.zero, ownedByPlayer, speed);
        projectile.transform.position = position;
        
        return projectile;
    }

    public override void Tick(float dt) {}
}

public enum ProjectileType {
    Standard = 0,
    Energy,
    Standard2,
    StandardBlue,
    StandardOrange,
    Grenade
}