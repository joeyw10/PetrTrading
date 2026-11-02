using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the binder from the player's Inventory instead of hand-placed stickers.
/// Put this on the Binder object. It removes the stickers placed by hand in the pages,
/// then creates one slot per owned sticker, filling the pages in order.
/// </summary>
public class BinderPopulator : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [Tooltip("A sticker slot saved as a prefab. It must have the Rarity Sort component.")]
    [SerializeField] private RaritySort stickerSlotPrefab;

    [Tooltip("Same Page 1 / Page 2 list used by the Binder Swipe Controller.")]
    [SerializeField] private GameObject[] stickerPages;

    [Tooltip("How many stickers fit on one page (a 2x2 grid = 4).")]
    [SerializeField] private int stickersPerPage = 4;

    private void Start()
    {
        Populate();
    }

    public void Populate()
    {
        if (inventory == null || stickerSlotPrefab == null || stickerPages == null || stickerPages.Length == 0)
        {
            Debug.LogError("BinderPopulator: assign the Inventory, Sticker Slot Prefab and Sticker Pages in the Inspector.");
            return;
        }

        // 1. Remove the hand-placed stickers
        foreach (GameObject page in stickerPages)
        {
            if (page == null) continue;

            foreach (RaritySort old in page.GetComponentsInChildren<RaritySort>(true))
            {
                old.transform.SetParent(null); // detach first so positions are counted correctly
                Destroy(old.gameObject);
            }
        }

        // 2. Create one slot per owned sticker, filling the pages in order
        int perPage = Mathf.Max(1, stickersPerPage);
        int count = 0;

        foreach (StickerInfo owned in inventory.myStickers)
        {
            if (owned == null) continue;

            int pageIndex = count / perPage;
            if (pageIndex >= stickerPages.Length || stickerPages[pageIndex] == null)
            {
                Debug.LogWarning("BinderPopulator: more stickers than pages can hold. Add more pages.");
                break;
            }

            RaritySort slot = Instantiate(stickerSlotPrefab, stickerPages[pageIndex].transform);
            slot.Setup(owned);
            count++;
        }

        // 3. Refresh the grid layouts
        foreach (GameObject page in stickerPages)
        {
            if (page != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(page.GetComponent<RectTransform>());
        }
    }
}
