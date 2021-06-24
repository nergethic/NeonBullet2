using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemSlot> slots;
    [SerializeField] int numberOfSlots;
    [SerializeField] InventoryDisplayer inventoryPanel;
    [SerializeField] Player player;
    public bool IsInventoryActive => inventoryPanel.isActiveAndEnabled;


    private void Awake()
    {
        slots = inventoryPanel.PrepareItemSlots(numberOfSlots);
    }
    public void AddItem(Item item)
    {
        foreach (var slot in slots)
        {
            if (!slot.HasItem)
            {
                item.Owner = player;
                slot.Item = item;
                break;
            }
        }
    }
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.gameObject.SetActive(false);
    }


}
