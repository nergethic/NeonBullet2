using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Transform myTransform;
    [SerializeField] float speed = 1.0f;
    private Vector2 dir;

    private int defaultLayerMask;
    private int playerLayerMask;
    private int projectileLayerMask;

    void Start() {
        defaultLayerMask = LayerMask.NameToLayer("Default");
        playerLayerMask = LayerMask.NameToLayer("Player");
        projectileLayerMask = LayerMask.NameToLayer("Projectile");
    }

    public void Initialize(Vector2 _dir) {
        dir = _dir;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.tag);
        if (other.gameObject.layer != playerLayerMask && other.gameObject.layer != projectileLayerMask && other.gameObject.layer != defaultLayerMask)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        var newPos = myTransform.position;
        newPos.x += Time.deltaTime * dir.x * speed;
        newPos.z += Time.deltaTime * dir.y * speed;
        
        myTransform.position = newPos;
    }
}
