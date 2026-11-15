
using UnityEngine;
using System.Collections.Generic;

public class TradeBot : MonoBehaviour
{
    [Header("Sticker Display")]
    public TradeBotStickerDisplay stickerDisplay;

    [Header("Possible Stickers")]
    public List<GameObject> possibleStickers = new List<GameObject>();

    [Header("Current Trade")]
    public GameObject currentSticker;
    public StickerInfo currentStickerInfo;

    public bool tradeCompleted = false;

    [Header("Sticker Database")]
    public StickerDatabase database;

    public void OpenTradeRoom()
    {
        if (possibleStickers == null || possibleStickers.Count == 0)
        {
            Debug.LogWarning("No stickers assigned to Possible Stickers!");
            return;
        }

        if (stickerDisplay == null)
        {
            Debug.LogWarning("Sticker Display is not assigned!");
            return;
        }

        tradeCompleted = false;
        currentStickerInfo = null;

        // Randomly select an actual sticker prefab
        int randomIndex = Random.Range(0, possibleStickers.Count);

        currentSticker = possibleStickers[randomIndex];

        if (currentSticker == null)
        {
            Debug.LogWarning("Selected sticker prefab is empty!");
            return;
        }

        // Display the actual sticker GameObject
        stickerDisplay.DisplaySticker(currentSticker);

        // Find the matching StickerInfo
        if (database != null)
{
    currentStickerInfo = database.GetByPrefab(currentSticker);
}

        if (currentStickerInfo == null)
        {
            Debug.LogWarning(
                "Could not find StickerInfo for: " + currentSticker.name
            );
        }
        else
        {
            Debug.Log("Bot wants to trade: " +
                      currentStickerInfo.stickerName);
        }
    }
}
