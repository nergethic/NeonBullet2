using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRoom : MonoBehaviour
{
    public event Func<bool, IEnumerator> ChangeRoom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerAudio = collision.GetComponentInChildren<PlayerAudioController>();
            playerAudio.ChangeFootstepsToTile();
            StartCoroutine(ChangeRoom(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerAudio = collision.GetComponentInChildren<PlayerAudioController>();
            playerAudio.ChangeFootstepsToGravel();
            StartCoroutine(ChangeRoom(false));
        }
    }
}
