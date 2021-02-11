using Assets._Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : Resource
{
    public override void AddResource(PlayerResources playerResources) => playerResources.Iron++;
}
