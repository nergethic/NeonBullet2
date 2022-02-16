using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] Transform elementToDisplay;
    private List<Transform> elements = new List<Transform>();


    public void UpdateStatusBar(int newStatusValue)
    {
        var numberOfElements = elements.Count;

        if (newStatusValue > numberOfElements)
        {
            var numberOfNewElements = newStatusValue - numberOfElements;
            AddNewElements(numberOfNewElements);
        }
        else if (newStatusValue <= 0)
            RemoveAllElements();
        else
        {
            var elementsToRemove =  numberOfElements - newStatusValue;
            RemoveElements(elementsToRemove);
        }
    }

    private void AddNewElements(int numberOfNewElements)
    {
        for (int i = 0; i < numberOfNewElements; i++)
        {
            var prefab = Instantiate(elementToDisplay);
            prefab.transform.SetParent(gameObject.transform, true);
            elements.Add(prefab);
        }
    }

    private void RemoveElements(int elementsToDestroy)
    {
        if (elements.Any())
        {
            var numberOfElements = elements.Count;
            for (int i = numberOfElements; i > numberOfElements - elementsToDestroy; i--)
            {
                var elementToDestroy = elements[i - 1];
                Destroy(elementToDestroy.gameObject);
                elements.RemoveAt(elements.Count - 1);
            }
        }
    }

    private void RemoveAllElements()
    {
        if (elements.Any())
        {
            for (int i = elements.Count; i > 0; i--)
            {
                var elementToDestroy = elements[i - 1];
                Destroy(elementToDestroy.gameObject);
                elements.RemoveAt(elements.Count - 1);
            }
        }
    }

}
