using System.Collections;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [Tooltip("The first wall object.")]
    public GameObject wall1;

    [Tooltip("The second wall object.")]
    public GameObject wall2;

    [Tooltip("Speed at which the walls lower (units per second).")]
    public float loweringSpeed = 2.0f;

    [Tooltip("Depth below ground to lower the walls before destroying.")]
    public float lowerDepth = -5.0f;

    [Tooltip("Delay before the walls start lowering.")]
    public float delayBeforeLowering = 1.0f;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenStartingWalls()
    {
        // Ensure both walls are assigned
        if (wall1 != null && wall2 != null)
        {
            StartCoroutine(LowerAndDestroyWalls());
            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource is not assigned!");
            }
        }
        else
        {
            Debug.LogError("Walls are not assigned to the WallController script.");
        }
    }

    private IEnumerator LowerAndDestroyWalls()
    {
        // Wait for the delay before starting the movement
        while (wall1.transform.position.y > lowerDepth || wall2.transform.position.y > lowerDepth)
        {
            if (wall1.transform.position.y > lowerDepth)
            {
                wall1.transform.position = new Vector3(wall1.transform.position.x, wall1.transform.position.y - loweringSpeed * Time.deltaTime, wall1.transform.position.z);
            }

            if (wall2.transform.position.y > lowerDepth)
            {
                wall2.transform.position = new Vector3(wall2.transform.position.x, wall2.transform.position.y - loweringSpeed * Time.deltaTime, wall2.transform.position.z);
            }

            yield return null; // Wait for the next frame
        }
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned!");
        }
        // Destroy both walls
        Destroy(wall1);
        Destroy(wall2);
    }
}
