using System.Collections.Generic;
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

    public Projectile SpawnProjectile(Vector3 position, Vector2 dir, ProjectileType type, bool ownedByPlayer, float speed) {
        bool found = projectiles.TryGetValue(type, out var projectileGameObject);
        if (!found)
            return null;

        Projectile projectile = SpawnProjectileObject(projectileGameObject);
        if (projectile == null) {
            Debug.LogError("[ProjectileManager]: projectile is null");
            return null;
        }


        projectile.Initialize(new ProjectileData { 
            dir = dir.normalized,
            speed = speed,
            ownedByPlayer = ownedByPlayer
        });

        projectile.transform.position = position;
        return projectile;
    }
    
    public Vector2 GetVectorWithRotation(Vector2 dir ,float angle) {
        float radian = angle * Mathf.Deg2Rad;
        float _x = dir.x * Mathf.Cos(radian) - dir.y * Mathf.Sin(radian);
        float _y = dir.x * Mathf.Sin(radian) + dir.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    public override void Tick(float dt) {}

    static Projectile SpawnProjectileObject(GameObject projectileGameObject) {
        var spawnedProjectileGameObject = Instantiate(projectileGameObject);
        var projectile = spawnedProjectileGameObject.GetComponent<Projectile>();
        return projectile;
    }
}

public enum ProjectileType {
    Standard = 0,
    Energy,
    Standard2,
    StandardBlue,
    StandardOrange,
    Grenade
}