using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class BulletBoss : Entity {
    const float PLAYER_DIST_TO_ACTIVATE = 2.8f;
    const float ROTATION_SPEED = 35f;
    
    [SerializeField] Transform mainBulletSpawnPoint;
    [SerializeField] Transform mainBulletSpawnPoint2;
    [SerializeField] Transform leftBulletSpawnPoint;
    [SerializeField] Transform rightBulletSpawnPoint;
    [SerializeField] Transform teleport;
    [SerializeField] SpriteRenderer bossBase;
    [SerializeField] SpaceBossMinion minion;

    List<int> projectilesEntered = new();
    
    readonly Quaternion leftSpawnWide    = Quaternion.Euler(0f, 0f, 138.87f);
    readonly Quaternion leftSpawnNarrow  = Quaternion.Euler(0f, 0f, 170f);
    readonly Quaternion rightSpawnWide   = Quaternion.Euler(0f, 0f, -170f);
    readonly Quaternion rightSpawnNarrow = Quaternion.Euler(0f, 0f, 180f);

    BulletBossStage currentStage;
    Coroutine mainCannonShoothingCor = null;
    Coroutine sideCannonsShoothingCor = null;
    
    float timer;
    float oldZBaseRotation;
    float baseRotationT;
    bool shouldCatchUpPlayer;
    const float speed = 0.8f;
    const float SHOOTING_DISTANCE = 40f;
    const float bulletFrequency = 0.045f;
    readonly WaitForSeconds WaitSomeTime = new(bulletFrequency);

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        base.Initialize(player, projectileManager);
        Assert.IsTrue(Health >= 5);
        SwitchStage(BulletBossStage.Inactive);
        teleport.localScale = Vector3.zero;
        teleport.SetParent(null);
        bossBase.enabled = false;
        player.PreHitEvent += PlayerOnHitEvent;
    }

    void OnDisable() {
        player.PreHitEvent -= PlayerOnHitEvent;
        StopAllCoroutines();
    }

    public void NotifyAboutDeadMinion() {
        var entitySystem = FindObjectOfType<EntitySceneManager>();
        if (entitySystem != null) {
            var spawnForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(3f, 5.5f);
            var m1 = SpawnMinion(spawnForce);
            entitySystem.AddEntity(m1);
        }
    }

    void SwitchStage(BulletBossStage stage) {
        switch (stage) {
            case BulletBossStage.Inactive:
                if (mainCannonShoothingCor != null) {
                    StopCoroutine(mainCannonShoothingCor);
                    mainCannonShoothingCor = null;
                }
                
                if (sideCannonsShoothingCor != null) {
                    StopCoroutine(sideCannonsShoothingCor);
                    sideCannonsShoothingCor = null;
                }
                break;
            
            case BulletBossStage.Stage1:
                teleport.transform.DOScale(new Vector3(.62f, .62f, .62f), 1.2f).OnComplete(() => {
                    bossBase.enabled = true;
                    bossBase.sortingOrder = 1;
                    StartCoroutine(ClearPlayerEnergyPoints());
                    teleport.transform.DOScale(Vector3.zero, 0.8f).OnComplete(() => {
                        bossBase.sortingOrder = 10;
                        //mainCannonShoothingCor = StartCoroutine(StartShootingMainCannon());
                        sideCannonsShoothingCor = StartCoroutine(StartShootingSideCannons());
                        StartCoroutine(SpawnMinions());
                    });
                });
                break;
            
            case BulletBossStage.Stage2:
                if (mainCannonShoothingCor != null) {
                    StopCoroutine(mainCannonShoothingCor);
                    mainCannonShoothingCor = null;
                }
                break;
        }
        
        currentStage = stage;
    }
    
    IEnumerator StartShootingMainCannon() {
        float duration = 0.8f;
        while (true) {
            float totalTime = 0;
            while (totalTime <= duration) {
                if (Vector3.SqrMagnitude(player.transform.position - transform.position) > SHOOTING_DISTANCE) {
                    yield return null;
                }
                totalTime += Time.deltaTime;
                ShootMainBullets();
                yield return WaitSomeTime;
                totalTime += bulletFrequency;
            }
        }
    }
    
    IEnumerator StartShootingSideCannons() {
        float duration = 0.8f;
        while (true) {
            float totalTime = 0;
            while (totalTime <= duration) {
                if (Vector3.SqrMagnitude(player.transform.position - transform.position) > SHOOTING_DISTANCE) {
                    yield return null;
                }
                totalTime += Time.deltaTime;
                ShootSideBullets();
                yield return WaitSomeTime;
                totalTime += bulletFrequency;
            }
        }
    }

    public override void Tick(float dt) {
        base.Tick(dt);

        switch (currentStage) {
            case BulletBossStage.Inactive:
                if (Vector3.Distance(player.transform.position, transform.position) < PLAYER_DIST_TO_ACTIVATE)
                    SwitchStage(BulletBossStage.Stage1);
                break;
            
            case BulletBossStage.Stage1:
                SetCannonRotation(false);
                break;
            
            case BulletBossStage.Stage2:
                break;
        }
        
        mainBulletSpawnPoint.Rotate(Vector3.forward,  Time.deltaTime*360f, Space.Self);
        mainBulletSpawnPoint2.Rotate(Vector3.forward,  Time.deltaTime*360f, Space.Self);
        
        //Debug.LogError(leftBulletSpawnPoint.rotation.eulerAngles.z);
        //leftBulletSpawnPoint.rotation = Quaternion.Lerp(leftSpawnWide, leftSpawnNarrow, 0f);
        //rightBulletSpawnPoint.rotation = Quaternion.Lerp(rightSpawnWide, rightSpawnNarrow, 0f);
    }

    float catchUpTimer = 0f;
    private void SetCannonRotation(bool followPlayer) {
        if (player.IsDead)
            return;
            
        Vector2 cannonPos = transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 lookDir = cannonPos - playerPos;
        
        if (shouldCatchUpPlayer) {
            var angles = transform.rotation.eulerAngles;
            var newZAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
            var finalAngle = Quaternion.Euler(angles.x, angles.y, newZAngle);

            catchUpTimer += Time.deltaTime * 0.5f;
            if (catchUpTimer > 1f) {
                catchUpTimer = 0f;
                shouldCatchUpPlayer = false;
            }
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, finalAngle, catchUpTimer);
        } else if (followPlayer) {
            var newAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
            var angles = transform.rotation.eulerAngles;
            transform.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
        } else {
            var angles = transform.eulerAngles;
            var newAngle = angles.z + Time.deltaTime * ROTATION_SPEED;
            transform.eulerAngles = new Vector3(angles.x, angles.y, newAngle);
        }
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
    
    void ShootMainBullets() {
        var bullet = projectileManager.SpawnProjectile(mainBulletSpawnPoint.position, mainBulletSpawnPoint.up, ProjectileType.StandardOrange, false, 5f);
        var bullet2 = projectileManager.SpawnProjectile(mainBulletSpawnPoint2.position, mainBulletSpawnPoint2.up,ProjectileType.StandardOrange, false, 5f);
    }

    void ShootSideBullets() {
        var bullet3 = projectileManager.SpawnProjectile(leftBulletSpawnPoint.position, leftBulletSpawnPoint.up, ProjectileType.StandardBlue, false, 5f);
        var bullet4 = projectileManager.SpawnProjectile(rightBulletSpawnPoint.position, rightBulletSpawnPoint.up, ProjectileType.StandardBlue, false, 5f);
    }

    SpaceBossMinion SpawnMinion(Vector2 force) {
        var minionInst = Instantiate(minion, transform.position, Quaternion.identity);
        minionInst.SetBossOwner(this);
        var rb = minionInst.GetComponentInChildren<Rigidbody2D>();
        if (rb != null) {
            rb.AddForce(force, ForceMode2D.Impulse);
        } else {
            Debug.LogError("Couldn't find rigidbody");
        }

        return minionInst;
    }

    IEnumerator SpawnMinions() {
        Vector2 cannonPos = transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 lookDir = cannonPos - playerPos;

        var entitySystem = FindObjectOfType<EntitySceneManager>();
        if (entitySystem != null) {
            var m1 = SpawnMinion(Vector2.left*Random.Range(3f, 5.5f));
            var m2 = SpawnMinion(Vector2.up*Random.Range(3f, 5.5f));
            var m3 = SpawnMinion(Vector2.down*Random.Range(3f, 5.5f));
            entitySystem.AddEntity(m1);
            entitySystem.AddEntity(m2);
            entitySystem.AddEntity(m3);
        }

        yield return null;
    }
    
    IEnumerator ClearPlayerEnergyPoints() {
        while (player.Energy > 0) {
            if (player.IsDead)
                yield break;

            player.Energy--;
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    void PlayerOnHitEvent(ProjectileData projectileData) {
        if (!projectileData.ownedByPlayer && projectileData.typeMask == (int)ProjectileType.StandardBlue) {
            catchUpTimer = 0f;
            shouldCatchUpPlayer = true;
        }
    }
}

enum BulletBossStage {
    Inactive = 0,
    Stage1,
    Stage2
}