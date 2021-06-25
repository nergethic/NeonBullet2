using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSystem : MonoBehaviour {
    public event Action OnSceneManagersInitialized;
    
    const int MAX_NUMBER_OF_WAIT_INTERVALS = 10;
    
    [SerializeField] SceneManager[] managers;
    [SerializeField] ScreenOverlayController screenOverlayController;
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] Player player;
    [SerializeField] PlayerController playerController;
    Dictionary<SceneManagerType, SceneManager> activeSceneManagers;

    SceneManagerData data = new SceneManagerData();
    bool allManagersAreInitialized;
    bool isInitialized;
    
    WaitForSeconds WaitForSomeTime = new WaitForSeconds(0.1f);
    public T TryGetManager<T>(SceneManagerType managerType) where T:SceneManager => activeSceneManagers.ContainsKey(managerType) ? (T)activeSceneManagers[managerType] : null;
    public ScreenOverlayController ScreenOverlayController => screenOverlayController;
    
    void Awake() {
        data.player = player;
        data.playerController = playerController;
        activeSceneManagers = new Dictionary<SceneManagerType, SceneManager>();
        
        ScreenOverlayController.SetDim();

        if (levelGenerator == null) {
            levelGenerator = FindObjectOfType<LevelGenerator>();
            if (levelGenerator == null) {
                Debug.LogError("[MasterSystem]: LevelGenerator component isn't found");
                return;
            }
        }
        levelGenerator.Initialize(this);
        CollectManagers();
        StartCoroutine(InitializeManagers());
    }

    void Update() {
        if (!isInitialized)
            return;

        for (int i = 0; i < managers.Length; i++) {
            var manager = managers[i];
            manager.Tick(Time.deltaTime);
        }
    }

    public void ReloadLevel() {
        StartCoroutine(ReloadLevelCor());
    }

    IEnumerator InitializeManagers() {
        Debug.Log("[MasterSystem]: Waiting for manager initialization...");
        
        foreach (var manager in managers) {
            manager.Init(this, data);

            yield return WaitForManagerInitialization(manager);
            
            switch (manager.InitializationState) {
                case ManagerInitializationState.IN_PROGRESS:
                    Debug.Log($"[MasterSystem]: Manager '{manager.name}' failed, taking too long to initialize, previous manager could failed initialization");
                    break;
                case ManagerInitializationState.NOT_INITIALIZED:
                    Debug.Log($"[MasterSystem]: Manager '{manager.name} state is NOT_INITIALIZED, should be at least IN_PROGRESS");
                    if (activeSceneManagers.ContainsKey(manager.Type))
                        activeSceneManagers.Remove(manager.Type);
                    manager.InformAboutInitializationFailed();
                    break;
            }

            yield return null;
        }

        HandleManagersInitialized();
        yield return null;
    }

    IEnumerator WaitForManagerInitialization(SceneManager manager) {
        for (int i = 0; i < MAX_NUMBER_OF_WAIT_INTERVALS; i++) {
            ManagerInitializationState currentManagerState = manager.InitializationState;
                
            if (currentManagerState == ManagerInitializationState.COMPLETED) {
                Debug.Log($"-- manager '{manager.name}' init completed");
                break;
            }
                
            if (currentManagerState == ManagerInitializationState.FAILED) {
                Debug.LogError($"-- manager '{manager.name}' init failed");
                break;
            }
                
            yield return WaitForSomeTime;
        }
    }
    
    IEnumerator ReloadLevelCor() {
        yield return ScreenOverlayController.FadeOutScreen(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void HandleManagersInitialized() {
        Debug.Log("[MasterSystem]: All managers initialized successfully");
        StartCoroutine(FadeInScreenAndFinishInit());
    }

    IEnumerator FadeInScreenAndFinishInit() {
        yield return ScreenOverlayController.FadeInScreen(1f);
        allManagersAreInitialized = true;
        isInitialized = true;
        OnSceneManagersInitialized?.Invoke();
    }

    bool TryToAddInitializedManager(SceneManager manager) {
        if (manager.InitializationState == ManagerInitializationState.COMPLETED) {
            Debug.Log($"-- manager '{manager.name}' init completed");
            activeSceneManagers.Add(manager.Type, manager);
            return true;
        }

        return false;
    }

    void CollectManagers() {
        managers = gameObject.GetComponentsInChildren<SceneManager>();
        if (managers == null) {
            Debug.LogError("[MasterSystem]: managers are null");
            return;
        }

        foreach (var manager in managers)
            activeSceneManagers.Add(manager.Type, manager);
    }
}

public enum SceneManagerType {
    Unknown = 0,
    Item,
    Entity,
    Projectile
}

public class SceneManagerData {
    public Player player;
    public PlayerController playerController;
}