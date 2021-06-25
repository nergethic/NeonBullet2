using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Transform> enemiesSpawnPoints;
    [SerializeField] List<EnemySpawnArea> enemySpawnAreas;
    
    EntitySceneManager entityManager;
    List<Enemy> spawnedEnemies;

    void Start() {
        spawnedEnemies = new List<Enemy>();

        entityManager = GetEntityManager();
        if (entityManager == null)
            return;
        
        if (entityManager.IsInitialized)
            HandleEntityManagerOnOnInitializationCompleted();
        else
            entityManager.OnInitializationCompleted += HandleEntityManagerOnOnInitializationCompleted;
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

    void HandleEntityManagerOnOnInitializationCompleted() {
        entityManager.OnInitializationCompleted -= HandleEntityManagerOnOnInitializationCompleted;
        
        SpawnEnemiesFromSpawnPoints();
        SpawnEnemiesFromSpawnAreas();
        
        foreach (var enemy in spawnedEnemies)
            entityManager.AddEntity(enemy);
    }

    EntitySceneManager GetEntityManager() {
        var masterSystem = FindObjectOfType<MasterSystem>();
        if (masterSystem == null) {
            Debug.LogError("[EnemySpawner]: Couldn't get master system");
            return null;
        }
        
        var manager = masterSystem.TryGetManager<EntitySceneManager>(SceneManagerType.Entity);
        if (manager == null) {
            Debug.LogError("Couldn't get entity manager");
            return null;
        }

        return manager;
    }
}
