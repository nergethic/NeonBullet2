using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : SceneManager {
    [SerializeField] GameObject[] projectileGameObjects;
    Dictionary<ProjectileType, GameObject> projectiles;

    public override void Init(MasterSystem masterSystem, SceneManagerData data) {
        base.Init(masterSystem, data);
        type = SceneManagerType.Projectile;

        if (!AssertDistinctProjectile()) {
            Debug.LogError("[]");
            initializationState = ManagerInitializationState.FAILED;
            return;
        }
        
        projectiles = new Dictionary<ProjectileType, GameObject>();
        foreach (var p in projectileGameObjects) {
            var projectileComponent = p.GetComponent<Projectile>();
            Assert.IsTrue(projectileComponent != null);

            ProjectileType type = projectileComponent.GetType();
            projectiles.Add(type, p);
        }
        
        initializationState = ManagerInitializationState.COMPLETED;
    }

    public Projectile SpawnProjectile(Vector3 position, ProjectileType type, bool ownedByPlayer, float speed) {
        bool found = projectiles.TryGetValue(type, out var projectileGameObject);
        if (!found)
            return null;
        
        var spawnedProjectileGameObject = Instantiate(projectileGameObject);
        var projectile = spawnedProjectileGameObject.GetComponent<Projectile>();
        if (projectile == null) {
            Debug.LogError("[]");
            return null;
        }
        
        projectile.Initialize(Vector2.zero, ownedByPlayer, speed);
        projectile.transform.position = position;
        
        return projectile;
    }

    public override void Tick(float dt)
    {
        
    }

    bool AssertDistinctProjectile() {
        return true;
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