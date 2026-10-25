using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RaritySort : MonoBehaviour, IPointerClickHandler
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

    [Header("Sticker Asset")]
    public StickerInfo stickerData;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (stickerData != null)
        {
            // 1. Pass this sticker's ScriptableObject to the bridge
            StickerDataBridge.SelectedSticker = stickerData;

            // 2. Open your Sticker Info scene (Make sure name matches exactly)
            SceneManager.LoadScene("Sticker Info");
        }
    }
}
