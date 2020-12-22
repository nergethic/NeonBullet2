using Assets._Scripts.Player.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item, ThrowableItem
{
    [SerializeField] PlayerController playerController;
    [SerializeField] float maxAirTime = 3f;
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject explosionEffect;
    private Vector2 dir;

    public void Throw(float speed, Vector2 itemDirection, Vector2 playerPosition)
    {
        myTransform.position = playerPosition;
        gameObject.SetActive(true);
        ItemSlot.RemoveItemFromSlot();
        playerController.ThrowableItem = null;
        StartCoroutine(Fly(speed, itemDirection));
    }

    public override void Use()
    {
        if (playerController.ThrowableItem is null)
        {
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        }
        else if (playerController.ThrowableItem == this)
        {
            playerController.ThrowableItem = null;
            SetButtonStatus(this, false);
        }
        else
        {
            playerController.ThrowableItem.SetButtonStatus(playerController.ThrowableItem, false);
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        }
    }

    public void SetButtonStatus(ThrowableItem throwableItem, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);


    IEnumerator Fly(float speed, Vector2 itemDirection)
    {
        dir = itemDirection;
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        var collider = gameObject.GetComponent<Collider2D>();
        collider.isTrigger = false;
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        for (float f = 0; f < maxAirTime; f += Time.deltaTime)
        {
            yield return null; 
        }
        SpriteRenderer.sprite = null;
        var flyingGrenade = Instantiate(explosionEffect, transform);
        
        Destroy(flyingGrenade, 0.5f);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
    }
}
