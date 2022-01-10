using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugDrawBoxCollider : MonoBehaviour {
    BoxCollider2D boxCollider;

    private void OnDrawGizmos() {
        TryGetBoxCollider();
        if (boxCollider == null)
            return;
        
        var center = new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0.0f);
        var size = new Vector3(transform.localScale.x*boxCollider.size.x, transform.localScale.y*boxCollider.size.y, 0.01f);
        Gizmos.DrawWireCube(center, size);
    }

    void TryGetBoxCollider() {
        if (boxCollider != null)
            return;
        
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
