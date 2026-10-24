using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    // SET TO PUBLIC SO WE CAN SEE IT IN PROPERTIES, BUT DO NOT ADD STICKERS MANUALLY UNLESS IT
    // IS FOR THE PROTOTYPE
    public List<StickerInfo> myStickers;

    public void addSticker(StickerInfo sticker) {
        myStickers.Add(sticker);
    }

    // edit needed probably
    public void removeSticker(StickerInfo sticker) {
        myStickers.Remove(sticker);
    }

}
