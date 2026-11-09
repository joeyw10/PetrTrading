using UnityEngine;
using UnityEngine.UI;

public class TradeBotStickerDisplay : MonoBehaviour
{
    public Transform spawnLocation;
    private GameObject currentDisplayedSticker;

    public void DisplaySticker(GameObject stickerPrefab)
    {
        if (currentDisplayedSticker != null)
        {
            Destroy(currentDisplayedSticker);
        }

        // Instantiate the prefab inside the UI spawn box
        currentDisplayedSticker = Instantiate(stickerPrefab, spawnLocation);

        // Automatically convert SpriteRenderer to a UI Image at runtime
        SpriteRenderer spriteRenderer = currentDisplayedSticker.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite stickerSprite = spriteRenderer.sprite;
            Color stickerColor = spriteRenderer.color;

            // Remove the world-space SpriteRenderer so it doesn't conflict
            Destroy(spriteRenderer);

            // Add a UI Image component so the Canvas can render it
            Image uiImage = currentDisplayedSticker.AddComponent<Image>();
            uiImage.sprite = stickerSprite;
            uiImage.color = stickerColor;
            uiImage.preserveAspect = true;
        }

        // Ensure the RectTransform fits snugly inside the target UI box
        RectTransform rectTransform = currentDisplayedSticker.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}