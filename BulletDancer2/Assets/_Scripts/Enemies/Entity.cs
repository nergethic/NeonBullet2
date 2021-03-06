using System;
using UnityEngine;

public class Entity : MonoBehaviour {
    [SerializeField] Sprite sprite;
    [SerializeField] SpriteRenderer renderer;

    protected ProjectileManager projectileManager;
    protected Player player;

    public int Health = 4;
    public int MaxHealth = 4;
    public bool isDead;

    public event Action DeathEvent;
    public event Action HitEvent;
    public event Action AttackEvent;

    public void PlayDeathEvent() => DeathEvent?.Invoke();
    public void PlayHitEvent() => HitEvent?.Invoke();
    public void PlayAttackEvent() => AttackEvent?.Invoke();
    
    public virtual void Initialize(Player player, ProjectileManager projectileManager) {
        SetSprite(sprite);
        this.player = player;
        this.projectileManager = projectileManager;
    }
    
    public void OnDestroy() {
        StopAllCoroutines();
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
