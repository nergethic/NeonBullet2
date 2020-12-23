using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLabel : MonoBehaviour
{
    [SerializeField] Button craftingButton;
    [SerializeField] Item itemToCraft;
    [SerializeField] Player player;
    [SerializeField] CraftingPanel craftingPanel;
    [SerializeField] int requiredOre;
    [SerializeField] int requiredIron;
    [SerializeField] int requiredGold;
    [SerializeField] Text oreText;
    [SerializeField] Text ironText;
    [SerializeField] Text goldText;

    void Awake()
    {
        craftingButton.image.sprite = itemToCraft.SpriteRenderer.sprite;
        craftingButton.interactable = false;
        SetupTextLabels();
        craftingButton.onClick.AddListener(() =>
        {
            var instanceOfItem = Instantiate(itemToCraft);
            instanceOfItem.gameObject.SetActive(false);
            player.Inventory.AddItem(instanceOfItem);
            player.Resources.UseResources(requiredOre, requiredIron, requiredGold);
            craftingPanel.UpdateCraftingButtonsAfterCraft();
        });
    }

    private void SetupTextLabels()
    {
        var symbolOfQuantity = "x";
        oreText.text = symbolOfQuantity + " " + requiredOre.ToString();
        ironText.text = symbolOfQuantity + " " + requiredIron.ToString();
        goldText.text = symbolOfQuantity + " " + requiredGold.ToString();
    }

    public void SetCraftingButton(bool state) => craftingButton.interactable = state;
    public bool CanPlayerCraftItem() => player.Resources.CanPlayerCraft(requiredOre, requiredIron, requiredGold);
}
