using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void LoadMainGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void LoadTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        return;
    }
}
