using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    [Tooltip("Assign the skybox material here.")]
    public Material skyboxMaterial;

    [Tooltip("Directional light representing the sun.")]
    public Light sunLight;

    [Tooltip("Name of the exposure property in the skybox shader (e.g., '_Exposure').")]
    public string exposureProperty = "_Exposure";

    [Tooltip("Maximum exposure value during the lightning flash.")]
    public float maxExposure = 2.0f;

    [Tooltip("Maximum intensity of the sunlight during the lightning flash.")]
    public float maxSunIntensity = 2.0f;

    [Tooltip("Duration of the lightning flash in seconds.")]
    public float flashDuration = 0.2f;

    [Tooltip("Duration for the exposure to return to normal.")]
    public float recoveryDuration = 0.5f;

    public bool isDebugLightningEnabled = false;

    private float originalExposure;
    private float originalSunIntensity;

    [Header("Lightning Bolt Settings")]
    [Tooltip("The starting point of the lightning.")]
    public Transform startPoint;

    [Tooltip("The ending point of the lightning.")]
    public Transform endPoint;

    [Tooltip("The number of segments in the lightning bolt.")]
    public int segments = 10;

    [Tooltip("The randomness of the lightning bolt's path.")]
    public float zigzagIntensity = 0.5f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1; // Set number of points in the LineRenderer
        lineRenderer.enabled = false; // Initially hidden

        // Store original exposure and sunlight intensity
        if (skyboxMaterial != null && skyboxMaterial.HasProperty(exposureProperty))
        {
            originalExposure = skyboxMaterial.GetFloat(exposureProperty);
        }
        else
        {
            Debug.LogError("Skybox material or exposure property not set correctly.");
        }

        if (sunLight != null)
        {
            originalSunIntensity = sunLight.intensity;
        }
        else
        {
            Debug.LogWarning("No sunlight assigned to the LightningEffect script.");
        }
    }

    public void SimulateLightning()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("StartPoint and EndPoint must be assigned.");
            return;
        }

        // Start the combined lightning effect
        StartCoroutine(LightningSequence());
    }

    private System.Collections.IEnumerator LightningSequence()
    {
        // Generate the lightning bolt
        GenerateLightningBolt();

        // Flash the skybox and sunlight
        FlashSkyboxAndSunlight();

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Hide the lightning bolt and restore skybox/sunlight
        lineRenderer.enabled = false;
        RestoreSkyboxAndSunlight();
    }

    private void GenerateLightningBolt()
    {
        // Generate the lightning bolt path
        Vector3[] lightningPoints = new Vector3[segments + 1];
        Vector3 direction = (endPoint.position - startPoint.position) / segments;

        lightningPoints[0] = startPoint.position; // Start of lightning
        for (int i = 1; i < segments; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-zigzagIntensity, zigzagIntensity),
                Random.Range(-zigzagIntensity, zigzagIntensity),
                Random.Range(-zigzagIntensity, zigzagIntensity)
            );

            lightningPoints[i] = startPoint.position + direction * i + offset;
        }
        lightningPoints[segments] = endPoint.position; // End of lightning

        // Apply the positions to the LineRenderer
        lineRenderer.SetPositions(lightningPoints);
        lineRenderer.enabled = true; // Show the lightning bolt
    }

    private void FlashSkyboxAndSunlight()
    {
        // Flash the skybox
        if (skyboxMaterial != null && skyboxMaterial.HasProperty(exposureProperty))
        {
            skyboxMaterial.SetFloat(exposureProperty, maxExposure);
        }

        // Flash the sunlight
        if (sunLight != null)
        {
            sunLight.intensity = maxSunIntensity;
        }
    }

    private void RestoreSkyboxAndSunlight()
    {
        // Gradually restore skybox exposure and sunlight intensity
        StartCoroutine(RestoreSkyboxAndSunlightCoroutine());
    }

    private System.Collections.IEnumerator RestoreSkyboxAndSunlightCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < recoveryDuration)
        {
            float t = elapsedTime / recoveryDuration;

            if (skyboxMaterial != null && skyboxMaterial.HasProperty(exposureProperty))
            {
                float exposure = Mathf.Lerp(maxExposure, originalExposure, t);
                skyboxMaterial.SetFloat(exposureProperty, exposure);
            }

            if (sunLight != null)
            {
                float intensity = Mathf.Lerp(maxSunIntensity, originalSunIntensity, t);
                sunLight.intensity = intensity;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are restored
        if (skyboxMaterial != null && skyboxMaterial.HasProperty(exposureProperty))
        {
            skyboxMaterial.SetFloat(exposureProperty, originalExposure);
        }

        if (sunLight != null)
        {
            sunLight.intensity = originalSunIntensity;
        }
    }

    // Update method to check for key press
    private void Update()
    {
        if (isDebugLightningEnabled && Input.GetKeyDown(KeyCode.V))
        {
            SimulateLightning();
        }
    }
}
