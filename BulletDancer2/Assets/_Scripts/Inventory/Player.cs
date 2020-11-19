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

}

