using Assets._Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    public abstract void AddResource(PlayerResources playerResources);
}
