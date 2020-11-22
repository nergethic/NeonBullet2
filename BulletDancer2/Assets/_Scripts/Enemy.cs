using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] GameObject bulletPrefab;
    public int Health = 4;
    public int MaxHealth = 4;

    void Start()
    {
        InvokeRepeating("ShootBullet", 1, 1);   
    }

    void ShootBullet() {
        var bullet = Instantiate(bulletPrefab).GetComponent<Projectile>();
        bullet.Initialize(Vector3.right, false, Projectile.ProjectileType.Energy);
        bullet.transform.position = transform.position;
    }

    void Update()
    {
        
    }
}
