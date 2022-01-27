using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablePortalWall : MonoBehaviour
{
    [SerializeField] Portal portal;
    [SerializeField] GameObject effect;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    List<int> projectilesEntered = new List<int>();
    int levelOfDestruction;

    private void Awake()
    {
        levelOfDestruction = sprites.Count;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        var bullet = other.GetComponent<Projectile>();
        if (bullet == null)
            return;

        if (bullet.Type() != ProjectileType.Grenade)
            return;

        int id = bullet.GetHashCode();
        if (projectilesEntered.Contains(id))
            return;

        projectilesEntered.Add(id);


        if (levelOfDestruction != 0)
        {
            levelOfDestruction -= bullet.projectileData.damage;

            if (levelOfDestruction <= 0)
            {
                var newEffect = Instantiate(effect);
                newEffect.transform.position = gameObject.transform.position;
                var spawnedPortal = Instantiate(portal);
                spawnedPortal.transform.position = gameObject.transform.position;
                Destroy(gameObject); ;
                return;
            }

            spriteRenderer.sprite = sprites[levelOfDestruction - 1];
        }
    }
}
