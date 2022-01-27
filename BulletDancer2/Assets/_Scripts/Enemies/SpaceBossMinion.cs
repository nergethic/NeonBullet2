using System;
using System.Collections;
using UnityEngine;

public class SpaceBossMinion : Enemy {
    //[SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float shootingDistance = 20f;
    [SerializeField] float bulletSpeed = 3.2f;
    [SerializeField] float shootFrequency = 2.3f;
    [SerializeField] bool upgraded;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] new Collider2D collider;
    
    const string SHOOT_BULLET_METHOD_NAME = "ShootBullet";
    
    Vector3 spawnPos;
    int counter;
    float timer = 0f;
    bool isActivated;

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);

        isActivated = false;
        spawnPos = transform.position;
        StartCoroutine(ActivateAfterSomeTime());
        InvokeRepeating(SHOOT_BULLET_METHOD_NAME, 1, shootFrequency);
        DeathEvent += () => CancelInvoke(SHOOT_BULLET_METHOD_NAME);
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    public override void Tick(float dt) {
        base.Tick(dt);
        
        timer += Time.deltaTime;
        if (timer >= shootFrequency) {
            timer -= shootFrequency;
            StartCoroutine(ShootBullet());
        }

        SetCannonRotation();

        if (isActivated)
            Move();
    }

    void Move() {
        var noiseValX = (Mathf.PerlinNoise( spawnPos.x*10f+Time.realtimeSinceStartup, spawnPos.x*10f+Time.realtimeSinceStartup) - 0.5f) * 2.0f; // (-1:1)
        var noiseValY = (Mathf.PerlinNoise(spawnPos.y*10f+Time.realtimeSinceStartup+100f, spawnPos.y*10f+Time.realtimeSinceStartup) - 0.5f) * 2.0f; // (-1:1)
        var pos = transform.position;
        var diff = (spawnPos - transform.position);
        if (diff.magnitude > 1f)
            diff.Normalize();
        
        pos.x += Time.deltaTime*noiseValX;
        pos.y += Time.deltaTime*noiseValY;

        float attractionStrength = diff.sqrMagnitude > 1f ? 1f : 0.3f;
        pos += diff * Time.deltaTime * attractionStrength;
        
        transform.position = pos;
    }
    
    private void SetCannonRotation() {
        if (player.IsDead)
            return;
            
        Vector2 cannonPos = transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 lookDir = cannonPos - playerPos;
        var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;

        var angles = transform.rotation.eulerAngles;
        transform.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
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
        if (animator != null) {
            animator.Play("Shoot");
        }
        PlayAttackEvent();
        
        if (upgraded) {
            yield return new WaitForSeconds(0.2f);
            bulletType = SelectBulletType(counter);
            counter++;
            var bullet2 = projectileManager.SpawnProjectile(bulletSpawnPoint.position, direction, bulletType, false, bulletSpeed);
            PlayAttackEvent();  
        }
    }

    IEnumerator ActivateAfterSomeTime() {
        yield return new WaitForSeconds(1.5f);
        spawnPos = transform.position;
        isActivated = true;
        yield return null;
    }

    ProjectileType SelectBulletType(int counter) {
        ProjectileType bulletType = ProjectileType.Energy;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Standard;
        return bulletType;
    }
}
