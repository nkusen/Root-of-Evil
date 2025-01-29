using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate the time between frames
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Calculate FPS
        float fps = 1.0f / deltaTime;
        if(fps > 360){
            fps = 360;
        }
        // Display FPS on the screen
        int width = Screen.width, height = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 50;
        style.normal.textColor = Color.white;

        string text = $"FPS: {fps:0.}";
        GUI.Label(rect, text, style);
    }
}