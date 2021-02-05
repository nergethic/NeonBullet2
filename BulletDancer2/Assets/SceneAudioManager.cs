using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SceneAudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioClip defaultMusic;
    [SerializeField] AudioClip tankRoomMusic;
    [SerializeField] AudioSource musicAudioSource;

    public void ChangeMusicToDefault()
    {
        StartCoroutine(ChangeMusicAtTankRoom("Music", 1f, false));

    }

    public void ChangeMusicToTankRoomMusic()
    {
        StartCoroutine(ChangeMusicAtTankRoom("Music", 1f, true));
    }

    public  IEnumerator ChangeMusicAtTankRoom(string exposedParam, float duration, bool isMusicSetDefault)
    {

        float currentTime = 0;
        float currentVol;
        masterMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(0, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            masterMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

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
        currentTime = 0;
        targetValue = currentVol;
        masterMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            masterMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
