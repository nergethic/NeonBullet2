using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
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

        int id = bullet.GetHashCode();
        if (projectilesEntered.Contains(id))
            return;

        projectilesEntered.Add(id);


        if (levelOfDestruction != 0)
        {
            levelOfDestruction -= bullet.projectileData.damage;
            Destroy(bullet.gameObject);

            if (levelOfDestruction <= 0)
            {
                Destroy(gameObject);
                return;
            }

            spriteRenderer.sprite = sprites[levelOfDestruction - 1];
        }
    }
}
