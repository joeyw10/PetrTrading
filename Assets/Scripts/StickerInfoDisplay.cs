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

    private void Start()
    {
        // 1. Grab the transferred sticker data card from the bridge
        StickerInfo data = StickerDataBridge.SelectedSticker;

        if (data == null)
        {
            Debug.LogWarning("No sticker data was selected! Returning home.");
            ReturnToBinder();
            return;
        }

        // 2. Populate standard UI elements using your script's exact variables
        if (nameText != null) nameText.text = data.stickerName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (rarityText != null) rarityText.text = "Rarity: " + data.rarity;
        if (previewImage != null) previewImage.sprite = data.stickerImage;

        // 3. Instantiate your stickerPrefab into the scene if it exists
        if (data.stickerPrefab != null && prefabSpawnLocation != null)
        {
            Instantiate(data.stickerPrefab, prefabSpawnLocation);
        }
    }

    public void ReturnToBinder()
    {
        StickerDataBridge.SelectedSticker = null;
        SceneManager.LoadScene("Inventory");
    }
}
