using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthBarDisplayer : MonoBehaviour
{
    [SerializeField] Transform heart;
    private List<Transform> hearts = new List<Transform>();


    public void UpdateHealthStatus(int health)
    {
        var healthUpdate = Mathf.Abs(health - hearts.Count);

        if (health > hearts.Count)
        {
            for (int i = 0; i < healthUpdate; i++)
            {
                AddNewHeart();
            }
        }
        else
        {
            for (int i = 0; i < healthUpdate; i++)
            {
                RemoveHearth();
            }
        }
    }
        private void AddNewHeart()
    {
        var prefab = Instantiate(heart);
        prefab.transform.SetParent(gameObject.transform, true) ;
        hearts.Add(prefab);
    }

    private void RemoveHearth()
    {
        if (hearts.Any())
        {
            var heartToDestroy = hearts[hearts.Count - 1];
            Destroy(heartToDestroy.gameObject);
            hearts.RemoveAt(hearts.Count - 1);

        }
    }
}
