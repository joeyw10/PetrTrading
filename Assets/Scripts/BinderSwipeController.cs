using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BinderSwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;

    [Header("Binder Backgrounds")]
    [Tooltip("Element 0 = Binder_1stPG_0, Element 1 = Binder_2nd...")]
    public Sprite[] binderBackgroundSprites;
    
    [Header("Sticker Pages")]
    [Tooltip("Drag your Page1 and Page2 GameObjects here in order.")]
    public GameObject[] stickerPages;

    [Header("Filter Settings")]
    [Tooltip("Drag your Rarity Dropdown GameObject here.")]
    [SerializeField] private TMP_Dropdown rarityDropdown; 
    
    [Tooltip("Drag the 'Page 1' GameObject from your Hierarchy here.")]
    [SerializeField] private Transform page1Container;
    
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

    public void OnDrag(PointerEventData eventData) 
    { 
        // Intentionally left blank, but required so Unity catches drag data
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - _pointerStartPosition;

        // Check if the swipe met our minimum distance requirement
        if (Mathf.Abs(dragDelta.x) >= swipeThreshold)
        {
            if (dragDelta.x > 0) 
            {
                OnSwipeRight(); // Go to previous page
            }
            else 
            {
                OnSwipeLeft(); // Go to next page
            }
        }
    }

    private void OnSwipeLeft()
    {
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
                    // Turn true for active page index, false for all others
                    stickerPages[i].SetActive(i == _currentPageIndex);
                }
            }
        }
    }

        public void OnDropdownValueChanged()
    {
        if (rarityDropdown == null || page1Container == null)
        {
            Debug.LogError("Please make sure both Rarity Dropdown and Page 1 Container are assigned in the Inspector!");
            return;
        }

        string selectedOption = rarityDropdown.options[rarityDropdown.value].text.ToLower().Trim();

        RaritySort[] allStickers = transform.parent.GetComponentsInChildren<RaritySort>(true);

        foreach (RaritySort sticker in allStickers)
        {
            if (sticker == null) continue;
            
            string stickerRarityStr = sticker.stickerRarity.ToString().ToLower();

            // Resetting back to native pages
            if (selectedOption == "all" || selectedOption == "all rarities")
            {
                sticker.transform.SetParent(sticker.originalParent);
                sticker.transform.SetSiblingIndex(sticker.originalSiblingIndex);
                sticker.gameObject.SetActive(true); 
            }
            // Moving onto Page 1 panel layout
            else if (stickerRarityStr == selectedOption)
            {
                sticker.transform.SetParent(page1Container);
                sticker.gameObject.SetActive(true);
            }
            else
            {
                sticker.gameObject.SetActive(false);
            }
        }
        
        // We force all underlying Layout Groups on your pages to rebuild their element positions immediately
        foreach (GameObject page in stickerPages)
        {
            if (page != null)
            {
                // Finds layout system elements (like GridLayoutGroup) and forces a position update
                LayoutRebuilder.ForceRebuildLayoutImmediate(page.GetComponent<RectTransform>());
            }
        }
        
        // Double-ensure layout calculations finish processing this frame
        Canvas.ForceUpdateCanvases();
    }
}
