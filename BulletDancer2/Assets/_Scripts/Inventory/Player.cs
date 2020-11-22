using _Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    public PlayerInventory Inventory => inventory;
    public int Health = 80;
    public int MaxHealth = 100;
    public int energy = 3;
    public int MaxEnergy = 3;

    public bool isImmuneToDamage = false;
    public bool isAbsorbingEnergy = false;

    private const float IMMUNITY_AFTER_BEING_HIT = 0.5f;

    public void PlayerHitByProjectileAction(ref ProjectileData projectileData) {
        Health -= projectileData.damage;
        StartCoroutine(ToggleDamageImmunity(IMMUNITY_AFTER_BEING_HIT));
        if (Health <= 0) {
            Debug.LogError("PLAYER IS DEAD");
            Destroy(gameObject);
        }
    }

    public void Dash() {
        
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

