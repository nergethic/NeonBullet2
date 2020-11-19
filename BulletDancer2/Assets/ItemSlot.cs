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
        set => AddItemToSlot(value);
    }
    public bool HasItem => Item != null ? true : false;

    private void Start()
    {
        deleteButton.onClick.AddListener(RemoveItemFromSlot);
    }

    public void UseItem()
    {
        // item.Use(player);
        item = null;
        itemImage.sprite = null;
    }
    public void RemoveItemFromSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemButton.onClick.RemoveAllListeners();
        itemButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
    }

    private void AddItemToSlot(Item item)
    {
        itemButton.gameObject.SetActive(true);
        this.item = item;
        itemImage.sprite = item.sprite;
        deleteButton.gameObject.SetActive(true);
        itemButton.onClick.AddListener(OnUse);
    }

    private void OnUse()
    {
        item.Use();
        RemoveItemFromSlot();
    }
}
