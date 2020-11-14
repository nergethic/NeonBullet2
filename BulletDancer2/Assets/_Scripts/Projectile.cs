using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] Transform myTransform;
    [SerializeField] float speed = 1.0f;
    private Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Vector2 _dir) {
        dir = _dir;
    }

    // Update is called once per frame
    void Update() {
        var newPos = myTransform.position;
        newPos.x += Time.deltaTime * dir.x * speed;
        newPos.z += Time.deltaTime * dir.y * speed;
        
        myTransform.position = newPos;
    }
}
