using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Transform myTransform;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float maxAirTime = 3f;
    [SerializeField] ProjectileType type;
    [SerializeField] int damage = 1;
    public event Action DestroyEvent;
    public ProjectileData projectileData;

    private int defaultLayerMask;
    private int playerLayerMask;
    private int projectileLayerMask;
    private int enemyCollisionLayerMask;
    
    private float airTime = 0f;
    
    public ProjectileType Type() => type;

    void Start() {
        defaultLayerMask = LayerMask.NameToLayer("Default");
        playerLayerMask = LayerMask.NameToLayer("Player");
        projectileLayerMask = LayerMask.NameToLayer("Projectile");
        enemyCollisionLayerMask = LayerMask.NameToLayer("EnemyCollision");
    }

    public void Initialize(ProjectileData projectileData) {
        projectileData.typeMask = (int)type;
        projectileData.damage = damage;
        this.projectileData = projectileData;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!projectileData.ownedByPlayer && other.gameObject.layer == enemyCollisionLayerMask)
            return;

        if (other.gameObject.layer == projectileLayerMask)
            return;
        
        if (other.gameObject.layer != playerLayerMask && other.gameObject.layer != defaultLayerMask)
        {
            DestroyEvent?.Invoke();
            Destroy(gameObject);
        }
    }
    
    void Update() {
        airTime += Time.deltaTime;
        if (airTime >= maxAirTime) {
            DestroyEvent?.Invoke();
            Destroy(gameObject);    
        }
        
        var newPos = myTransform.position;
        newPos.x += Time.deltaTime * projectileData.dir.x * projectileData.speed;
        newPos.y += Time.deltaTime * projectileData.dir.y * projectileData.speed;
        
        myTransform.position = newPos;
    }
}

public struct ProjectileData {
    public bool ownedByPlayer;
    public int typeMask; // NOTE: for now there are no mixed types
    public int damage;
    public float speed;
    public Vector2 dir;
}