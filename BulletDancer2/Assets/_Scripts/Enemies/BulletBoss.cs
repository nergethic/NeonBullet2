using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : Entity {
    [SerializeField] Transform mainBulletSpawnPoint;
    [SerializeField] Transform mainBulletSpawnPoint2;
    [SerializeField] Transform leftBulletSpawnPoint;
    [SerializeField] Transform rightBulletSpawnPoint;

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

    const float bulletFrequency = 0.045f;
    readonly WaitForSeconds WaitSomeTime = new WaitForSeconds(bulletFrequency);
    IEnumerator StartShooting() {
        float duration = 0.8f;
        while (true) {
            float totalTime = 0;
            while (totalTime <= duration) {
                if (Vector3.SqrMagnitude(player.transform.position - transform.position) > SHOOTING_DISTANCE) {
                    yield return null;
                }
                totalTime += Time.deltaTime;
                ShootBullets();
                yield return WaitSomeTime;
                totalTime += bulletFrequency;
            }
        }

        yield return null;
    }

    [SerializeField] private float param1;
    [SerializeField] private float param2;
    public override void Tick(float dt) {
        base.Tick(dt);
        
        mainBulletSpawnPoint.Rotate(Vector3.forward,  Time.deltaTime*360f, Space.Self);
        mainBulletSpawnPoint2.Rotate(Vector3.forward,  Time.deltaTime*360f, Space.Self);
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
    
    void ShootBullets() {
        var bullet = projectileManager.SpawnProjectile(mainBulletSpawnPoint.position, mainBulletSpawnPoint.up, ProjectileType.StandardOrange, false, 5f);
        var bullet2 = projectileManager.SpawnProjectile(mainBulletSpawnPoint2.position, mainBulletSpawnPoint2.up,ProjectileType.StandardOrange, false, 5f);
        var bullet3 = projectileManager.SpawnProjectile(leftBulletSpawnPoint.position, leftBulletSpawnPoint.up, ProjectileType.StandardBlue, false, 5f);
        var bullet4 = projectileManager.SpawnProjectile(rightBulletSpawnPoint.position, rightBulletSpawnPoint.up, ProjectileType.StandardBlue, false, 5f);
    }
}