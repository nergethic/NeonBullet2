using Assets._Scripts.Inventory;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {
    public event Action BlockEvent;
    public event Action DeathEvent;
    public event Action HitEvent;
    public event Action SpawnEvent;
    
    [SerializeField] int health;
    [SerializeField] int energy;
    [SerializeField] PlayerInventory inventory;
    [SerializeField] PlayerStatusBar healthBar;
    [SerializeField] PlayerStatusBar energyBar;
    [SerializeField] int ore;
    [SerializeField] int iron;
    [SerializeField] int gold;
    [SerializeField] Transform shield;
    [SerializeField] Material shieldMaterial;
    [SerializeField] Material screenMaterial;
    [SerializeField] PlayerController controller;
    [SerializeField] ParticleSystem particle;
    
    public PlayerResources Resources;
    public PlayerInventory Inventory => inventory;
    public bool IsDead => health <= 0;
    
    Coroutine dashCor;
    Coroutine blockCor;

    readonly Color shieldDefaultColor = new Color(0.24f, 0.68f, 0.95f, 0f);
    readonly Color shieldHitColor = new Color(0.24f, 0.45f, 0.95f, 0f);
    readonly int ShieldColorID = Shader.PropertyToID("_Color");

    public float playerSpeed = 1.0f;
    public float dashSpeed = 8f;
    public int MaxHealth = 4;
    public int Health {
        get => health;
        set {
            health = value;
            healthBar.UpdateStatusBar(health);
        }
    }
    
    public int MaxEnergy = 3;
    public int Energy {
        get => energy;
        set {
            energy = value;
            energyBar.UpdateStatusBar(energy);
        }
    }

    public bool isImmuneToDamage;
    public bool isAbsorbingEnergy;
    public bool isDashing;
    bool isDead;

    const float IMMUNITY_AFTER_BEING_HIT = 1f;
    const float DASH_DURATION = 0.3f;
    const float SHIELD_DURATION_TIME = 0.45f;

    void Awake() {
        Resources.SetPlayerResources(ore, iron, gold);
    }
    
    void Start() {
        healthBar.UpdateStatusBar(health);
        energyBar.UpdateStatusBar(Energy);
        UpdateShieldColor(false);
        SpawnEvent?.Invoke();
    }

    public void PlayerHitByProjectileAction(ref ProjectileData projectileData) {
        if (isDead)
            return;
        
        Health -= projectileData.damage;
        HitEvent?.Invoke();
        StartCoroutine(ToggleDamageImmunity(IMMUNITY_AFTER_BEING_HIT));
        if (Health <= 0 && !isDead) {
            isDead = true;
            StartCoroutine(HandleDeath());
        }
    }

    public bool PerformDash() {
        if (dashCor != null)
            return false;

        int newEnergy = Energy - 1;
        if (newEnergy < 0)
            return false;
            
        Energy = newEnergy;
        dashCor = StartCoroutine(ToggleDash());
        return true;
    }

    public void PerformBlock() {
        if (blockCor == null) {
            blockCor = StartCoroutine(ToggleEnergyAbsorption(SHIELD_DURATION_TIME));
        }
    }

    public void HandleProjectile(Projectile projectile) {
        var projectileData = projectile.projectileData;
        if (isAbsorbingEnergy) {
            if (projectileData.typeMask == (int)ProjectileType.Energy) {
                BlockEvent?.Invoke();
                int newEnergy = Energy + 1;
                if (newEnergy <= MaxEnergy)
                    Energy = newEnergy;
                UpdateShieldColor(true);
            } else {
                int newEnergy = Energy - 1;
                if (newEnergy >= 0) {
                    Energy = newEnergy;
                    UpdateShieldColor(true);
                } else
                    PlayerHitByProjectileAction(ref projectileData);
            }
            
        } else if (!isImmuneToDamage)
            PlayerHitByProjectileAction(ref projectileData);
        
        Destroy(projectile.gameObject);
    }
    
    IEnumerator ToggleDash() {
        isDashing = true;
        yield return new WaitForSeconds(DASH_DURATION);
        isDashing = false;
        dashCor = null;

        yield return null;
    }

    IEnumerator ToggleDamageImmunity(float immunityTime) {
        isImmuneToDamage = true;
        yield return new WaitForSeconds(immunityTime);
        isImmuneToDamage = false;
        
        yield return null;
    }
    
    IEnumerator ToggleEnergyAbsorption(float absorbtionTime) {
        Assert.IsTrue(absorbtionTime > 0.1f);
        shield.gameObject.SetActive(true);
        shield.localScale = Vector3.zero;
        shield.DOScale(Vector3.one*0.35f, 0.1f);
        
        isAbsorbingEnergy = true;
        isImmuneToDamage = true;
        yield return new WaitForSeconds(absorbtionTime-0.07f);
        shield.DOScale(Vector3.zero, 0.07f);
        yield return new WaitForSeconds(0.07f);
        isImmuneToDamage = false;
        isAbsorbingEnergy = false;
        
        shield.gameObject.SetActive(false);
        UpdateShieldColor(false);
        blockCor = null;
        
        yield return null;
    }
    
    void UpdateShieldColor(bool wasHit) {
        if (wasHit)
            shieldMaterial.SetColor(ShieldColorID, shieldHitColor);
        else
            shieldMaterial.SetColor(ShieldColorID, shieldDefaultColor);
    }
    
    IEnumerator HandleDeath() {
        DeathEvent?.Invoke();
        controller.enabled = false;
        particle.startColor = Color.red;
        particle.Play();

        var masterSystem = FindObjectOfType<MasterSystem>(); // TODO: spawn player from master system
        if (masterSystem == null)
            yield break;

        masterSystem.ReloadLevel();
    }
}

