using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuGroup : MonoBehaviour
{
    private bool isPaused = false;
    private string pauseMenuSceneName = "PauseMenuScene";

    void Update()
    {
        // Check for the pause button (default: Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive); // Load the pause scene
        Time.timeScale = 0f; // Freeze the game
        AudioListener.pause = true; // Pause all audio
    }

    public void ResumeGame()
    {
        isPaused = false;
        SceneManager.UnloadSceneAsync(pauseMenuSceneName); // Unload the pause scene
        Time.timeScale = 1f; // Resume the game
        AudioListener.pause = false; // Resume audio
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale before quitting
        Application.Quit();

        // For the editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

