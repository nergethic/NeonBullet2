using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
    public ResourceTypeDrop drop { get; set; }
    private Resource _resource;
    
    List<int> projectilesEntered = new List<int>();

    public override void Initialize(Player player, ProjectileManager projectileManager) {
        drop = (ResourceTypeDrop)Random.Range(0, System.Enum.GetValues(typeof(ResourceTypeDrop)).Length);
        base.Initialize(player, projectileManager);
    }

    public void InitializeResource(Resource resource) => _resource = resource;


    public override void Tick(float dt) {
        base.Tick(dt);
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
            PlayHitEvent();
            if (Health <= 0) {
                isDead = true;
                SpawnDrop();
                ResetSprites();
                PlayDeathEvent();
                Destroy(gameObject);
            }
        }
    }

    private void SpawnDrop() {
        var newResource = Instantiate(_resource, gameObject.transform);
        newResource.transform.parent = gameObject.transform.parent;
    }

    private void ResetSprites() {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
            renderer.sprite = null;
    }
}
