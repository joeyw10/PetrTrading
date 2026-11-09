using UnityEngine;
using System.Collections.Generic;

public class TradeBot : MonoBehaviour
{
    [Header("Sticker Display")]
    public TradeBotStickerDisplay stickerDisplay;

    [Header("Possible Stickers")]
    public List<GameObject> possibleStickers = new List<GameObject>();

    public GameObject currentSticker;

    public void OpenTradeRoom()
    {
        if (possibleStickers.Count == 0)
        {
            Debug.LogWarning("No stickers assigned!");
            return;
        }

        int randomIndex = Random.Range(0, possibleStickers.Count);

        currentSticker = possibleStickers[randomIndex];

        stickerDisplay.DisplaySticker(currentSticker);

        Debug.Log("Bot wants to trade: " + currentSticker.name);
    }
}