using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRoom : MonoBehaviour
{
    [SerializeField] SceneAudioManager sceneAudioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerAudio = collision.GetComponentInChildren<PlayerAudioController>();
            playerAudio.ChangeFootstepsToTile();
            StartCoroutine(sceneAudioManager.ChangeRoomMusic(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerAudio = collision.GetComponentInChildren<PlayerAudioController>();
            playerAudio.ChangeFootstepsToGravel();
            StartCoroutine(sceneAudioManager.ChangeRoomMusic(false));
        }
    }
}
