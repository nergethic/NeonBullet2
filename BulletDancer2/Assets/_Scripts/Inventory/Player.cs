using _Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    public float playerSpeed = 1.0f;
    public float dashSpeed = 8f;
    public PlayerInventory Inventory => inventory;
    public int Health = 80;
    public int MaxHealth = 100;
    public int energy = 3;
    public int MaxEnergy = 3;

    public bool isImmuneToDamage = false;
    public bool isAbsorbingEnergy = false;
    public bool isDashing = false;

    private const float IMMUNITY_AFTER_BEING_HIT = 0.5f;
    private const float DASH_DURATION = 0.2f;

    public void PlayerHitByProjectileAction(ref ProjectileData projectileData) {
        Health -= projectileData.damage;
        StartCoroutine(ToggleDamageImmunity(IMMUNITY_AFTER_BEING_HIT));
        if (Health <= 0) {
            Debug.LogError("PLAYER IS DEAD");
            Destroy(gameObject);
        }
    }

    public void Dash() {
        StartCoroutine(ToggleDash());
    }
    
    IEnumerator ToggleDash() {
        isDashing = true;
        isImmuneToDamage = true;
        isAbsorbingEnergy = true;
        yield return new WaitForSeconds(DASH_DURATION);
        isDashing = false;
        isImmuneToDamage = false;
        isAbsorbingEnergy = false;
        
        yield return null;
    }

    IEnumerator ToggleDamageImmunity(float immunityTime) {
        isImmuneToDamage = true;
        yield return new WaitForSeconds(immunityTime);
        isImmuneToDamage = false;
        
        yield return null;
    }
    
    IEnumerator ToggleEnergyAbsorbtion(float absorbtionTime) {
        isAbsorbingEnergy = true;
        yield return new WaitForSeconds(absorbtionTime);
        isAbsorbingEnergy = false;
        
        yield return null;
    }
}

