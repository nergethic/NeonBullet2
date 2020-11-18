using _Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInventory playerInventory;
    private int health = 100;
    public bool IsInventoryActive => playerInventory.IsInventoryActive;
    public void AddItemToPlayerInventory(Item item)
    {
        playerInventory.AddItem(item);
    }
    public void ShowPlayerInventory() => playerInventory.ShowInventory();

    public void HidePlayerInventory() => playerInventory.HideInventory();
}

