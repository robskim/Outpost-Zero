using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenControls()
    {
        SceneManager.LoadScene("ControlsMenuScene");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenuScene");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
