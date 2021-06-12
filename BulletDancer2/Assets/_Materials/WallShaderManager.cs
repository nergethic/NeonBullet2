using UnityEngine;

[ExecuteAlways]
public class WallShaderManager : MonoBehaviour {
    [SerializeField] Material wallMat;
    [SerializeField] Material distortionMat;
    [SerializeField] Transform mainCamera;
    [SerializeField] bool hideWalls;

    readonly int cameraWorldPosID = Shader.PropertyToID("_CameraWorldPos");

    void Start() {
        UpdateMaterialParameter();
    }
    
    void Update() {
        UpdateMaterialParameter();
    }

    void UpdateMaterialParameter() {
        var pos = mainCamera.position;
        var cameraWorldPos = new Vector4(pos.x, pos.y, pos.z, 0.0f);
        
        wallMat.SetVector(cameraWorldPosID, cameraWorldPos);
        distortionMat.SetVector(cameraWorldPosID, cameraWorldPos);
        
        if (Application.isPlaying) {
            wallMat.SetFloat("_Hide", hideWalls ? 1f : 0f);
        } else {
            wallMat.SetFloat("_Hide", 1f);
        }
    }
}
