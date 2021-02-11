using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TowersRoom : MonoBehaviour
{
    public event Action<bool> ChangeFireVolumeEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            ChangeFireVolumeEvent(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            ChangeFireVolumeEvent(true);
    }
}
