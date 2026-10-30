using System.Collections.Generic;
using System.Linq;
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

    [Tooltip("How many stickers fit on one page (a 2x2 grid = 4).")]
    [SerializeField] private int stickersPerPage = 4;

    [Header("Filter Settings")]
    [Tooltip("Drag your Rarity Dropdown GameObject here.")]
    [SerializeField] private TMP_Dropdown rarityDropdown;

    [Tooltip("Drag your Search Bar's input field here (the one you type into).")]
    [SerializeField] private TMP_InputField searchInput;

    [Header("Button Looks (optional)")]
    [Tooltip("Drag the Favorited / Newest button Images here to tint them while they are on.")]
    [SerializeField] private Image favoritedButtonImage;
    [SerializeField] private Image newestButtonImage;
    [SerializeField] private Color buttonOnColor = new Color(1f, 0.85f, 0.4f);
    [SerializeField] private Color buttonOffColor = Color.white;

    private Image _binderImage;
    private int _currentPageIndex = 0;
    private Vector2 _pointerStartPosition;
    private bool _favoritesOnly = false;
    private bool _newestFirst = false;
    private string _searchText = "";

    private void Start()
    {
        _binderImage = GetComponent<Image>();
        UpdatePageVisuals();
        UpdateButtonLooks();

        // Re-filter every time the player types a letter (no manual OnValueChanged hookup needed)
        if (searchInput != null)
            searchInput.onValueChanged.AddListener(OnSearchChanged);
    }

    private void OnSearchChanged(string text)
    {
        _searchText = text == null ? "" : text.Trim().ToLower();
        ApplyFilters();
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

    // ---------- Button hooks ----------
    // Rarity Dropdown -> On Value Changed
    public void OnDropdownValueChanged()
    {
        ApplyFilters();
    }

    // Favorited button -> OnClick (click once = on, again = off)
    public void OnFavoritedClicked()
    {
        _favoritesOnly = !_favoritesOnly;
        UpdateButtonLooks();
        ApplyFilters();
    }

    // Newest button -> OnClick (click once = newest first, again = back to normal order)
    public void OnNewestClicked()
    {
        _newestFirst = !_newestFirst;
        UpdateButtonLooks();
        ApplyFilters();
    }

    private void UpdateButtonLooks()
    {
        if (favoritedButtonImage != null)
            favoritedButtonImage.color = _favoritesOnly ? buttonOnColor : buttonOffColor;
        if (newestButtonImage != null)
            newestButtonImage.color = _newestFirst ? buttonOnColor : buttonOffColor;
    }

    // ---------- Filtering + sorting ----------
    // Position of a sticker in the book's ORIGINAL order (page by page)
    private int NativeOrder(RaritySort sticker)
    {
        int pageIndex = 0;
        if (sticker.originalParent != null)
        {
            for (int i = 0; i < stickerPages.Length; i++)
            {
                if (stickerPages[i] != null && stickerPages[i].transform == sticker.originalParent)
                {
                    pageIndex = i;
                    break;
                }
            }
        }
        return pageIndex * 1000 + sticker.originalSiblingIndex;
    }

    // One place that combines rarity filter + favorites filter + newest sort
    private void ApplyFilters()
    {
        if (rarityDropdown == null || stickerPages == null || stickerPages.Length == 0)
        {
            Debug.LogError("Please make sure the Rarity Dropdown and Sticker Pages are assigned in the Inspector!");
            return;
        }

        string selectedOption = rarityDropdown.options[rarityDropdown.value].text.ToLower().Trim();
        bool rarityFilterOn = !(selectedOption == "all" || selectedOption == "all rarities");
        bool searchOn = _searchText.Length > 0;
        bool anyOn = rarityFilterOn || _favoritesOnly || _newestFirst || searchOn;

        RaritySort[] allStickers = transform.parent.GetComponentsInChildren<RaritySort>(true);

        if (!anyOn)
        {
            // Nothing selected: send every sticker back to its native page and spot
            foreach (RaritySort sticker in allStickers)
            {
                if (sticker == null) continue;
                sticker.transform.SetParent(sticker.originalParent, false);
                sticker.transform.SetSiblingIndex(sticker.originalSiblingIndex);
                sticker.gameObject.SetActive(true);
            }
        }
        else
        {
            // 1. Keep only the stickers that pass the filters; hide the rest
            List<RaritySort> matches = new List<RaritySort>();
            foreach (RaritySort sticker in allStickers)
            {
                if (sticker == null) continue;

                bool matchesRarity = !rarityFilterOn ||
                                     sticker.stickerRarity.ToString().ToLower() == selectedOption;
                bool matchesFavorite = !_favoritesOnly || sticker.IsFavorited;
                bool matchesSearch = !searchOn || (sticker.stickerData != null &&
                                     sticker.stickerData.stickerName != null &&
                                     sticker.stickerData.stickerName.ToLower().Contains(_searchText));

                if (matchesRarity && matchesFavorite && matchesSearch) matches.Add(sticker);
                else sticker.gameObject.SetActive(false);
            }

            // 2. Order them: newest first if the Newest button is on, otherwise the book's normal order
            IEnumerable<RaritySort> ordered;
            if (_newestFirst)
            {
                // To flip to oldest first, change OrderByDescending to OrderBy.
                ordered = matches.OrderByDescending(s => s.AddedTicks).ThenBy(s => NativeOrder(s));
            }
            else
            {
                ordered = matches.OrderBy(s => NativeOrder(s));
            }

            // 3. Deal them out into the page grids, stickersPerPage at a time
            int perPage = Mathf.Max(1, stickersPerPage);
            int count = 0;
            foreach (RaritySort sticker in ordered)
            {
                int pageIndex = count / perPage;

                if (pageIndex < stickerPages.Length && stickerPages[pageIndex] != null)
                {
                    sticker.transform.SetParent(stickerPages[pageIndex].transform, false);
                    sticker.transform.SetAsLastSibling(); // keeps the sorted order inside the grid
                    sticker.gameObject.SetActive(true);
                }
                else
                {
                    sticker.gameObject.SetActive(false); // more stickers than pages can hold
                }
                count++;
            }

            // Results start on page 1
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