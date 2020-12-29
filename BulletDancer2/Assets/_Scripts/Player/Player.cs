using Assets._Scripts.Inventory;
using System;
using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    public PlayerInventory Inventory => inventory;
    [SerializeField] PlayerStatusBar healthBar;
    [SerializeField] PlayerStatusBar energyBar;
    [SerializeField] int ore;
    [SerializeField] int iron;
    [SerializeField] int gold;
    [SerializeField] Transform shield;
    [SerializeField] Material shieldMaterial;
    [SerializeField] GameObject deathScreen;
    [SerializeField] PlayerController controller;
    [SerializeField] ParticleSystem particle;
    public event Action DeathEvent;
    public event Action HitEvent;
    public PlayerResources Resources;

    Coroutine dashCor;
    Coroutine blockCor;

    readonly Color shieldDefaultColor = new Color(0.24f, 0.68f, 0.95f, 0f);
    readonly Color shieldHitColor = new Color(0.24f, 0.45f, 0.95f, 0f);

    public float playerSpeed = 1.0f;
    public float dashSpeed = 8f;
    [SerializeField] int health;
    public int MaxHealth = 4;
    public int Health {
        get => health;
        set {
            health = value;
            healthBar.UpdateStatusBar(health);
        }
    }

    [SerializeField] int energy;
    public int MaxEnergy = 3;
    public int Energy {
        get => energy;
        set {
            energy = value;
            energyBar.UpdateStatusBar(energy);
        }
    }

    public bool isImmuneToDamage = false;
    public bool isAbsorbingEnergy = false;
    public bool isDashing = false;
    private bool isDead = false;

    private const float IMMUNITY_AFTER_BEING_HIT = 1f;
    private const float DASH_DURATION = 0.2f;
    private const float SHIELD_DURATION_TIME = 0.45f;

    private void Awake() {
        Resources.SetPlayerResources(ore, iron, gold);
    }
    private void Start() {
        healthBar.UpdateStatusBar(health);
        energyBar.UpdateStatusBar(Energy);
        UpdateShieldColor(false);
    }

    public void PlayerHitByProjectileAction(ref ProjectileData projectileData) {
        if (!isDead)
        {
            Health -= projectileData.damage;
            HitEvent();
            StartCoroutine(ToggleDamageImmunity(IMMUNITY_AFTER_BEING_HIT));
            if (Health <= 0 && !isDead)
            {
                isDead = true;
                Debug.LogError("PLAYER IS DEAD");
                StartCoroutine(HandleDeath());
            }
        }
    }

    public void PerformDash() {
        if (dashCor == null)
            dashCor = StartCoroutine(ToggleDash());
    }

    public void PerformBlock() {
        if (blockCor == null)
            blockCor = StartCoroutine(ToggleEnergyAbsorbtion(SHIELD_DURATION_TIME));
    }

    public void HandleProjectile(Projectile projectile) {
        var projectileData = projectile.projectileData;
        if (isAbsorbingEnergy && projectileData.typeMask == (int)ProjectileType.Energy) { // TODO
            int newEnergy = Energy + 1;
            if (newEnergy <= MaxEnergy)
                Energy = newEnergy;
            UpdateShieldColor(true);
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
    
    IEnumerator ToggleEnergyAbsorbtion(float absorbtionTime) {
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
            shieldMaterial.SetColor("_Color", shieldHitColor);
        else
            shieldMaterial.SetColor("_Color", shieldDefaultColor);
    }

    IEnumerator HandleDeath()
    {
        DeathEvent();
        deathScreen.SetActive(true);
        controller.enabled = false;
        particle.startColor = Color.red;
        particle.Play();
        yield return new WaitForSeconds(3f);
        deathScreen.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

