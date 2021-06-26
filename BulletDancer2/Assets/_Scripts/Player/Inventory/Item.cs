using System;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public Sprite Sprite { get; set; }
    public SpriteRenderer SpriteRenderer;
    public Player Owner { get; set; }
    public PlayerController playerController;
    public ItemSlot ItemSlot { get; set; }
    public event Action UseEvent;
    protected ProjectileManager projectileManager;

    public virtual void Use() {
        SpriteRenderer.sprite = null;
        UseEvent();
    }

    public void Initialize(Player player, PlayerController playerController, ProjectileManager projectileManager) {
        this.Owner = player;
        this.playerController = playerController;
        this.projectileManager = projectileManager;
    }

    private void Awake() {
        Sprite = SpriteRenderer.sprite;
    }
}
