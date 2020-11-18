using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Button itemButton;
    [SerializeField] Button deleteButton;
    private Item item;
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            itemImage.sprite = value.sprite;
        }
    }
    public bool HasItem => Item != null ? true : false;


}
