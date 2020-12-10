using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] List<CraftingLabel> craftingLabels;
    private List<CraftingLabel> activeCraftingLabels = new List<CraftingLabel>();
    [SerializeField] int maxItemsOnPage;
    [SerializeField] Button nextButton;
    private int page = 1;
    void Start()
    {
        Display(page);
    }

    public void Display(int page)
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

        if (page > 0 && canDisplayItemOnNextPage)
        {
            endPagePlace = startPagePlace + maxItemsOnPage;
            nextButton.interactable = true;
            nextButton.onClick.AddListener(() => Display(page + 1));

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

    private void SetActiveCraftingLabelsOnGivenPage(int startPagePlace, int endPagePlace)
    {
        for (int i = startPagePlace; i < endPagePlace; i++)
        {
            var craftingLabel = craftingLabels[i];
            craftingLabel.gameObject.SetActive(true);
            activeCraftingLabels.Add(craftingLabel);
            if (craftingLabel.CanPlayerCraftItem())
            {
                craftingLabel.SetCraftingButtonActive();
            }
        }
    }

    private void ClearSelectedCraftingLabels()
    {
        foreach (var activeCraftingLabel in activeCraftingLabels)
        {
            activeCraftingLabel.gameObject.SetActive(false);
        }
    }
}
