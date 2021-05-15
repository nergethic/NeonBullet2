using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<Transform> enemiesSpawns;

    void Awake() {
        for (int i = 0; i < enemiesSpawns.Count; i++) {
            int randIndex = Random.Range(0, enemies.Count - 1);
            var randomEnemy = enemies[randIndex];
            var createdEnemy = Instantiate(randomEnemy);
            createdEnemy.transform.position = enemiesSpawns[i].position;
        }
    }
}
