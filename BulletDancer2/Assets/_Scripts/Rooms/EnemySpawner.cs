using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Transform> enemiesSpawnPoints;
    [SerializeField] List<EnemySpawnArea> enemySpawnAreas;
    [SerializeField] Entity boss;
    [SerializeField] Transform bossSpawn;
    
    EntitySceneManager entityManager;
    List<Entity> spawnedEnemies;

    void Start() {
        spawnedEnemies = new List<Entity>();

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
            SpawnEntity(randomEnemy, enemiesSpawnPoints[i].position);
        }
        
        if (boss == null)
            return;
        
        SpawnEntity(boss, bossSpawn.position);
    }

    void SpawnEnemiesFromSpawnAreas() {
        for (int i = 0; i < enemySpawnAreas.Count; i++) {
            var area = enemySpawnAreas[i];
            SpawnEntity(area.GetAssignedEnemy(), area.GetRandomPointWithinArea());
        }
    }

    void SpawnEntity(Entity entity, Vector3 position) {
        var createdEntity = Instantiate(entity);
        createdEntity.transform.position = position;
        createdEntity.transform.SetParent(gameObject.transform);
        spawnedEnemies.Add(createdEntity);
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
