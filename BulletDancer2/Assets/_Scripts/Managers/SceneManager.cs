using UnityEngine;

public abstract class SceneManager : MonoBehaviour {
    protected SceneManagerData data;
    protected ManagerInitializationState initializationState = ManagerInitializationState.NOT_INITIALIZED;
    protected bool isInitializing = false;
    protected SceneManagerType type;

    public SceneManagerType Type => type;

    public virtual void Init(MasterSystem masterSystem, SceneManagerData data) {
        initializationState = ManagerInitializationState.IN_PROGRESS;
    }
    
    public abstract void Tick(float dt);

    public ManagerInitializationState GetInitializationState() {
        return initializationState;
    }
}

public enum ManagerInitializationState {
    NOT_INITIALIZED = 0,
    IN_PROGRESS,
    COMPLETED,
    FAILED
}