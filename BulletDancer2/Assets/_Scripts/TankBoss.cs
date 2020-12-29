using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Entity {
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;

    List<int> projectilesEntered = new List<int>();
    
    int counter;

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        
        InvokeRepeating("ShootBullet", 1, 2.3f);
    }

    public override void Tick(float dt) {
        base.Tick(dt);

        SetCannonRotation();
    }
    
    private void SetCannonRotation() {
        Vector2 playerPos = playerTransform.position;
        Vector2 cannonPos = cannon.position;
        Vector2 lookDir = cannonPos - playerPos;
        var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        var angles = cannon.eulerAngles;
        cannon.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
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
                Destroy(gameObject);
            }
            
            Destroy(bullet.gameObject);
        }
    }
    
    void ShootBullet() {
        ProjectileType bulletType = ProjectileType.Standard;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Energy;
        
        var (bulletGO, bullet) = projectileManager.SpawnProjectile(bulletSpawnPoint.position, bulletType, false);
        Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        bullet.SetDirection(direction);

        counter++;
    }
}
