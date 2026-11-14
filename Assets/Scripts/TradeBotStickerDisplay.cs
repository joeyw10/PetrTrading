
using UnityEngine;
using UnityEngine.UI;

public class TradeBotStickerDisplay : MonoBehaviour
{
    public Transform spawnLocation;

    private GameObject currentDisplayedSticker;

    public void DisplaySticker(GameObject stickerPrefab)
    {
        if (stickerPrefab == null)
        {
            Debug.LogWarning("Sticker prefab is missing!");
            return;
        }

        // Remove the previous sticker
        if (currentDisplayedSticker != null)
        {
            Destroy(currentDisplayedSticker);
        }

        // Instantiate the new sticker
        currentDisplayedSticker = Instantiate(stickerPrefab, spawnLocation);

        // Convert SpriteRenderer into UI Image if necessary
        SpriteRenderer spriteRenderer =
            currentDisplayedSticker.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Sprite stickerSprite = spriteRenderer.sprite;
            Color stickerColor = spriteRenderer.color;

            Destroy(spriteRenderer);

            Image uiImage = currentDisplayedSticker.AddComponent<Image>();
            uiImage.sprite = stickerSprite;
            uiImage.color = stickerColor;
            uiImage.preserveAspect = true;
        }

        // Adjust its UI position and size
        RectTransform rectTransform =
            currentDisplayedSticker.GetComponent<RectTransform>();

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
