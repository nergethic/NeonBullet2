using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float shootingDistance = 20f;
    [SerializeField] float bulletSpeed = 3.2f;
    [SerializeField] ResourceTypeDrop drop;
    [SerializeField] Ore ore;
    [SerializeField] Iron iron;
    [SerializeField] Gold gold;
    public event Action DeathEvent;
    public event Action HitEvent;
    public event Action AttackEvent;
    List<int> projectilesEntered = new List<int>();
    
    int counter;
    private const string SHOOT_BULLET_METHOD_NAME = "ShootBullet";

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        
        InvokeRepeating(SHOOT_BULLET_METHOD_NAME, 1, 2.3f);
    }

    public override void Tick(float dt) {
        base.Tick(dt);

        SetCannonRotation();
    }
    
    private void SetCannonRotation() {
        Vector2 playerPos = playerTransform.position;
        Vector2 cannonPos = cannon.position;
        Vector2 lookDir = cannonPos - playerPos;
        var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;

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
            HitEvent();
            if (Health <= 0) {
                isDead = true;
                CancelInvoke(SHOOT_BULLET_METHOD_NAME);
                SpawnDrop();
                DeathEvent();
                Destroy(gameObject, 0.15f);
            }
            
            Destroy(bullet.gameObject);
        }
    }

    private void SpawnDrop()
    {
        Resource newResource = null;
        switch (drop)
        {
            case ResourceTypeDrop.Ore:
                newResource = Instantiate(ore, gameObject.transform);
                newResource.transform.parent = null;
                break;
            case ResourceTypeDrop.Iron:
                newResource = Instantiate(iron, transform);
                newResource.transform.parent = null;
                break;
            case ResourceTypeDrop.Gold:
                newResource = Instantiate(gold, transform);
                newResource.transform.parent = null;
                break;
        }
    }

    void ShootBullet() {
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) > shootingDistance) {
            return;
        }
        ProjectileType bulletType = ProjectileType.Standard;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Energy;
        
        var (bulletGO, bullet) = projectileManager.SpawnProjectile(bulletSpawnPoint.position, bulletType, false, bulletSpeed);
        Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        bullet.SetDirection(direction);
        AttackEvent();
        counter++;
    }
}
