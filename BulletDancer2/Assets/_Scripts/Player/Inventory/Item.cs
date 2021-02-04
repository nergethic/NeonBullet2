using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite Sprite { get; set; }
    public SpriteRenderer SpriteRenderer;
    public Player Owner { get; set; }
    public PlayerController playerController;
    public ItemSlot ItemSlot { get; set; }
    public event Action UseEvent;

    public virtual void Use()
    {
        SpriteRenderer.sprite = null;
        UseEvent();
    }

    public void Initialize(Player player, PlayerController playerController) {
        this.Owner = player;
        this.playerController = playerController;
    }

    private void Awake() {
        Sprite = SpriteRenderer.sprite;
    }
}
