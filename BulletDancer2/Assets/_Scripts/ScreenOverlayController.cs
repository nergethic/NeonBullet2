using System.Collections;
using UnityEngine;

public class ScreenOverlayController : MonoBehaviour {
    [SerializeField] Material screenMat;
    readonly int DimValID = Shader.PropertyToID("_DimVal");
    
    void Awake() {
        SetDim();
    }

    public void SetDim() {
        screenMat.SetFloat(DimValID, 1f);
    }
    
    public IEnumerator FadeInScreen(float fadeOutDuration) {
        float normalizedTime = 0;
        while (normalizedTime <= 1f) {
            screenMat.SetFloat(DimValID,  Mathf.Clamp01(1f-normalizedTime));
            normalizedTime += Time.deltaTime / fadeOutDuration;
            yield return null;
        }
        screenMat.SetFloat(DimValID, 0f);
    }

    public IEnumerator FadeOutScreen(float fadeOutDuration) {
        float normalizedTime = 0;
        while (normalizedTime <= 1f) {
            screenMat.SetFloat(DimValID, normalizedTime);
            normalizedTime += Time.deltaTime / fadeOutDuration;
            yield return null;
        }
        screenMat.SetFloat(DimValID, 1f);
    }
}
