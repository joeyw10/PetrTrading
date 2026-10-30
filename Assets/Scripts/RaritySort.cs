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

    [Header("Sticker Asset")]
    public StickerInfo stickerData;

    private void Awake()
    {
        // Remember the home page and order location as soon as the game starts
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    // ---------- Favorites (stored by StickerFavorites, shared with the Sticker Info screen) ----------
    public bool IsFavorited
    {
        get { return StickerFavorites.IsFavorited(stickerData); }
    }

    // Right-click the component header in the Inspector during Play mode to test it.
    [ContextMenu("Toggle Favorite")]
    public void ToggleFavorite()
    {
        StickerFavorites.Toggle(stickerData);
    }

    // ---------- Date added (stored by StickerDates) ----------
    // Bigger number = added more recently. 0 = no date was ever recorded.
    public long AddedTicks
    {
        get { return StickerDates.GetAddedTicks(stickerData); }
    }

    // For testing: right-click the component header in the Inspector during Play mode
    // to stamp this sticker as "added right now" (only works the first time).
    [ContextMenu("Stamp As Added Now")]
    public void StampAddedNow()
    {
        StickerDates.RecordAdded(stickerData);
    }

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