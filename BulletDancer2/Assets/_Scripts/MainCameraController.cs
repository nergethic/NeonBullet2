using UnityEngine;

public class MainCameraController : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] CameraShakeController shakeController;

    public Camera GetCamera() => mainCamera;
    public void Shake() => shakeController.StartShake();
}
