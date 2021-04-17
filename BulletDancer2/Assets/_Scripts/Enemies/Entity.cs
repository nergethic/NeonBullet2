using System;
using UnityEngine;

public class Entity : MonoBehaviour {
    [SerializeField] Sprite sprite;
    [SerializeField] SpriteRenderer renderer;

    protected ProjectileManager projectileManager;
    protected Player player;
    protected Transform playerTransform;

    public int Health = 4;
    public int MaxHealth = 4;
    public bool isDead;

    public event Action DeathEvent;
    public event Action HitEvent;
    public event Action AttackEvent;

    public void PlayDeathEvent() => DeathEvent();
    public void PlayHitEvent() => HitEvent();
    public void PlayAttackEvent() => AttackEvent();
    
    public virtual void Initialize(Player player, ProjectileManager projectileManager) {
        SetSprite(sprite);
        this.player = player;
        playerTransform = player.transform;
        this.projectileManager = projectileManager;
    }

    public virtual void Tick(float dt) {}

    void SetSprite(Sprite sprite) {
        if (renderer != null && sprite != null)
            renderer.sprite = sprite;
    }
}

public enum EnemyActionType {
    Wait = 0,
    Follow
}
