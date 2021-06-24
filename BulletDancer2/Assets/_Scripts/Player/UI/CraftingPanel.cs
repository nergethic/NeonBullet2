using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public List<CraftingLabel> craftingLabels;
    private List<CraftingLabel> activeCraftingLabels = new List<CraftingLabel>();
    [SerializeField] int maxItemsOnPage;
    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;
    public MasterSystem masterSystem;

    private int page = 1;
    void Awake()
    {
        Display();
    }

    public void Display()
    {
        var pageIndex = page - 1;
        var startPagePlace = pageIndex * maxItemsOnPage;
        var canDisplayItemOnNextPage = page * maxItemsOnPage < craftingLabels.Count;
        int endPagePlace;
        nextButton.onClick.RemoveAllListeners();

        if (activeCraftingLabels.Any())
        {
            ClearSelectedCraftingLabels();
        }

        SetPreviousButton(page);

        if (page > 0 && canDisplayItemOnNextPage)
        {
            endPagePlace = startPagePlace + maxItemsOnPage;
            nextButton.interactable = true;
            nextButton.onClick.AddListener(SetNextButton);

        }
        else if (page * maxItemsOnPage == craftingLabels.Count)
        {
            endPagePlace = startPagePlace + maxItemsOnPage;
            nextButton.interactable = false;
        }
        else
        {
            endPagePlace = craftingLabels.Count % maxItemsOnPage + startPagePlace;
            nextButton.interactable = false;
        }

        SetActiveCraftingLabelsOnGivenPage(startPagePlace, endPagePlace);
    }

    private void SetPreviousButton(int page)
    {
        if (page == 1)
        {
            previousButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
            previousButton.onClick.RemoveAllListeners();
            previousButton.onClick.AddListener(SetPreviousButton);
        }
    }

    private void SetPreviousButton()
    {
        page--;
        Display();
    }

    public void UpdateCraftingButtonsAfterCraft()
    {
        foreach (var activeCraftingLabel in activeCraftingLabels)
        {
            if (!activeCraftingLabel.CanPlayerCraftItem())
            {
                activeCraftingLabel.SetCraftingButton(false);
            }
        }
    }

    private void SetNextButton()
    {
        page++;
        Display();
    }

    private void SetActiveCraftingLabelsOnGivenPage(int startPagePlace, int endPagePlace)
    {
        for (int i = startPagePlace; i < endPagePlace; i++)
        {
            var craftingLabel = craftingLabels[i];
            craftingLabel.gameObject.SetActive(true);
            activeCraftingLabels.Add(craftingLabel);
            if (craftingLabel.CanPlayerCraftItem())
            {
                craftingLabel.SetCraftingButton(true);
            }
        }
    }

    private void ClearSelectedCraftingLabels()
    {
        foreach (var activeCraftingLabel in activeCraftingLabels)
        {
            activeCraftingLabel.SetCraftingButton(false);
            activeCraftingLabel.gameObject.SetActive(false);
        }
    }
}
