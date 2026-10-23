using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIDynamicFilterManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown rarityDropdown;
    public Transform gridContainer; // The parent layout holding all stickers

    [Header("Grid Page Settings")]
    [Tooltip("How many stickers fit cleanly on a single page layout?")]
    public int stickersPerPage = 4;

    private List<StickerData> _allStickers = new List<StickerData>();
    private List<StickerData> _filteredStickers = new List<StickerData>();
    private int _currentPageIndex = 0;

    private void Start()
    {
        // 1. Gather every sticker asset under the grid container automatically
        foreach (Transform child in gridContainer)
        {
            StickerData data = child.GetComponent<StickerData>();
            if (data != null) _allStickers.Add(data);
        }

        if (rarityDropdown != null)
        {
            rarityDropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        // Initialize display
        UpdateFilterAndLayout();
    }

    private void OnDropdownChanged(int index)
    {
        // Reset back to page 1 whenever they toggle a new filter category
        _currentPageIndex = 0; 
        UpdateFilterAndLayout();
    }

    private void UpdateFilterAndLayout()
    {
        string selectedText = rarityDropdown.options[rarityDropdown.value].text.ToLower();
        _filteredStickers.Clear();

        // 2. Filter step: Find stickers matching the selected category
        foreach (var sticker in _allStickers)
        {
            if (selectedText.Contains("all") || sticker.stickerRarity.ToString().ToLower() == selectedText)
            {
                _filteredStickers.Add(sticker);
            }
            else
            {
                // Instantly turn off any sticker that doesn't match the active filter criteria
                sticker.gameObject.SetActive(false);
            }
        }

        // 3. Page Layout step: Pack the remaining valid items onto the active screen
        int startIndex = _currentPageIndex * stickersPerPage;
        int endIndex = startIndex + stickersPerPage;

        for (int i = 0; i < _filteredStickers.Count; i++)
        {
            // Only turn ON the sticker if its position falls inside the current page limits
            bool isInsideCurrentPageCanvas = (i >= startIndex && i < endIndex);
            _filteredStickers[i].gameObject.SetActive(isInsideCurrentPageCanvas);
        }
    }

    // Connect these methods to your Left/Right Swipe scripts or arrows!
    public void PageForward()
    {
        if ((_currentPageIndex + 1) * stickersPerPage < _filteredStickers.Count)
        {
            _currentPageIndex++;
            UpdateFilterAndLayout();
        }
    }

    public void PageBackward()
    {
        if (_currentPageIndex > 0)
        {
            _currentPageIndex--;
            UpdateFilterAndLayout();
        }
    }
}
