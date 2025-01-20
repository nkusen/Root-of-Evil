using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu Canvas
    public GameObject playerCamera; // Reference to the object controlling the camera
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Resume game time
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the game view
        Cursor.visible = false; // Hide the cursor

        // Enable camera movement (if your camera has a script)
        if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
        {
            cameraController.enabled = true;
        }

        pauseMenuUI.SetActive(false); // Hide the pause menu
        isPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f; // Freeze game time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible

        // Disable camera movement (if your camera has a script)
        if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
        {
            cameraController.enabled = false;
        }

        pauseMenuUI.SetActive(true); // Show the pause menu
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Ensure time is running
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure time is running
        SceneManager.LoadScene("StartMenuScene"); // Go to main menu
    }
}
