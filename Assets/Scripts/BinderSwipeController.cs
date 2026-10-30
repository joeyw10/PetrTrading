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

    [Header("Favorited Button (optional look)")]
    [Tooltip("Drag the Favorited button's Image here to tint it while the filter is on.")]
    [SerializeField] private Image favoritedButtonImage;
    [SerializeField] private Color favoritedOnColor = new Color(1f, 0.85f, 0.4f);
    [SerializeField] private Color favoritedOffColor = Color.white;

    private Image _binderImage;
    private int _currentPageIndex = 0;
    private Vector2 _pointerStartPosition;
    private bool _favoritesOnly = false;

    private void Start()
    {
        _binderImage = GetComponent<Image>();
        UpdatePageVisuals();
        UpdateFavoritedButtonLook();
    }

    // ---------- Swiping ----------
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

        if (Mathf.Abs(dragDelta.x) >= swipeThreshold)
        {
            if (dragDelta.x > 0) OnSwipeRight(); // previous page
            else OnSwipeLeft();                  // next page
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

        // 2. Turn on ONLY the active page
        if (stickerPages != null)
        {
            for (int i = 0; i < stickerPages.Length; i++)
            {
                if (stickerPages[i] != null)
                    stickerPages[i].SetActive(i == _currentPageIndex);
            }
        }
    }

    // ---------- Filtering ----------
    // Hook this to the Rarity Dropdown's "On Value Changed" event (same as before).
    public void OnDropdownValueChanged()
    {
        ApplyFilters();
    }

    // Hook this to the Favorited button's OnClick event. Click once = on, click again = off.
    public void OnFavoritedClicked()
    {
        _favoritesOnly = !_favoritesOnly;
        UpdateFavoritedButtonLook();
        ApplyFilters();
    }

    private void UpdateFavoritedButtonLook()
    {
        if (favoritedButtonImage != null)
            favoritedButtonImage.color = _favoritesOnly ? favoritedOnColor : favoritedOffColor;
    }

    // One place that combines BOTH filters (rarity + favorites)
    private void ApplyFilters()
    {
        if (rarityDropdown == null || page1Container == null)
        {
            Debug.LogError("Please make sure both Rarity Dropdown and Page 1 Container are assigned in the Inspector!");
            return;
        }

        string selectedOption = rarityDropdown.options[rarityDropdown.value].text.ToLower().Trim();
        bool rarityFilterOn = !(selectedOption == "all" || selectedOption == "all rarities");
        bool anyFilterOn = rarityFilterOn || _favoritesOnly;

        RaritySort[] allStickers = transform.parent.GetComponentsInChildren<RaritySort>(true);

        foreach (RaritySort sticker in allStickers)
        {
            if (sticker == null) continue;

            if (!anyFilterOn)
            {
                // No filters: send every sticker back to its native page
                sticker.transform.SetParent(sticker.originalParent);
                sticker.transform.SetSiblingIndex(sticker.originalSiblingIndex);
                sticker.gameObject.SetActive(true);
                continue;
            }

            bool matchesRarity = !rarityFilterOn ||
                                 sticker.stickerRarity.ToString().ToLower() == selectedOption;
            bool matchesFavorite = !_favoritesOnly || sticker.IsFavorited;

            if (matchesRarity && matchesFavorite)
            {
                // Matching stickers move into Page 1's grid, so the binder look stays the same
                sticker.transform.SetParent(page1Container);
                sticker.gameObject.SetActive(true);
            }
            else
            {
                sticker.gameObject.SetActive(false);
            }
        }

        // Matches now all live on Page 1, so jump back to it
        if (anyFilterOn)
        {
            _currentPageIndex = 0;
            UpdatePageVisuals();
        }

        // Rebuild the grid layouts immediately
        foreach (GameObject page in stickerPages)
        {
            if (page != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(page.GetComponent<RectTransform>());
        }
        Canvas.ForceUpdateCanvases();
    }
}