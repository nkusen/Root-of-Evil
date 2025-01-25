using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject svgObject;
    private SVGImage svgImage; // The Image component for the SVG child

    void Start()
    {
        // Find the SVG Image component in the button's child
        svgImage = svgObject.GetComponent<SVGImage>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (svgImage != null)
        {
            // Dim the alpha when hovering
            Color dimmedColor = svgImage.color;
            dimmedColor.a = 0.7f;
            svgImage.color = dimmedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (svgImage != null)
        {
            // Restore the original color
            Color originalColor = svgImage.color;
            originalColor.a = 1;
            svgImage.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (svgImage != null)
        {
            // Restore the original color
            Color originalColor = svgImage.color;
            originalColor.a = 1;
            svgImage.color = originalColor;
        }
    }
}
