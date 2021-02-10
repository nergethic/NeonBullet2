using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterSystem : MonoBehaviour {
    [SerializeField] List<SceneManager> managers = new List<SceneManager>();
    public Player player;
    public PlayerController playerController;

    SceneManagerData data = new SceneManagerData();
    bool isInitialized;
    
    WaitForSeconds WaitForSomeTime = new WaitForSeconds(0.1f);
    
    void Awake() {
        data.player = player;
        data.playerController = playerController;
        
        CollectManagers();
        InitializeManagers();
        StartCoroutine(WaitForInitialization());
    }

    void InitializeManagers() {
        foreach (var manager in managers) {
            manager.Init(this, data);
        }
    }
    
    void Update() {
        if (!isInitialized)
            return;
        
        foreach (var manager in managers) 
            manager.Tick(Time.deltaTime);
    }

    public ProjectileManager GetProjectileManager() => managers.OfType<ProjectileManager>().FirstOrDefault();

    IEnumerator WaitForInitialization() {
        Debug.Log("[MasterSystem]: Waiting for manager initialization...");
        while (true) {
            bool allManagersAreInitialized = true;
            foreach (var manager in managers) {
                if (manager.GetInitializationState() != ManagerInitializationState.INITIALIZED) {
                    allManagersAreInitialized = false;
                    break;
                }
            }

            if (allManagersAreInitialized) {
                isInitialized = true;
                break;
            }

            yield return WaitForSomeTime;
        }
        
        Debug.Log("[MasterSystem]: All managers initialized successfully");
        yield return null;
    }
    
#if UNITY_EDITOR
    [ContextMenu("Collect Managers")]
    public void CollectManagers() {
        if (managers == null)
            managers = new List<SceneManager>();
        managers.Clear();
        
        var childrenManagers = gameObject.GetComponentsInChildren<SceneManager>();
        foreach (var m in childrenManagers) {
            managers.Add(m);
        }
    }
#endif
}

public enum SceneManagerType {
    Unknown = 0,
    Item,
    Entity
}

public class SceneManagerData {
    public List<SceneManagerType> previousManagers = new List<SceneManagerType>();
    public Player player;
    public PlayerController playerController;
}