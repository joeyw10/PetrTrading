using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    private List<StickerInfo> myStickers;

    public void addSticker(StickerInfo sticker) {
        myStickers.add(sticker);
    }

    // edit needed probably
    public void removeSticker(String sticker) {
        myStickers.remove(sticker);
    }

}
