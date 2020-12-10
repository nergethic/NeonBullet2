using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLabel : MonoBehaviour
{
    [SerializeField] Button craftingButton;
    [SerializeField] Item itemToCraft;
    [SerializeField] Player player;
    [SerializeField] int requiredOre;
    [SerializeField] int requiredIron;
    [SerializeField] int requiredGold;

    void Awake()
    {
        craftingButton.image.sprite = itemToCraft.sprite;
        craftingButton.interactable = false;

        craftingButton.onClick.AddListener(() =>
        {
            player.Inventory.AddItem(itemToCraft);
            player.Resources.UseResources(requiredOre, requiredIron, requiredGold);

            if (!CanPlayerCraftItem())
            {
                craftingButton.interactable = false;
            }
        });
    }

    public void SetCraftingButtonActive() => craftingButton.interactable = true;
    public bool CanPlayerCraftItem() => player.Resources.CanPlayerCraft(requiredOre, requiredIron, requiredGold);
}
