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
            prefab.transform.SetParent(gameObject.transform, false);
            slots.Add(prefab.GetComponent<ItemSlot>());
        }
        return slots;
    }

}
