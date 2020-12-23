using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Food : Item
{
    [SerializeField] FoodType type;
	private int HealthRegen => type switch
	{
		FoodType.apple => 10,
		FoodType.banana => 20,
		FoodType.turkey => 30,
		_ => 0
	};

    public override void Use()
    {
        if (Owner.Health < Owner.MaxHealth)
        {
            Owner.Health += HealthRegen;
            if (Owner.Health > Owner.MaxHealth)
            {
                Owner.Health = Owner.MaxHealth;
            }
        }

        ItemSlot.RemoveItemFromSlot();
        Destroy(gameObject);
    }
}
