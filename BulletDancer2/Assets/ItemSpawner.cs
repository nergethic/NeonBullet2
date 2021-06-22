using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] List<Transform> itemsSpawnPoints;

    ItemSceneManager itemManager;
    List<Item> spawnedItems;

    void Start()
    {
        spawnedItems = new List<Item>();
        SpawnItemsFromSpawnPoints();
        InitializeSpawnedItems();
    }

    void SpawnItemsFromSpawnPoints()
    {
        for (int i = 0; i < itemsSpawnPoints.Count; i++)
        {
            int randIndex = Random.Range(0, items.Count);
            var randomItem = items[randIndex];
            SpawnEnemy(randomItem, itemsSpawnPoints[i].position);
        }
    }


    void SpawnEnemy(Item enemy, Vector3 position)
    {
        var createdItem = Instantiate(enemy);
        createdItem.transform.position = position;
        spawnedItems.Add(createdItem);
    }

    void InitializeSpawnedItems()
    {
        var masterSystem = GetComponent<MasterSystem>();
        if (masterSystem != null)
        {
            itemManager = masterSystem.TryGetManager<ItemSceneManager>(SceneManagerType.Item);
            if (itemManager == null)
            {
                Debug.LogError("Couldn't get item manager");
            }
            else
            {
                if (itemManager.IsInitialized)
                    HandleEntityManagerOnOnInitializationCompleted();
                else
                    itemManager.OnInitializationCompleted += HandleEntityManagerOnOnInitializationCompleted;
            }
        }
    }

    void HandleEntityManagerOnOnInitializationCompleted()
    {
        itemManager.OnInitializationCompleted -= HandleEntityManagerOnOnInitializationCompleted;
        foreach (var item in spawnedItems)
            itemManager.AddItem(item);
    }
}
