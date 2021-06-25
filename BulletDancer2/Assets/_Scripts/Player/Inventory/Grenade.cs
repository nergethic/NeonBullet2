using Assets._Scripts.Player.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item, ThrowableItem
{
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Rigidbody2D rb;
    private Projectile projectile;
    public void Throw(float speed, Vector2 itemDirection, Vector2 throwableSpawn)
    {
        /*myTransform.position = throwableSpawn;
        gameObject.SetActive(true);
        ItemSlot.RemoveItemFromSlot();
        playerController.ThrowableItem = null;
        StartCoroutine(Fly(speed, itemDirection));*/

        projectile = projectileManager.SpawnProjectile(playerController.transform.position, itemDirection, ProjectileType.Grenade, true, speed);
        if (projectile != null) {
            projectile.DestroyEvent += OnProjectileDestroy;
        }
    }

    private void OnProjectileDestroy()
    {
        ItemSlot.RemoveItemFromSlot();
        var explosion = Instantiate(explosionEffect);
        explosion.transform.position = projectile.transform.position;
        explosion.transform.localScale = new Vector3(1, 1);
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public override void Use()
    {
        if (playerController.ThrowableItem is null)
        {
            base.Use();
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        }
        else if (playerController.ThrowableItem == this)
        {
            RemoveActiveGrenade();
        }
        else
        {
            base.Use();
            playerController.ThrowableItem.SetButtonStatus(playerController.ThrowableItem, false);
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        }
    }

    public void RemoveActiveGrenade()
    {
        if (playerController.ThrowableItem == this)
        {
            playerController.ThrowableItem = null;
            SetButtonStatus(this, false);
        }
    }

    public void SetButtonStatus(ThrowableItem throwableItem, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);
}
