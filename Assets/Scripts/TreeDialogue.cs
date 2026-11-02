using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeDialogue : MonoBehaviour
{
    [Header("Drag your Sticker Prefabs here")]
    public List<GameObject> stickerPrefabs; // List of all tradeable sticker prefabs
    private int currentIndex = 0;

    [Header("UI Display")]
    public Image botRequestImage; // The image showing what the bot wants

    [Header("Player Inventory Slots")]
    public List<Transform> playerStickerSlots; // Drag your slot Transforms (or GameObjects) here!
    private int inventoryIndex = 0; // Tracks which slot to fill next

    void Start()
    {
        UpdateBotRequestDisplay();
    }

    // Call this when clicking the "No" button
    public void OnClickNo()
    {
        currentIndex++; // Move to the next prefab in the list

        if (currentIndex >= stickerPrefabs.Count)
        {
            currentIndex = 0; // Loop back to the start if it reaches the end
        }

        UpdateBotRequestDisplay();
    }

    // Call this when clicking the "Yes" button
    public void OnClickYes()
    {
        // Make sure we have stickers and haven't run out of inventory slots
        if (stickerPrefabs.Count > 0 && inventoryIndex < playerStickerSlots.Count)
        {
            // Get the current sticker prefab
            GameObject prefabToSpawn = stickerPrefabs[currentIndex];

            if (prefabToSpawn != null)
            {
                // Instantiate (spawn) the sticker prefab as a child of the current inventory slot
                GameObject spawnedSticker = Instantiate(prefabToSpawn, playerStickerSlots[inventoryIndex]);

                // Reset its position and scale so it fits nicely inside the slot
                RectTransform rectTransform = spawnedSticker.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.localScale = Vector3.one;
                }

                // Advance to the next slot for future trades
                inventoryIndex++;
            }
        }
        else if (inventoryIndex >= playerStickerSlots.Count)
        {
            Debug.Log("Inventory slots are full!");
        }
    }

    void UpdateBotRequestDisplay()
    {
        if (stickerPrefabs.Count > 0 && botRequestImage != null)
        {
            Sprite stickerSprite = GetSpriteFromPrefab(stickerPrefabs[currentIndex]);
            if (stickerSprite != null)
            {
                botRequestImage.sprite = stickerSprite;
            }
        }
    }

    // Helper function to grab the sprite for the bot's request window
    Sprite GetSpriteFromPrefab(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) return sr.sprite;

        Image img = prefab.GetComponentInChildren<Image>();
        if (img != null) return img.sprite;

        return null;
    }
}