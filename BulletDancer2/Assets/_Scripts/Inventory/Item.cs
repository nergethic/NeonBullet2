using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite sprite;
    public string Name { get; }

    public abstract void Use();
}
