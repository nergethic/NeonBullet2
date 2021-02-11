using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectsAudioController : MonoBehaviour
{
    [SerializeField] AudioSource explosionSource;
    private void Awake()
    {
        explosionSource.Play();
    }
}
