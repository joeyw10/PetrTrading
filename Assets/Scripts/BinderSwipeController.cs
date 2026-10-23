using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;

    [Header("Binder Backgrounds")]
    [Tooltip("Element 0 = Binder_1stPG_0, Element 1 = Binder_2nd...")]
    public Sprite[] binderBackgroundSprites;
    
    [Header("Sticker Pages")]
    [Tooltip("Drag your Page1 and Page2 GameObjects here in order.")]
    public GameObject[] stickerPages;
    
    private Image _binderImage;
    private int _currentPageIndex = 0;
    private Vector2 _pointerStartPosition;

    private void Start()
    {
        _binderImage = GetComponent<Image>();
        UpdatePageVisuals();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _pointerStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) { /* Required interface method */ }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - _pointerStartPosition;

        if (Mathf.Abs(dragDelta.x) >= swipeThreshold)
        {
            if (dragDelta.x > 0) OnSwipeRight(); // Go to previous page
            else OnSwipeLeft();                 // Go to next page
        }
    }

    private void OnSwipeLeft()
    {
        // Check if we can move forward based on backgrounds or pages count
        int maxPages = Mathf.Min(binderBackgroundSprites.Length, stickerPages.Length);
        if (_currentPageIndex < maxPages - 1)
        {
            _currentPageIndex++;
            UpdatePageVisuals();
        }
    }

    private void OnSwipeRight()
    {
        if (_currentPageIndex > 0)
        {
            _currentPageIndex--;
            UpdatePageVisuals();
        }
    }

    private void UpdatePageVisuals()
    {
        // 1. Update the Binder Background Frame
        if (binderBackgroundSprites != null && _currentPageIndex < binderBackgroundSprites.Length && _binderImage != null)
        {
            _binderImage.sprite = binderBackgroundSprites[_currentPageIndex];
        }

        // 2. Loop through our page containers and turn on ONLY the active one
        if (stickerPages != null)
        {
            for (int i = 0; i < stickerPages.Length; i++)
            {
                if (stickerPages[i] != null)
                {
                    // If i matches current index, turn it ON (true). Otherwise, turn it OFF (false).
                    stickerPages[i].SetActive(i == _currentPageIndex);
                }
            }
        }
    }
}
