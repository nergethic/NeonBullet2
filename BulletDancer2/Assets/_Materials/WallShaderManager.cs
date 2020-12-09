using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShaderManager : MonoBehaviour {
    [SerializeField] Material wallMat;
    [SerializeField] private Transform mainCamera;

    void Start() {
        UpdateMaterialParameter();
    }
    
    void Update() {
        UpdateMaterialParameter();
    }

    void UpdateMaterialParameter() {
        var pos = mainCamera.position; 
        wallMat.SetVector("_CameraWorldPos", new Vector4(pos.x, pos.y, pos.z, 0.0f));
    }
}
