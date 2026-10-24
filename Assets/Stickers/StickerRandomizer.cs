using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StickerRandomizer", menuName = "Scriptable Objects/StickerRandomizer")]
public class StickerRandomizer : ScriptableObject
{
    // Drag and drop your items into this list via the Unity Inspector
    public List<StickerInfo> possibleStickers;
    private System.Random _sysRandom = new System.Random();

    // Function to return a completely random item from the list
    public StickerInfo GetRandomSticker()
    {
    int randomIndex = _sysRandom.Next(0, possibleStickers.Count);
        
        return possibleStickers[randomIndex]; 
    }
}
