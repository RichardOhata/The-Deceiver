using UnityEngine;
using UnityEngine.EventSystems;

public class TextSlider : MonoBehaviour, IDragHandler
{
    [Header("Settings")]
    public ABCUI gameScript;      // Drag your ABCUI object here
    public float sensitivity = 0.05f; // How fast the number changes
    public float minTime = 0.1f;  // Don't let time go below 0

    // This function runs automatically when you drag the mouse on this object
    public void OnDrag(PointerEventData eventData)
    {
        // eventData.delta.x tells us how much the mouse moved left/right
        float adjustment = eventData.delta.x * sensitivity;

        // Apply the change
        gameScript.timer += adjustment;

        // Clamp the values so it doesn't go negative or too high
        gameScript.timer = Mathf.Max(gameScript.timer, minTime);


    }
}
