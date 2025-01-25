using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public GameObject messageCanvas;
    public TMP_Text text;
    public GameObject playerCamera; // Reference to the object controlling the camera
    private readonly Queue<string> messageQueue = new();

    public void AddMessage(string message)
    {
        if (messageQueue.Count == 0)
        {
            text.text = message;
            messageCanvas.SetActive(true);

            Time.timeScale = 0f; // Freeze game time
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible

            // Disable camera movement (if your camera has a script)
            if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
            {
                cameraController.enabled = false;
            }
        }
        messageQueue.Enqueue(message);
    }

    public void RemoveMessage()
    {
        messageQueue.Dequeue();
        messageCanvas.SetActive(false);
        if (messageQueue.Count == 0)
        {
            Time.timeScale = 1f; // Resume game time
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the game view
            Cursor.visible = false; // Hide the cursor

            // Enable camera movement (if your camera has a script)
            if (playerCamera.TryGetComponent(out MonoBehaviour cameraController))
            {
                cameraController.enabled = true;
            }
        } else
        {
            text.text = messageQueue.Peek();
            messageCanvas.SetActive(true);
        }
    }
}
