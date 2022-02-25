using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Food : Item
{
    [SerializeField] int healthToRestore;

    public override void Use()
    {
        base.Use();
        if (player.Health < player.MaxHealth)
        {
            if (player.Health + healthToRestore> player.MaxHealth)
                player.Health = player.MaxHealth;
            else
                player.Health += healthToRestore;
        }

        ItemSlot.RemoveItemFromSlot();
        Destroy(gameObject, 0.2f);
    }
}
