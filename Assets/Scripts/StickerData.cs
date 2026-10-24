using UnityEngine;

public class StickerRarity : MonoBehaviour
{
    // Define an enum to easily select the rarity from an inspector dropdown
    public enum Rarity { All, Common, Rare, Epic, Legendary }
    
    [Header("Sticker Settings")]
    public Rarity stickerRarity;
}
