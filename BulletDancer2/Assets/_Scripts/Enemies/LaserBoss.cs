using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : Entity {
    [SerializeField] List<Transform> bulletSpawnPoints;
    [SerializeField] Transform spiralBulletDirection;
    
    readonly WaitForSeconds WaitSomeTime = new WaitForSeconds(0.1f);
    readonly WaitForSeconds WaitSomeTime2 = new WaitForSeconds(0.06f);

    List<int> projectilesEntered = new List<int>();
    
    float timer;
    float oldZBaseRotation;
    float baseRotationT;
    const float speed = 0.8f;
    const float SHOOTING_DISTANCE = 40f;

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        StartCoroutine(StartShooting());
    }

    IEnumerator StartShooting() {
        float duration = 0.8f;
        while (true) {
            float totalTime = 0;
            int typeIndex = 0;
            while (totalTime <= duration) {
                if (Vector3.SqrMagnitude(player.transform.position - transform.position) > SHOOTING_DISTANCE) {
                    yield return null;
                }
                totalTime += Time.deltaTime;
                if (typeIndex == 3)
                    ShootBullets(ProjectileType.Standard2, spiralBulletDirection.up);
                else
                    ShootBullets(ProjectileType.Standard2, spiralBulletDirection.up);
                yield return WaitSomeTime;
                totalTime += 0.045f;
                typeIndex++;
                if (typeIndex >= 4)
                    typeIndex = 0;
            }
            
            totalTime = 0;
            while (totalTime <= duration) {
                if (Vector3.SqrMagnitude(player.transform.position - transform.position) > SHOOTING_DISTANCE) {
                    yield return null;
                }
                totalTime += Time.deltaTime;
                ShootBullets(ProjectileType.Standard2, spiralBulletDirection.up);
                yield return WaitSomeTime2;
                totalTime += 0.03f;
                typeIndex++;
                if (typeIndex >= 4)
                    typeIndex = 0;
            }
            // ShootQuadrupleBullet();
            //yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    [SerializeField] private float param1;
    [SerializeField] private float param2;
    public override void Tick(float dt) {
        base.Tick(dt);
        
        spiralBulletDirection.Rotate(Vector3.forward,  Mathf.Sin(Time.time*param1)*Time.deltaTime*param2, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var bullet = other.GetComponent<Projectile>();
        if (bullet == null)
            return;
        
        int id = bullet.GetHashCode();
        if (projectilesEntered.Contains(id))
            return;

        projectilesEntered.Add(id);
        
        if (bullet.projectileData.ownedByPlayer) {
            Health -= bullet.projectileData.damage;
            if (Health <= 0) {
                isDead = true;
                Destroy(gameObject);
            }
            
            Destroy(bullet.gameObject);
        }
    }
    
    void ShootBullets(ProjectileType projectileType, Vector3 bulletDir) {
        for (int i = 0; i < bulletSpawnPoints.Count; i++) {
            var bulletSpawnPoint = bulletSpawnPoints[i];
            var bullet = projectileManager.SpawnProjectile(bulletSpawnPoint.position, bulletDir, projectileType, false, 5f);
        }
    }
}