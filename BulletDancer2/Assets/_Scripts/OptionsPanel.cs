using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsPanel : UIPanel
{
    [SerializeField] GameObject MainPage;
    [SerializeField] GameObject AudioPage;
    [SerializeField] AudioMixer audioMixer;

    public void SwitchToAudioPage()
    {
        MainPage.SetActive(false);
        AudioPage.SetActive(true);
    }

    public void GetBackToMainPage()
    {
        AudioPage.SetActive(false);
        MainPage.SetActive(true);
    }
    public void StartNewGame() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    public void StartTutorial() => UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");

    public void ExitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            return;
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSpecialEffectsVolume(float sliderValue)
    {
        audioMixer.SetFloat("SpecialEffects", Mathf.Log10(sliderValue) * 20);
    }
}
