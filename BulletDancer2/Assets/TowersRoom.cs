using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TowersRoom : MonoBehaviour
{
    [SerializeField] SceneAudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            audioManager.ChangeFireVolume(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            audioManager.ChangeFireVolume(true);
    }
}
