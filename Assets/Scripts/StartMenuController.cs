using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        // This will only work in a built application, not in the Unity Editor
        Debug.Log("Game is exiting"); // Use this to verify in the Editor
    }

}
