using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Food : Item
{
    [SerializeField] FoodType type;
	private int HealthRegen => type switch
	{
		FoodType.apple => 1,
		FoodType.banana => 2,
		FoodType.turkey => 3,
        FoodType.medkit => 4,
		_ => 0
	};

    public override void Use()
    {
        base.Use();
        if (Owner.Health < Owner.MaxHealth)
        {
            Owner.Health += HealthRegen;
            if (Owner.Health > Owner.MaxHealth)
            {
                Owner.Health = Owner.MaxHealth;
            }
        }

        ItemSlot.RemoveItemFromSlot();
        Destroy(gameObject, 0.2f);
    }
}
