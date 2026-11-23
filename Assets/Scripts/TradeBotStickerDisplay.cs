
using UnityEngine;
using UnityEngine.UI;

public class TradeBotStickerDisplay : MonoBehaviour
{
    [Header("Bot Sticker Slots")]
    public Transform[] spawnLocations = new Transform[3];

    private GameObject[] displayedStickers = new GameObject[3];

    public void DisplaySticker(GameObject stickerPrefab)
    {
        // Keep compatibility with the original single-sticker method
        ClearDisplay();

        if (stickerPrefab == null)
        {
            Debug.LogWarning("Sticker prefab is missing!");
            return;
        }

        DisplayStickerInSlot(stickerPrefab, 0);
    }

    public void DisplayStickers(System.Collections.Generic.List<StickerInfo> stickers)
    {
        ClearDisplay();

        if (stickers == null)
            return;

        for (int i = 0; i < stickers.Count && i < 3; i++)
        {
            if (stickers[i] == null)
                continue;

            if (stickers[i].stickerPrefab == null)
            {
                Debug.LogWarning(
                    "Missing sticker prefab for " + stickers[i].stickerName
                );
                continue;
            }

            DisplayStickerInSlot(stickers[i].stickerPrefab, i);
        }
    }

    private void DisplayStickerInSlot(GameObject stickerPrefab, int slotIndex)
    {
        if (slotIndex >= spawnLocations.Length || spawnLocations[slotIndex] == null)
        {
            Debug.LogWarning("Bot sticker slot " + slotIndex + " is not assigned!");
            return;
        }

        GameObject newSticker = Instantiate(
            stickerPrefab,
            spawnLocations[slotIndex]
        );

        // Convert SpriteRenderer into UI Image if necessary
        SpriteRenderer spriteRenderer =
            newSticker.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Sprite stickerSprite = spriteRenderer.sprite;
            Color stickerColor = spriteRenderer.color;

            Destroy(spriteRenderer);

            Image uiImage = newSticker.GetComponent<Image>();

            if (uiImage == null)
                uiImage = newSticker.AddComponent<Image>();

            uiImage.sprite = stickerSprite;
            uiImage.color = stickerColor;
            uiImage.preserveAspect = true;
        }

        // Also support prefabs that already contain a child Image
        Image childImage = newSticker.GetComponentInChildren<Image>();

        if (childImage != null)
            childImage.preserveAspect = true;

        RectTransform rectTransform =
            newSticker.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
        }

        displayedStickers[slotIndex] = newSticker;
    }

    public void ClearDisplay()
    {
        for (int i = 0; i < displayedStickers.Length; i++)
        {
            if (displayedStickers[i] != null)
            {
                Destroy(displayedStickers[i]);
                displayedStickers[i] = null;
            }
        }
    }
}
