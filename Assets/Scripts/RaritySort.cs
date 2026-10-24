using UnityEngine;

public class RaritySort : MonoBehaviour
{
    // Define an enum to easily select the rarity from an inspector dropdown
    public enum Rarity { All, Common, Rare, Epic, Legendary }
    
    [Header("Sticker Settings")]
    public Rarity stickerRarity;

    // These store where the sticker originally belongs in your book layout
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public int originalSiblingIndex;

    private void Awake()
    {
        // Remember the home page and order location as soon as the game starts
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
    }
}
