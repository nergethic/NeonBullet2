using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Item : MonoBehaviour {
    public Sprite Sprite { get; set; }
    public SpriteRenderer SpriteRenderer;
    public ShadowCaster2D ShadowCaster;
    public Player player { get; set; }
    public PlayerController playerController;
    public ItemSlot ItemSlot { get; set; }
    public event Action UseEvent;
    protected ProjectileManager projectileManager;

    public virtual void Use() {
        UseEvent();
    }

    public void Initialize(Player player, PlayerController playerController, ProjectileManager projectileManager) {
        this.player = player;
        this.playerController = playerController;
        this.projectileManager = projectileManager;
    }

    public void PutDownItemFromInventory()
    {
        transform.parent = null;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
        gameObject.transform.position = player.transform.position;
        SpriteRenderer.sprite = Sprite;
        ShadowCaster.enabled = true;
    }

    private void Awake() {
        Sprite = SpriteRenderer.sprite;
    }

}
