using UnityEngine;

public class EnemySpawnArea : MonoBehaviour {
    [SerializeField] BoxCollider area;
    [SerializeField] Enemy enemy;

    public Bounds GetBounds() => area.bounds;
    public Enemy GetAssignedEnemy() => enemy;

    public Vector2 GetRandomPointWithinArea() {
        Bounds bounds = area.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.max.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
