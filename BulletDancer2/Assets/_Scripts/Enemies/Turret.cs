using UnityEngine;

public class Turret : Enemy {
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float shootingDistance = 20f;
    [SerializeField] float bulletSpeed = 3.2f;
    
    int counter;
    private const string SHOOT_BULLET_METHOD_NAME = "ShootBullet";
    
    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        
        InvokeRepeating(SHOOT_BULLET_METHOD_NAME, 1, 2.3f);
        DeathEvent += () => CancelInvoke(SHOOT_BULLET_METHOD_NAME);
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
    
    void ShootBullet() {
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) > shootingDistance)
            return;

        ProjectileType bulletType = ProjectileType.Standard;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Energy;
        
        var bullet = projectileManager.SpawnProjectile(bulletSpawnPoint.position, bulletType, false, bulletSpeed);
        Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        bullet.SetDirection(direction);
        PlayAttackEvent();
        counter++;
    }
}
