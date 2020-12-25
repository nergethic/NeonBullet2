using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Transform myTransform;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float maxAirTime = 3f;
    [SerializeField] ProjectileType type;

    public ProjectileData projectileData;

    private int defaultLayerMask;
    private int playerLayerMask;
    private int projectileLayerMask;
    
    private float airTime = 0f;
    private Vector2 dir;
    
    public ProjectileType GetType() => type;

    void Start() {
        defaultLayerMask = LayerMask.NameToLayer("Default");
        playerLayerMask = LayerMask.NameToLayer("Player");
        projectileLayerMask = LayerMask.NameToLayer("Projectile");
    }

    public void Initialize(Vector2 dir, bool ownedByPlayer) {
        this.dir = dir.normalized;

        projectileData.ownedByPlayer = ownedByPlayer;
        projectileData.typeMask = (int)type;
        projectileData.damage = 1;
        projectileData.speed = speed;
    }

    public void SetDirection(Vector2 dir) {
        this.dir = dir.normalized;
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log(other.tag);
        if (other.gameObject.layer != playerLayerMask && other.gameObject.layer != projectileLayerMask && other.gameObject.layer != defaultLayerMask)
            Destroy(gameObject);
    }
    
    void Update() {
        airTime += Time.deltaTime;
        if (airTime >= maxAirTime) {
            Destroy(gameObject);    
        }
        
        var newPos = myTransform.position;
        newPos.x += Time.deltaTime * dir.x * projectileData.speed;
        newPos.y += Time.deltaTime * dir.y * projectileData.speed;
        
        myTransform.position = newPos;
    }
}

public struct ProjectileData {
    public bool ownedByPlayer;
    public int typeMask; // NOTE: for now there are no mixed types
    public int damage;
    public float speed;
}