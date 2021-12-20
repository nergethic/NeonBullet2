using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Entity {
    [SerializeField] Transform tankBase;
    [SerializeField] Transform cannon;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject explosion;

    const float speed = 0.8f;
    const float SHOOTING_DISTANCE = 35f;
    const float MOVEMENT_TIME = 2.5f;
    
    readonly WaitForSeconds WaitSomeTime = new WaitForSeconds(0.06f);
    List<int> projectilesEntered = new List<int>();
    TankDirection currentDirection = TankDirection.Left;
    Coroutine cor;
    float timer;
    float oldZBaseRotation;
    float baseRotationT;

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        cor = StartCoroutine(StartShooting());
    }
    
    IEnumerator StartShooting() {
        float duration = 0.8f;
        int quadrupleShootCount = 0;
        int uziShootCount = 0;
        while (true) {
            if (player.IsDead)
                yield break;
            
            if (Vector3.SqrMagnitude(player.transform.position - transform.position) < SHOOTING_DISTANCE) {
                if (uziShootCount % 4 == 0) {
                    float totalTime = 0;
                    while (totalTime <= 1f) {
                        totalTime += Time.deltaTime;
                        ShootBullet(false, Mathf.Clamp01(1f-totalTime+0.4f));
                        yield return new WaitForSeconds(0.15f);
                        totalTime += 0.15f;
                    }
                    ShootQuadrupleBullet(quadrupleShootCount % 4);
                    quadrupleShootCount++;
                    uziShootCount++;
                    yield return new WaitForSeconds(0.5f);
                } else {
                    float totalTime = 0;
                    while (totalTime <= duration) {
                        totalTime += Time.deltaTime;
                        ShootBullet(true);
                        yield return WaitSomeTime;
                        totalTime += 0.045f;
                    }

                    uziShootCount++;
                    
                    ShootCircle(10, 2, false);  
                    yield return new WaitForSeconds(0.3f);
                    ShootCircle(8, 4, true);
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    
    public override void Tick(float dt) {
        base.Tick(dt);

        SetCannonRotation();

        var pos = transform.position;
        float increment = dt * speed;

        timer += dt;
        if (timer >= MOVEMENT_TIME) {
            SwitchDirection();
            baseRotationT = 0f;
            oldZBaseRotation = tankBase.eulerAngles.z;
            timer -= MOVEMENT_TIME;
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
        if (player.IsDead)
            return;
        
        Vector2 playerPos = player.transform.position;
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
        if (isDead || !bullet.projectileData.ownedByPlayer)
            return;
        
        Health -= bullet.projectileData.damage;
        Destroy(bullet.gameObject);
        PlayHitEvent();

        if (Health <= 0) {
            isDead = true;
            //PlayDeathEvent();
            if (cor != null) {
                StopCoroutine(cor);
                cor = null;
            }
            var explosionInstance = Instantiate(explosion, transform);
            explosionInstance.transform.localScale = new Vector3(12, 12);
            explosionInstance.transform.parent = null;
            StartCoroutine(HandleDeathCor());
        }
    }

    void ShootBullet(bool randomizeDirection, float speedMultiplier = 1f) {
        PlayAttackEvent();
        var playerPos = player.transform.position;
        if (randomizeDirection) {
            var acc = player.Controller.Acceleration * Time.deltaTime * 0.2f;
            playerPos.x += acc.x;
            playerPos.y += acc.y;
            playerPos.x += Random.Range(-0.2f, 0.2f);
            playerPos.y += Random.Range(-0.2f, 0.2f);
        }
        var dirToPlayer = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);
        var bullet = projectileManager.SpawnProjectile(bulletSpawnPoint.position, dirToPlayer, ProjectileType.Standard, false, 4.5f*speedMultiplier);
    }
    
    void ShootQuadrupleBullet(int energyBulletIndex) {
        for (int i = 0; i < 4; i++) {
            var bulletSpawnPos = bulletSpawnPoint.position;
            var bulletType = i == energyBulletIndex ? ProjectileType.Energy : ProjectileType.Standard;
            Vector2 dir = Vector2.zero;
            if (i == 0)
                dir = Vector2.left;
            else if (i == 1)
                dir = Vector2.up;
            else if (i == 2)
                dir = Vector2.right;
            else if (i == 3)
                dir = Vector2.down;
            
            var bullet = projectileManager.SpawnProjectile(bulletSpawnPos, dir, bulletType, false, 3f);
            bullet.gameObject.transform.localScale *= 2f;
        }
    }

    int shootCircleCount = 0;
    void ShootCircle(int burstsCount, int bulletCountInBurst, bool hasEnergyBullets) {
        for (int i = 0; i < burstsCount; i++) {
            float angle = i * (360f/burstsCount) * Mathf.Deg2Rad;
            var projectileType = shootCircleCount % 8 == i ? ProjectileType.Energy : ProjectileType.Standard;
            if (!hasEnergyBullets)
                projectileType = ProjectileType.Standard;
            for (int j = 0; j < bulletCountInBurst; j++) {
                float angleOffset = j * 5f * Mathf.Deg2Rad;
                var dir = new Vector2(Mathf.Cos(angle+angleOffset), Mathf.Sin(angle+angleOffset));
                var bullet = projectileManager.SpawnProjectile(bulletSpawnPoint.position, dir, projectileType, false, 1.8f);
            }
        }

        shootCircleCount++;
    }
    
    IEnumerator HandleDeathCor() {
        var levelGenerator = FindObjectOfType<LevelGenerator>();
        if (levelGenerator != null) {
            yield return new WaitForSeconds(3f);
            levelGenerator.GenerateLevel();
            player.transform.position = player.startPosition;
        }
        Destroy(gameObject);
    }
}

public enum TankDirection {
    NoDirection = 0,
    Up,
    Down,
    Left,
    Right
}