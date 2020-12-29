using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Entity {
    [SerializeField] Transform tankBase;
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;

    List<int> projectilesEntered = new List<int>();
    
    int counter;
    float timer;
    float oldZBaseRotation;
    float baseRotationT;
    TankDirection currentDirection = TankDirection.Left;

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        
        // InvokeRepeating("ShootBullet", 1, 2.3f);
    }

    public override void Tick(float dt) {
        base.Tick(dt);

        SetCannonRotation();

        var pos = transform.position;
        const float speed = 0.2f;
        float increment = dt * speed;

        timer += dt;
        if (timer >= 1.0f) {
            SwitchDirection();
            baseRotationT = 0f;
            oldZBaseRotation = tankBase.eulerAngles.z;
            timer = 0f;
        }

        baseRotationT += dt*3.0f;
        Mathf.Clamp01(baseRotationT);
        
        switch (currentDirection) {
            case TankDirection.Up:
                pos.y += increment;
                break;
            
            case TankDirection.Down:
                pos.y -= increment;
                break;
            
            case TankDirection.Left:
                pos.x -= increment;
                break;
            
            case TankDirection.Right:
                pos.x += increment;
                break;
        }

        SetBaseRotation();
        transform.position = pos;
    }

    void SwitchDirection() {
        switch (currentDirection) {
            case TankDirection.Up:
                currentDirection = TankDirection.Left;
                break;
            
            case TankDirection.Down:
                currentDirection = TankDirection.Right;
                break;
            
            case TankDirection.Left:
                currentDirection = TankDirection.Down;
                break;
            
            case TankDirection.Right:
                currentDirection = TankDirection.Up;
                break;
        }
    }
    
    private void SetCannonRotation() {
        Vector2 playerPos = playerTransform.position;
        Vector2 cannonPos = cannon.position;
        Vector2 lookDir = cannonPos - playerPos;
        var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        var angles = cannon.eulerAngles;
        cannon.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
    }
    
    private void SetBaseRotation() {
        Vector2 movementDir = Vector2.zero;
        const float increment = 10f;
        
        switch (currentDirection) {
            case TankDirection.Up:
                movementDir.y += increment;
                break;
            
            case TankDirection.Down:
                movementDir.y -= increment;
                break;
            
            case TankDirection.Left:
                movementDir.x -= increment;
                break;
            
            case TankDirection.Right:
                movementDir.x += increment;
                break;
        }
        
        var angles = tankBase.eulerAngles;
        var newAngle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
        
        if (newAngle <= 0f)
            newAngle += 360f;

        if (oldZBaseRotation <= 0 || (oldZBaseRotation+180f) < newAngle) {
            oldZBaseRotation += 360f;
        }
        
        newAngle = Mathf.Lerp(oldZBaseRotation, newAngle, baseRotationT);

        tankBase.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
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

public enum TankDirection {
    NoDirection = 0,
    Up,
    Down,
    Left,
    Right
}
