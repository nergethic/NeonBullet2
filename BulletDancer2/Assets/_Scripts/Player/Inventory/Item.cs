using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite Sprite { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Player Owner { get; set; }
    public ItemSlot ItemSlot { get; set; }
    public abstract void Use();

    private void Start()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite = SpriteRenderer.sprite;
    }
}
