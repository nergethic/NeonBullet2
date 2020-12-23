using _Config;
using Assets._Scripts.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Action DeathAction;
    public PlayerResources Resources;

    Coroutine dashCor;
    Coroutine blockCor;

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

    private const float IMMUNITY_AFTER_BEING_HIT = 0.5f;
    private const float DASH_DURATION = 0.2f;

    private void Awake() {
        Resources.SetPlayerResources(ore, iron, gold);
    }
    private void Start() {
        healthBar.UpdateStatusBar(health);
        energyBar.UpdateStatusBar(Energy);
    }

    public void PlayerHitByProjectileAction(ref ProjectileData projectileData) {
        Health -= projectileData.damage;
        StartCoroutine(ToggleDamageImmunity(IMMUNITY_AFTER_BEING_HIT));
        if (Health <= 0) {
            DeathAction();
            Debug.LogError("PLAYER IS DEAD");
            Destroy(gameObject, 1f);
        }
    }

    public void PerformDash() {
        if (dashCor == null)
            dashCor = StartCoroutine(ToggleDash());
    }

    public void PerformBlock() {
        if (blockCor == null)
            blockCor = StartCoroutine(ToggleEnergyAbsorbtion(0.3f));
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
        shield.gameObject.SetActive(true);
        isAbsorbingEnergy = true;
        isImmuneToDamage = true;
        yield return new WaitForSeconds(absorbtionTime);
        isImmuneToDamage = false;
        isAbsorbingEnergy = false;
        shield.gameObject.SetActive(false);
        blockCor = null;
        
        yield return null;
    }
}

