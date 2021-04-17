using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSystem : MonoBehaviour {
    [SerializeField] List<SceneManager> managers = new List<SceneManager>();
    public Player player;
    public PlayerController playerController;
    Dictionary<SceneManagerType, SceneManager> initializedSceneManagers;

    SceneManagerData data = new SceneManagerData();
    bool isInitialized;
    
    WaitForSeconds WaitForSomeTime = new WaitForSeconds(0.1f);
    public SceneManager TryGetManager(SceneManagerType managerType) => initializedSceneManagers.ContainsKey(managerType) ? initializedSceneManagers[managerType] : null;
    
    void Awake() {
        data.player = player;
        data.playerController = playerController;
        
        initializedSceneManagers = new Dictionary<SceneManagerType, SceneManager>();
        CollectManagers();
        StartCoroutine(InitializeManagers());
    }

    void Update() {
        if (!isInitialized)
            return;
        
        foreach (var manager in managers) 
            manager.Tick(Time.deltaTime);
    }

    IEnumerator InitializeManagers() {
        Debug.Log("[MasterSystem]: Waiting for manager initialization...");
        const int MAX_NUMBER_OF_WAIT_INTERVALS = 10;
        foreach (var manager in managers) {
            manager.Init(this, data);
            if (TryToAddInitializedManager(manager))
                continue;

            bool managerWasInitialized = false;
            for (int i = 0; i < MAX_NUMBER_OF_WAIT_INTERVALS; i++) {
                yield return WaitForSomeTime;

                if (TryToAddInitializedManager(manager)) {
                    managerWasInitialized = true;
                    break;
                }
            }

            if (!managerWasInitialized) {
                Debug.Log($"[MasterSystem]: Manager '{manager.name} failed, taking too long to initialize'");
                yield return null;
            }
        }

        Debug.Log("[MasterSystem]: All managers initialized successfully");
        isInitialized = true;
        yield return null;
    }

    bool TryToAddInitializedManager(SceneManager manager) {
        if (manager.GetInitializationState() == ManagerInitializationState.COMPLETED) {
            Debug.Log($"-- manager '{manager.name}' init completed");
            initializedSceneManagers.Add(manager.Type, manager);
            return true;
        }

        return false;
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
    Entity,
    Projectile
}

public class SceneManagerData {
    public List<SceneManagerType> previousManagers = new List<SceneManagerType>();
    public Player player;
    public PlayerController playerController;
}