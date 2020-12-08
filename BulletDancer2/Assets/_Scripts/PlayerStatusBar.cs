using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] Transform elementToDisplay;
    private List<Transform> elements = new List<Transform>();


    public void UpdateStatusBar(int change)
    {
        var healthUpdate = Mathf.Abs(change - elements.Count);

        if (change > elements.Count)
        {
            for (int i = 0; i < healthUpdate; i++)
            {
                AddNewElement();
            }
        }
        else
        {
            for (int i = 0; i < healthUpdate; i++)
            {
                RemoveElement();
            }
        }
    }
        private void AddNewElement()
    {
        var prefab = Instantiate(elementToDisplay);
        prefab.transform.SetParent(gameObject.transform, true);
        elements.Add(prefab);
    }

    private void RemoveElement()
    {
        if (elements.Any())
        {
            var elementToDestroy = elements[elements.Count - 1];
            Destroy(elementToDestroy.gameObject);
            elements.RemoveAt(elements.Count - 1);

        }
    }
}
