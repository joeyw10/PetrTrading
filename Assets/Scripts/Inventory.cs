using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    private List<StickerInfo> myStickers;

    public void addSticker(StickerInfo sticker) {
        myStickers.Add(sticker);
    }

    // edit needed probably
    public void removeSticker(StickerInfo sticker) {
        myStickers.Remove(sticker);
    }

}
