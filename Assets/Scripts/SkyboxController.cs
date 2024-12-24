using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [Tooltip("Assign the skybox material here.")]
    public Material skyboxMaterial;

    [Tooltip("The name of the tint color property in the shader (e.g., '_SkyTint').")]
    public string tintColorProperty = "_Tint";

    [Tooltip("Toggle to enable or disable random tint generation.")]
    public bool isRandomTintEnabled = false;

    [Tooltip("Initial tint color in hexadecimal format (#RRGGBBAA).")]
    public string initialTintHex = "#17F91480"; // Default to the provided color

    // Method to change the tint color of the skybox
    public void ChangeSkyboxTint(Color newTint)
    {
        if (skyboxMaterial != null && skyboxMaterial.HasProperty(tintColorProperty))
        {
            RenderSettings.skybox = skyboxMaterial;
            skyboxMaterial.SetColor(tintColorProperty, newTint);
            Debug.Log("Skybox tint color changed to: " + newTint);
        }
        else
        {
            Debug.LogError("Skybox material or tint color property not set correctly.");
        }
    }

    // Method to change the fog color
    public void ChangeFogColor(Color newFogColor)
    {
        RenderSettings.fogColor = newFogColor;
        Debug.Log("Fog color changed to: " + newFogColor);
    }

    // Method to generate and apply a random tint color
    public void GenerateRandomTint()
    {
        if (!isRandomTintEnabled) return;

        Color randomColor = new Color(Random.value, Random.value, Random.value, 1f); // Random RGB color with full alpha
        ChangeSkyboxTint(randomColor);
        ChangeFogColor(randomColor);
        Debug.Log("Generated random skybox tint: " + randomColor);
    }

    private void Start()
    {
        Color initialColor;
        if (ColorUtility.TryParseHtmlString(initialTintHex, out initialColor))
        {
            ChangeSkyboxTint(initialColor);
            ChangeFogColor(initialColor);
        }
        else
        {
            Debug.LogError("Invalid initial tint hex color: " + initialTintHex);
        }
    }

    // Update method to check for key press
    private void Update()
    {
        if (isRandomTintEnabled && Input.GetKeyDown(KeyCode.C))
        {
            GenerateRandomTint();
        }
    }
}

