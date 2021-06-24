using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    [SerializeField] AudioClip craft;
    [SerializeField] AudioSource craftSource;
    [SerializeField] CraftingPanel craftingPanel;
    private void Start()
    {
        foreach (var craftingLabel in craftingPanel.craftingLabels)
        {
            craftingLabel.CraftEvent += OnCraft;
        }
    }

    private void OnCraft()
    {
        craftSource.PlayOneShot(craft);
    }
}
