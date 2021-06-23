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
    public void RemoveItemFromSlot()
    {
        SetButtonStatus(Item, false);
        HandleEquipableItems();

        item = null;
        itemImage.sprite = null;
        itemButton.onClick.RemoveAllListeners();
        itemButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
    }

    private void HandleEquipableItems()
    {
        if (item is Weapon weapon)
            weapon.RemoveActiveWeapon();
        else if (item is Grenade grenade)
            grenade.RemoveActiveGrenade();
    }

    private void AddItemToSlot(Item item)
    {
        itemButton.gameObject.SetActive(true);
        item.ItemSlot = this;
        this.item = item;
        itemImage.sprite = item.Sprite;
        deleteButton.gameObject.SetActive(true);
        itemButton.onClick.AddListener(OnUse);
    }

    public void SetButtonStatus(Item item, bool isActive)
    {
        if (isActive)
        {
            ColorBlock colors = itemButton.colors;
            colors.normalColor = Color.green;
            colors.highlightedColor = Color.green;
            colors.selectedColor = Color.green;
            itemButton.colors = colors;
        }
        else
        {
            ColorBlock colors = itemButton.colors;
            colors.normalColor = Color.black;
            colors.highlightedColor = Color.white;
            colors.selectedColor = Color.black;
            itemButton.colors = colors;
        }
    }

    private void OnUse() => item.Use();
}
