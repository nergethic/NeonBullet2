using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SceneAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip defaultMusic;
    [SerializeField] AudioClip tankRoomMusic;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioMixerSnapshot fadeOutMusic;
    [SerializeField] AudioMixerSnapshot fadeInMusic;
    [SerializeField] AudioMixerSnapshot defaultSnapshot;
    [SerializeField] AudioMixerSnapshot fireSnapshot;

    private void Awake()
    {
        ChangeFireVolume(true);
    }
    public IEnumerator ChangeRoomMusic(bool isMusicSetDefault)
    {
        fadeOutMusic.TransitionTo(1f);
        yield return new WaitForSeconds(1f);
        if (isMusicSetDefault)
        {
            musicAudioSource.clip = tankRoomMusic;
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.clip = defaultMusic;
            musicAudioSource.Play();
        }
        fadeInMusic.TransitionTo(1f);
    }

    public void ChangeFireVolume(bool isFirePlaying)
    {
        if (isFirePlaying)
            defaultSnapshot.TransitionTo(0.4f);
        else
            fireSnapshot.TransitionTo(0.4f);
    }
}
