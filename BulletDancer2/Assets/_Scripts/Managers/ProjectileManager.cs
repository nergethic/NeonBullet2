using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileManager : SceneManager {
    [SerializeField] List<GameObject> projectileGameObjects = new List<GameObject>();

    public override void Init(MasterSystem masterSystem, SceneManagerData data)
    {
         initializationState = ManagerInitializationState.INITIALIZED;
    }
    private void Awake() {
        foreach (var p in projectileGameObjects) {
            var projectileComponent = p.GetComponent<Projectile>();
            Assert.IsTrue(projectileComponent != null);
        }
    }

    public (GameObject, Projectile) SpawnProjectile(Vector3 position, ProjectileType type, bool ownedByPlayer, float speed) {
        foreach (var projectileGameObject in projectileGameObjects) {
            var p = projectileGameObject.GetComponent<Projectile>();
            if ((int)p.GetType() == (int)type) { // TODO
                var bulletGO = Instantiate(projectileGameObject);
                var bullet = bulletGO.GetComponent<Projectile>();
                
                bullet.Initialize(Vector2.zero, ownedByPlayer, speed);
                bullet.transform.position = position;

                return (bulletGO, bullet);
            }
        }

        return (null, null);
    }

    public override void Tick(float dt)
    {
        
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