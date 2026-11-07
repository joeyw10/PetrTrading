using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Builds the binder from the player's Inventory instead of hand-placed stickers.
/// Put this on the Binder object (next to Binder Swipe Controller).
/// It makes sure there are enough pages for every sticker the player owns (never fewer than
/// Minimum Pages), creates one slot per sticker, and gives the page list to the swipe controller.
/// </summary>
[RequireComponent(typeof(BinderSwipeController))]
public class BinderPopulator : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [Tooltip("A sticker slot saved as a prefab ASSET (from Assets/Prefabs). It must have the Rarity Sort component.")]
    [SerializeField] private Sort stickerSlotPrefab;

    [Tooltip("How many stickers fit on one page (a 2x2 grid = 4). Keep this the same as on Binder Swipe Controller.")]
    [SerializeField] private int stickersPerPage = 4;

    [Tooltip("The binder always has at least this many pages, even when it is empty.")]
    [SerializeField] private int minimumPages = 3;

    private void Start()
    {
        Populate();
    }

    public void Populate()
    {
        BinderSwipeController swipe = GetComponent<BinderSwipeController>();

        if (inventory == null || stickerSlotPrefab == null || swipe == null ||
            swipe.stickerPages == null || swipe.stickerPages.Length == 0)
        {
            Debug.LogError("BinderPopulator: assign the Inventory and Sticker Slot Prefab here, and the Sticker Pages on the Binder Swipe Controller.");
            return;
        }

        if (inventory.myStickers == null)
            inventory.myStickers = new List<StickerInfo>();

        // A scene object here still works, but a prefab asset from Assets/Prefabs is the right setup
        if (stickerSlotPrefab.gameObject.scene.IsValid())
        {
            Debug.LogWarning("BinderPopulator: the Sticker Slot Prefab is an object in the scene, not a prefab asset. " +
                             "Drag it into Assets/Prefabs and use that prefab instead.");
        }

        // How many pages do we need?
        int owned = 0;
        foreach (StickerInfo sticker in inventory.myStickers)
        {
            if (sticker != null) owned++;
        }

        int perPage = Mathf.Max(1, stickersPerPage);
        int pagesNeeded = Mathf.Max(Mathf.Max(1, minimumPages), Mathf.CeilToInt(owned / (float)perPage));

        Debug.Log("BinderPopulator: inventory '" + inventory.name + "' has " + owned + " stickers, so the binder needs " + pagesNeeded + " pages.");

        GameObject[] existing = swipe.stickerPages;

        // 1. Remove the hand-placed stickers from the pages that already exist
        GameObject template = null;
        foreach (GameObject page in existing)
        {
            if (page == null) continue;
            RemoveStickers(page);
            template = page; // the last page found is copied when more pages are needed
        }

        if (template == null)
        {
            Debug.LogError("BinderPopulator: none of the Sticker Pages on the Binder Swipe Controller are assigned.");
            return;
        }

        // 2. Use the pages that exist, then copy the last one for any extra
        List<GameObject> pages = new List<GameObject>();
        for (int i = 0; i < pagesNeeded; i++)
        {
            if (i < existing.Length && existing[i] != null)
            {
                pages.Add(existing[i]);
            }
            else
            {
                GameObject copy = Instantiate(template, template.transform.parent, false);
                copy.name = "Page " + (i + 1);
                RemoveStickers(copy); // the copy also copied the template's stickers
                pages.Add(copy);
            }
        }

        // Pages in the scene beyond what's needed are switched off for good
        for (int i = pagesNeeded; i < existing.Length; i++)
        {
            if (existing[i] != null) existing[i].SetActive(false);
        }

        // 3. Create one slot per owned sticker, filling the pages in order
        int count = 0;
        foreach (StickerInfo sticker in inventory.myStickers)
        {
            if (sticker == null) continue;

            int pageIndex = count / perPage; // always a page that exists
            Sort slot = Instantiate(stickerSlotPrefab, pages[pageIndex].transform, false);
            slot.gameObject.SetActive(true); // in case the template was switched off
            slot.Setup(sticker);
            count++;
        }

        Debug.Log("BinderPopulator: created " + count + " sticker slots on " + pages.Count + " pages.");

        // 4. Refresh the grid layouts, then tell the swipe controller about the pages
        foreach (GameObject page in pages)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(page.GetComponent<RectTransform>());
        }

        swipe.SetPages(pages.ToArray());
    }

    // Switches the stickers off first (layouts ignore switched-off objects) and deletes them.
    // They are NOT moved out of the page, because moving UI out of the Canvas changes its size.
    private static void RemoveStickers(GameObject page)
    {
        foreach (Sort old in page.GetComponentsInChildren<Sort>(true))
        {
            old.gameObject.SetActive(false);
            Destroy(old.gameObject);
        }
    }
}