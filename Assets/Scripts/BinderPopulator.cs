using System.Collections.Generic;
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

    [Tooltip("A sticker slot saved as a prefab ASSET (from Assets/Prefabs). It must have the Rarity Sort component.")]
    [SerializeField] private Sort stickerSlotPrefab;

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

        if (inventory.myStickers == null)
            inventory.myStickers = new List<StickerInfo>();

        Debug.Log("BinderPopulator: inventory '" + inventory.name + "' has " + inventory.myStickers.Count + " stickers.");

        // A scene object here still works, but a prefab asset from Assets/Prefabs is the right setup
        if (stickerSlotPrefab.gameObject.scene.IsValid())
        {
            Debug.LogWarning("BinderPopulator: the Sticker Slot Prefab is an object in the scene, not a prefab asset. " +
                             "Drag it into Assets/Prefabs and use that prefab instead.");
        }

        // 1. Remove the hand-placed stickers. They're switched off first (layouts ignore switched-off
        //    objects) and are NOT moved out of the page, because moving UI out of the Canvas changes its size.
        foreach (GameObject page in stickerPages)
        {
            if (page == null) continue;

            foreach (Sort old in page.GetComponentsInChildren<Sort>(true))
            {
                old.gameObject.SetActive(false);
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

            Sort slot = Instantiate(stickerSlotPrefab, stickerPages[pageIndex].transform, false);
            slot.gameObject.SetActive(true); // in case the template was switched off
            slot.Setup(owned);
            count++;
        }

        Debug.Log("BinderPopulator: created " + count + " sticker slots.");

        // 3. Refresh the grid layouts
        foreach (GameObject page in stickerPages)
        {
            if (page != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(page.GetComponent<RectTransform>());
        }
    }
}