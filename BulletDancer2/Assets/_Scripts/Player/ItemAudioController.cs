using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAudioController : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] AudioSource itemSource;
    [SerializeField] AudioClip use;

    private void Awake()
    {
        item.UseEvent += OnUse;
    }
    private void OnUse()
    {
        item.Sprite = null;
        item.gameObject.SetActive(true);
        itemSource.PlayOneShot(use);
    }
}
