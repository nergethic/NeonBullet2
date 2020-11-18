using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] FoodType type;
	public int HealthRegen => type switch
	{
		FoodType.apple => 10,
		FoodType.banana => 20,
		FoodType.turkey => 30,
		_ => 0
	};

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
