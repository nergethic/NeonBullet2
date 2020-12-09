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
    public int Page {
        get => page;
        set
        {
            if (value > 0 && page * maxItemsOnPage < craftingLabels.Count)
            {
                Display(value);
                page = value;
            }
        }
    }
    void Start()
    {
        InstantiateCraftingLabels();
        Display(page);
    }

    public void Display(int page)
    {
        var pageIndex = page - 1;
        var startPagePlace = pageIndex * maxItemsOnPage;
        var endPagePlace = startPagePlace + maxItemsOnPage;

        if (activeCraftingLabels.Any())
        {
            ClearSelectedCraftingLabels();
        }

        SetActiveCraftingLabelsOnGivenPage(startPagePlace, endPagePlace);

        if (page > 0 && page * maxItemsOnPage < craftingLabels.Count)
        {
            nextButton.interactable = true;
            nextButton.onClick.AddListener(() => Display(page++));
        }
        else
        {
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

    private void InstantiateCraftingLabels()
    {
        foreach (var craftingLabel in craftingLabels)
        {
            var prefab = Instantiate(craftingLabel);
            prefab.transform.SetParent(gameObject.transform, true);
        }
    }
}
