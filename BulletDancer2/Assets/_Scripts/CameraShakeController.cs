using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShakeController : MonoBehaviour {
    [SerializeField] float shakeLength = 0.15f;
    [SerializeField] float shakePower = 0.018f;
    float shakeTimeRamaining;
    float shakeFadeTime;

    void LateUpdate() {
        if (shakeTimeRamaining > 0f) {
            shakeTimeRamaining -= Time.deltaTime;
            float xDiff = Random.Range(-1f, 1f) * shakePower;
            float yDiff = Random.Range(-1f, 1f) * shakePower;
            transform.position += new Vector3(xDiff, yDiff, 0f);
        }
    }

    public void StartShake() {
        StartShake(shakeLength, shakePower);
    }

    public void StartShake(float length, float power) {
        if (Mathf.Approximately(length, 0f))
            length += 0.1f;
        
        shakeTimeRamaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
    }
}
