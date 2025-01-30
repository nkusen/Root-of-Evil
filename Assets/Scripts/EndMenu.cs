using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public GameObject endMenuUI; // Reference to the Pause Menu Canvas
    public GameObject playerCamera; // Reference to the object controlling the camera

    public void Win()
    {
        endMenuUI.GetComponentInChildren<TMP_Text>().text = "You Won";
        endMenuUI.SetActive(true); // Hide the pause menu
        Time.timeScale = 0f; // Freeze game time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible

        // Disable camera movement (if your camera has a script)
        if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
        {
            cameraController.enabled = false;
        }
    }

    public void Lose()
    {
        endMenuUI.GetComponentInChildren<TMP_Text>().text = "You Died";
        endMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Freeze game time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible

        // Disable camera movement (if your camera has a script)
        if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
        {
            cameraController.enabled = false;
        }
    }
}
