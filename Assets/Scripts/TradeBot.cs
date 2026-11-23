
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

    public List<StickerInfo> offeredStickers = new List<StickerInfo>();

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
        currentSticker = null;

        offeredStickers.Clear();
        stickerDisplay.ClearDisplay();

        int randomIndex = Random.Range(0, possibleStickers.Count);

        currentSticker = possibleStickers[randomIndex];

        if (currentSticker == null)
        {
            Debug.LogWarning("Selected sticker prefab is empty!");
            return;
        }

        if (database != null)
        {
            currentStickerInfo = database.GetByPrefab(currentSticker);
        }

        if (currentStickerInfo == null)
        {
            Debug.LogWarning(
                "Could not find StickerInfo for: " + currentSticker.name
            );
            return;
        }

        offeredStickers.Add(currentStickerInfo);

        stickerDisplay.DisplaySticker(currentSticker);

        Debug.Log("Bot wants to trade: " + currentStickerInfo.stickerName);
    }

    public bool AddRequestedSticker(GameObject requestedPrefab)
    {
        if (tradeCompleted)
        {
            Debug.LogWarning("This trade has already been completed!");
            return false;
        }

        if (offeredStickers.Count >= 3)
        {
            Debug.Log("The bot cannot offer more than 3 stickers!");
            return false;
        }

        if (requestedPrefab == null)
        {
            Debug.LogWarning("Requested sticker prefab is missing!");
            return false;
        }

        if (database == null)
        {
            Debug.LogWarning("Sticker Database is not assigned!");
            return false;
        }

        StickerInfo requestedInfo = database.GetByPrefab(requestedPrefab);

        if (requestedInfo == null)
        {
            Debug.LogWarning(
                "Could not find StickerInfo for: " + requestedPrefab.name
            );
            return false;
        }

        offeredStickers.Add(requestedInfo);

        if (stickerDisplay != null)
        {
            stickerDisplay.DisplayStickers(offeredStickers);
        }

        Debug.Log("Bot added requested sticker: " + requestedInfo.stickerName);

        return true;
    }
}
