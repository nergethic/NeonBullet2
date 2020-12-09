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

        if (activeCraftingLabels.Any())
        {
            ClearSelectedCraftingLabels();
        }

        if (page > 0 && page * maxItemsOnPage < craftingLabels.Count)
        {
            var endPagePlace = startPagePlace + maxItemsOnPage;
            nextButton.interactable = true;
            nextButton.onClick.AddListener(() => Display(page++));
            SetActiveCraftingLabelsOnGivenPage(startPagePlace, endPagePlace);
        }
        else
        {
            var endPagePlace = craftingLabels.Count % maxItemsOnPage;
            SetActiveCraftingLabelsOnGivenPage(startPagePlace, endPagePlace);
            nextButton.interactable = false;
        }
    }

    private void SetActiveCraftingLabelsOnGivenPage(int startPagePlace, int endPagePlace)
    {
        for (int i = startPagePlace; i < endPagePlace; i++)
        {
            craftingLabels[i].gameObject.SetActive(true);
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
