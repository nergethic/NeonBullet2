using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneManager : MonoBehaviour {
    [SerializeField] SceneManagerType type;
    protected SceneManagerData data;
    protected ManagerInitializationState initializationState = ManagerInitializationState.NOT_INITIALIZED;
    protected bool isInitializing = false;

    public virtual void Init(MasterSystem masterSystem, SceneManagerData data) {
        initializationState = ManagerInitializationState.INITIALIZING;
    }
    
    public abstract void Tick();

    public ManagerInitializationState GetInitializationState() {
        return initializationState;
    }
}

public enum ManagerInitializationState {
    NOT_INITIALIZED = 0,
    INITIALIZING,
    INITIALIZED
}