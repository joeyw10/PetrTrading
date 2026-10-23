using UnityEngine;
using UnityEngine.EventSystems;

public class UISwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;

    [Header("Linked Master Manager")]
    [Tooltip("Drag the GameObject that has the UIDynamicFilterManager script here.")]
    public UIDynamicFilterManager filterManager;
    
    private Vector2 _pointerStartPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Capture where the touch or mouse click started
        _pointerStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) 
    { 
        // Required for Unity's Event System to process drag sequences
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - _pointerStartPosition;

        // Check if the swipe met our minimum distance requirement
        if (Mathf.Abs(dragDelta.x) >= swipeThreshold)
        {
            if (dragDelta.x > 0)
            {
                // Swiping Right means flipping to the PREVIOUS page
                if (filterManager != null) filterManager.PageBackward();
            }
            else
            {
                // Swiping Left means flipping to the NEXT page
                if (filterManager != null) filterManager.PageForward();
            }
        }
    }
}
