using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingOrderOverrider : MonoBehaviour {
    [SerializeField] MeshRenderer r;
    // Start is called before the first frame update
    void Awake() {
        if (r != null) {
            r.rendererPriority = 100;
            r.sortingLayerName = "Default";
            r.sortingOrder = 20;
        }
    }
}
