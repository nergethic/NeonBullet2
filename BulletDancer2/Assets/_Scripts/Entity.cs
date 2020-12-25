using UnityEngine;

public class Entity : MonoBehaviour {
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Sprite sprite;
    [SerializeField] SpriteRenderer renderer;

    ProjectileManager projectileManager;
    Player player;
    Transform playerTransform;
    
    public int Health = 4;
    public int MaxHealth = 4;

    public void Initialize(Player player, ProjectileManager projectileManager) {
        SetSprite(sprite);
        this.player = player;
        playerTransform = player.transform;
        this.projectileManager = projectileManager;
    }

    void Start() {
        InvokeRepeating("ShootBullet", 1, 1.3f);
    }

    int counter;
    void ShootBullet() {
        ProjectileType bulletType = ProjectileType.Standard;
        if (counter % 2 == 0)
            bulletType = ProjectileType.Energy;
        
        var (bulletGO, bullet) = projectileManager.SpawnProjectile(transform.position, bulletType, false);
        Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        bullet.SetDirection(direction);

        counter++;
    }
    
    void Update() {
        
    }
    
    void SetSprite(Sprite sprite) {
        if (renderer != null && sprite != null)
            renderer.sprite = sprite;
    }
}

public enum EnemyActionType {
    Wait = 0,
    Follow
}
