using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Transform> enemiesSpawnPoints;
    [SerializeField] List<EnemySpawnArea> enemySpawnAreas;
    
    // TODO: get EntitySceneManager at the beginning and init spawned entities
    EntitySceneManager entityManager;
    List<Enemy> spawnedEnemies;

    void Start() {
        spawnedEnemies = new List<Enemy>();
        SpawnEnemiesFromSpawnPoints();
        SpawnEnemiesFromSpawnAreas();
        InitializeSpawnedEnemies();
    }

    void SpawnEnemiesFromSpawnPoints() {
        for (int i = 0; i < enemiesSpawnPoints.Count; i++) {
            int randIndex = Random.Range(0, enemies.Count);
            var randomEnemy = enemies[randIndex];
            SpawnEnemy(randomEnemy, enemiesSpawnPoints[i].position);
        }
    }

    void SpawnEnemiesFromSpawnAreas() {
        for (int i = 0; i < enemySpawnAreas.Count; i++) {
            var area = enemySpawnAreas[i];
            SpawnEnemy(area.GetAssignedEnemy(), area.GetRandomPointWithinArea());
        }
    }

    void SpawnEnemy(Enemy enemy, Vector3 position) {
        var createdEnemy = Instantiate(enemy);
        createdEnemy.transform.position = position;
        createdEnemy.transform.SetParent(gameObject.transform);
        spawnedEnemies.Add(createdEnemy);
    }

    void InitializeSpawnedEnemies() {
        var masterSystem = FindObjectOfType<MasterSystem>();
        if (masterSystem != null) {
            entityManager = masterSystem.TryGetManager<EntitySceneManager>(SceneManagerType.Entity);
            if (entityManager == null) {
                Debug.LogError("Couldn't get entity manager");
            } else {
                if (entityManager.IsInitialized)
                    HandleEntityManagerOnOnInitializationCompleted();
                else
                    entityManager.OnInitializationCompleted += HandleEntityManagerOnOnInitializationCompleted;
            }
        } else
            Debug.LogError("Couldn't get master system");
    }
    
    void HandleEntityManagerOnOnInitializationCompleted() {
        entityManager.OnInitializationCompleted -= HandleEntityManagerOnOnInitializationCompleted;
        foreach (var enemy in spawnedEnemies)
            entityManager.AddEntity(enemy);
    }
}
