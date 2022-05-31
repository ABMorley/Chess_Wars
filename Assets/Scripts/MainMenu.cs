using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string PlayScene;

    public void Play()
    {
        SceneManager.LoadScene(PlayScene);
    }

    public void ShowOptions()
    {
        // TODO: Add options?
    }

    public void Quit()
    {
        Application.Quit();
    }
}
