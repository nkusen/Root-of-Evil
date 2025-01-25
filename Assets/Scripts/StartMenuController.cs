using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject startContainer;
    public GameObject aboutContainer;

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

    public void OpenAbout()
    {
        startContainer.SetActive(false);
        aboutContainer.SetActive(true);
    }

    public void CloseAbout()
    {
        startContainer.SetActive(true);
        aboutContainer.SetActive(false);
    }

}
