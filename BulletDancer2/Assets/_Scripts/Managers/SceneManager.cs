using System;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour {
    public event Action OnInitializationCompleted;
    public event Action OnInitializationFailed;
    
    [SerializeField] SceneManagerType type;
    
    protected SceneManagerData data;
    protected bool isInitializing = false;
    private ManagerInitializationState initializationState = ManagerInitializationState.NOT_INITIALIZED;

    public SceneManagerType Type => type;
    public ManagerInitializationState InitializationState => initializationState;
    public bool IsInitialized => InitializationState == ManagerInitializationState.COMPLETED;

    public virtual void Init(MasterSystem masterSystem, SceneManagerData data) {
        initializationState = ManagerInitializationState.IN_PROGRESS;
        this.data = data;
    }

    public void InformAboutInitializationFailed() {
        OnInitializationFailed?.Invoke();
    }

    protected void ChangeInitializationState(ManagerInitializationState newState) {
        initializationState = newState;
        if (newState == ManagerInitializationState.COMPLETED)
            OnInitializationCompleted?.Invoke();
    }
    
    public abstract void Tick(float dt);
}

public enum ManagerInitializationState {
    NOT_INITIALIZED = 0,
    IN_PROGRESS,
    COMPLETED,
    FAILED
}