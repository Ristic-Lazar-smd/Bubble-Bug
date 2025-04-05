using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DestroyOnHover : MonoBehaviour, IPointerEnterHandler
{
    // Reference to the Image component (optional - you could also just destroy the whole GameObject)
    private Image image;

    private void Awake()
    {
        // Get the Image component
        image = GetComponent<Image>();

        // Make sure the GameObject has a Collider (required for mouse hover)
        if (GetComponent<Collider2D>() == null && GetComponent<Collider>() == null)
        {
            Debug.LogWarning("DestroyOnHover requires a Collider or Collider2D component to work properly!");
        }
    }

    // This method is called when the mouse cursor enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Destroy the Image component
        if (image != null)
        {
            Destroy(image);
        }

        // Alternatively, destroy the whole GameObject:
        // Destroy(gameObject);
    }
}