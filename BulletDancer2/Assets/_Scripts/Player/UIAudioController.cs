using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    [SerializeField] AudioClip craft;
    [SerializeField] AudioSource craftSource;
    [SerializeField] List<CraftingLabel> craftingLabels;

    private void Awake()
    {
        foreach (var craftingLabel in craftingLabels)
        {
            craftingLabel.CraftEvent += OnCraft;
        }
    }

    private void OnCraft()
    {
        craftSource.PlayOneShot(craft);
    }
}
