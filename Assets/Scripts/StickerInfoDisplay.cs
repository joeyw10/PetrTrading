using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StickerInfoDisplay : MonoBehaviour
{
    [Header("UI Component Outlets")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private Image previewImage;
    [SerializeField] private Transform prefabSpawnLocation;

    [Header("Favorite Button")]
    [Tooltip("The heart/star button the player taps.")]
    [SerializeField] private Button favoriteButton;
    [Tooltip("The Image that changes look when favorited (can be the button's own Image).")]
    [SerializeField] private Image favoriteIcon;
    [Tooltip("Optional: swap sprites instead of tinting. Leave empty to just tint.")]
    [SerializeField] private Sprite favoritedSprite;
    [SerializeField] private Sprite notFavoritedSprite;
    [SerializeField] private Color favoritedColor = new Color(1f, 0.35f, 0.45f);
    [SerializeField] private Color notFavoritedColor = new Color(0.6f, 0.6f, 0.6f);

    private StickerInfo _data;

    private void Start()
    {
        // 1. Grab the transferred sticker data card from the bridge
        _data = StickerDataBridge.SelectedSticker;

        if (_data == null)
        {
            Debug.LogWarning("No sticker data was selected! Returning home.");
            ReturnToBinder();
            return;
        }

        // 2. Populate standard UI elements using your script's exact variables
        if (nameText != null) nameText.text = _data.stickerName;
        if (descriptionText != null) descriptionText.text = _data.description;
        if (rarityText != null) rarityText.text = "Rarity: " + _data.rarity;
        if (previewImage != null) previewImage.sprite = _data.stickerImage;

        // 3. Instantiate your stickerPrefab into the scene if it exists
        if (_data.stickerPrefab != null && prefabSpawnLocation != null)
        {
            Instantiate(_data.stickerPrefab, prefabSpawnLocation);
        }

        // 4. Hook up the favorite button and show the saved state
        if (favoriteButton != null)
            favoriteButton.onClick.AddListener(OnFavoriteClicked);

        UpdateFavoriteVisuals();
    }

    public void OnFavoriteClicked()
    {
        if (_data == null) return;

        StickerFavorites.Toggle(_data);
        UpdateFavoriteVisuals();
    }

    private void UpdateFavoriteVisuals()
    {
        if (favoriteIcon == null || _data == null) return;

        bool isFav = StickerFavorites.IsFavorited(_data);

        if (favoritedSprite != null && notFavoritedSprite != null)
            favoriteIcon.sprite = isFav ? favoritedSprite : notFavoritedSprite;

        favoriteIcon.color = isFav ? favoritedColor : notFavoritedColor;
    }

    public void ReturnToBinder()
    {
        StickerDataBridge.SelectedSticker = null;
        SceneManager.LoadScene("Inventory");
    }
}