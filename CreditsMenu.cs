using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenuScene");
    }
}
