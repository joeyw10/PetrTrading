using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    // SET TO PUBLIC SO WE CAN SEE IT IN PROPERTIES, BUT DO NOT ADD STICKERS MANUALLY UNLESS IT
    // IS FOR THE PROTOTYPE
    public List<StickerInfo> myStickers;

    private int commonAnts;
    private int goldenAnts;
    private int diamondAnts;

    private int stickerPacks;

    public void addSticker(StickerInfo sticker) {
        myStickers.Add(sticker);
    }

    // edit needed probably
    public void removeSticker(StickerInfo sticker) {
        myStickers.Remove(sticker);
    }

    public int getCommonAnts() {
        return commonAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setCommonAnts(int antModifier) {
        commonAnts = commonAnts + antModifier;
    }

    public int getGoldenAnts() {
        return goldenAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setGoldenAnts(int antModifier) {
        goldenAnts = goldenAnts + antModifier;
    }

    public int getDiamondAnts() {
        return diamondAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setDiamondAnts(int antModifier) {
        diamondAnts = diamondAnts + antModifier;
    }

    public int getStickerPacks() {
        return stickerPacks;
    }

    // when using this, positive number adds while negative subtracts
    public void setStickerPacks(int packsModifier) {
        stickerPacks = stickerPacks + packsModifier;
    }

}
