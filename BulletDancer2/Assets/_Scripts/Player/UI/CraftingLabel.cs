using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CraftingLabel : MonoBehaviour
{
    [SerializeField] Button craftingButton;
    [SerializeField] Item itemToCraft;
    [SerializeField] Player player;
    [SerializeField] PlayerController playerController;
    [SerializeField] CraftingPanel craftingPanel;
    [SerializeField] int requiredOre;
    [SerializeField] int requiredIron;
    [SerializeField] int requiredGold;
    [SerializeField] Text oreText;
    [SerializeField] Text ironText;
    [SerializeField] Text goldText;
    [SerializeField] MasterSystem masterSystem;

    public event Action CraftEvent;

    void Awake() {
        craftingButton.image.sprite = itemToCraft.SpriteRenderer.sprite;
        craftingButton.interactable = false;
        SetupTextLabels();
        var projectileManager = masterSystem.TryGetManager(SceneManagerType.Projectile);
        if (projectileManager == null) {
            Debug.LogError("[CroftingLabel]: tried to get projectileManager but it wasn't initialized");
            return;
        }
        
        craftingButton.onClick.AddListener(() =>
        {
            CraftEvent();
            var instanceOfItem = Instantiate(itemToCraft);
            instanceOfItem.gameObject.SetActive(false);
            instanceOfItem.Initialize(player, playerController, projectileManager as ProjectileManager);
            player.Inventory.AddItem(instanceOfItem);
            player.Resources.UseResources(requiredOre, requiredIron, requiredGold);
            craftingPanel.UpdateCraftingButtonsAfterCraft();
        });
    }

    private void SetupTextLabels() {
        oreText.text = GetLabelText(requiredOre);
        ironText.text = GetLabelText(requiredIron);
        goldText.text = GetLabelText(requiredGold);
    }

    string GetLabelText(int resourceQuantity) {
        const string SYMBOL_OF_QUANTITY = "x ";
        StringBuilder sb = new StringBuilder();
        sb.Append(SYMBOL_OF_QUANTITY);
        sb.Append(resourceQuantity);
        return sb.ToString();
    }

    public void SetCraftingButton(bool state) => craftingButton.interactable = state;
    public bool CanPlayerCraftItem() => player.Resources.CanPlayerCraft(requiredOre, requiredIron, requiredGold);
}
