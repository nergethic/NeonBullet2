using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayer : MonoBehaviour
{
    [SerializeField] GameObject inventorySlotPrefab;
    

    public List<ItemSlot> PrepareItemSlots(int numberOfSlots)
    {
        var slots = new List<ItemSlot>();
        for (int i = 0; i < numberOfSlots; i++)
        {
            var prefab = Instantiate(inventorySlotPrefab);
            prefab.transform.parent = gameObject.transform;
            var a = prefab.GetComponent<ItemSlot>();
            slots.Add(prefab.GetComponent<ItemSlot>());
        }
        return slots;
    }
    public void DisplayItems(List<Item> items)
    {
        foreach (var item in items)
        {
             
        }
    }
}
