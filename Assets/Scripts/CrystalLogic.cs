using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableLogic : MonoBehaviour
{
    [Tooltip("How close the player needs to be to collect the crystal.")]
    public float collectDistance = 1.5f;

    [Tooltip("0 = fragment, 1 = green, 2 = red")]
    public int collectableId = 0;

    private Transform player;

    private void Start()
    {
        // Find the player in the scene
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Check if the player is within range
        if (Vector3.Distance(transform.position, player.position) <= collectDistance)
        {
            // Check if the player presses the E key
            if (Input.GetKeyDown(KeyCode.E))
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        // Notify the player script
        PlayerLogic playerScript = player.GetComponent<PlayerLogic>();
        if (playerScript != null)
        {
            playerScript.AddCollectable(collectableId);
        }

        // Destroy the crystal
        Destroy(gameObject);
    }
}
