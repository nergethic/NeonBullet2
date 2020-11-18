using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemSlot> slots;
    [SerializeField] int numberOfSlots;
    public bool IsInventoryActive => inventoryPanel.isActiveAndEnabled;
    [SerializeField] InventoryDisplay inventoryPanel;

    private void Start()
    {
        slots = inventoryPanel.PrepareItemSlots(numberOfSlots);
    }
    public void AddItem(Item item)
    {
        foreach (var slot in slots)
        {
            if (!slot.HasItem)
            {
                slot.Item = item;
                break;
            }
        }
    }

    public void DeleteItem(Item item)
    {
        //items.Remove(item);
    }
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    public void DisplayItems()
    {
    }
}
