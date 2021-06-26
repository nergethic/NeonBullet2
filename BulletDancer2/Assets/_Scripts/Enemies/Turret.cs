using System.Collections;
using UnityEngine;

public class Turret : Enemy {
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float shootingDistance = 20f;
    [SerializeField] float bulletSpeed = 3.2f;
    [SerializeField] float shootFrequency = 2.3f;
    [SerializeField] bool upgraded;
    
    const string SHOOT_BULLET_METHOD_NAME = "ShootBullet";
    
    int counter;
    float timer = 0f;
    
    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        
        InvokeRepeating(SHOOT_BULLET_METHOD_NAME, 1, shootFrequency);
        DeathEvent += () => CancelInvoke(SHOOT_BULLET_METHOD_NAME);
    }
    
    public override void Tick(float dt) {
        base.Tick(dt);
        
        timer += Time.deltaTime;
        if (timer >= shootFrequency) {
            timer -= shootFrequency;
            StartCoroutine(ShootBullet());
        }

        SetCannonRotation();
    }
    
    private void SetCannonRotation() {
        if (player.IsDead)
            return;
        
        Vector2 playerPos = player.transform.position;
        Vector2 cannonPos = cannon.position;
        Vector2 lookDir = cannonPos - playerPos;
        var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90;

        var angles = cannon.eulerAngles;
        cannon.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
    }
    
    IEnumerator ShootBullet() {
        if (player.IsDead)
            yield break;
        
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) > shootingDistance)
            yield break;

        var bulletType = SelectBulletType(counter);
        counter++;

        var playerPos= player.transform.position;
        Vector2 direction = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);
        var bullet = projectileManager.SpawnProjectile(bulletSpawnPoint.position, direction, bulletType, false, bulletSpeed);
        PlayAttackEvent();
        
        if (upgraded) {
            yield return new WaitForSeconds(0.2f);
            bulletType = SelectBulletType(counter);
            counter++;
            var bullet2 = projectileManager.SpawnProjectile(bulletSpawnPoint.position, direction, bulletType, false, bulletSpeed);
            PlayAttackEvent();  
        }
    }

    ProjectileType SelectBulletType(int counter) {
        ProjectileType bulletType = ProjectileType.Energy;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Standard;
        return bulletType;
    }
}
