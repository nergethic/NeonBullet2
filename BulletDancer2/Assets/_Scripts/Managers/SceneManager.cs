using System;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour {
    public event Action OnInitializationCompleted;
    
    protected SceneManagerData data;
    protected bool isInitializing = false;
    protected SceneManagerType type;
    private ManagerInitializationState initializationState = ManagerInitializationState.NOT_INITIALIZED;

    public SceneManagerType Type => type;
    public ManagerInitializationState InitializationState => initializationState;
    public bool IsInitialized => InitializationState == ManagerInitializationState.COMPLETED;

    public virtual void Init(MasterSystem masterSystem, SceneManagerData data) {
        initializationState = ManagerInitializationState.IN_PROGRESS;
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