using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The "Sticker packs" tab: shows today's free packs.
/// Tapping an unopened pack marks it opened and loads the pack-opening scene.
/// When you come back, opened packs show their "opened" sprite (the grey-overlay version).
/// Which packs were opened is saved, and everything resets when the date changes.
/// </summary>
public class DailyPacksPanel : MonoBehaviour
{
    [System.Serializable]
    public class PackSlot
    {
        public Button button;        // the pack's button (tap to open)
        public Image image;          // the pack's picture (usually the button's own Image)
        public Sprite openedSprite;  // same art with a grey overlay, shown once opened

        [HideInInspector] public Sprite normalSprite; // remembered automatically at start
    }

    [Header("The free packs")]
    [SerializeField] private PackSlot[] slots;

    [Header("Scene to load when a pack is tapped")]
    [Tooltip("Exact name of your home / pack opening scene. It must be in the build's scene list.")]
    [SerializeField] private string packOpeningSceneName = "Home";

    [Header("Coming back")]
    [Tooltip("Which tab number to show when returning to the Inventory scene (Sticker packs = 1).")]
    [SerializeField] private int packsTabIndex = 1;

    [Header("Fallback look (only used if a slot has no Opened Sprite)")]
    [SerializeField] private Color openedTint = new Color(0.45f, 0.45f, 0.45f, 1f);

    [Header("When a pack is tapped")]
    [Tooltip("Runs with the pack's number (0-3) just before the scene loads. Optional.")]
    public UnityEvent<int> onPackOpened;

    // Other scenes can read which pack was tapped (0 = first pack). -1 = none.
    public static int LastTappedPack = -1;

    private const string DateKey = "dailypacks_date";

    private static string OpenedKey(int index)
    {
        return "dailypacks_opened_" + index;
    }

    private static string Today()
    {
        return DateTime.Now.ToString("yyyy-MM-dd");
    }

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Remember the normal picture so we can switch back after a daily reset
            if (slots[i].image != null)
                slots[i].normalSprite = slots[i].image.sprite;

            int index = i; // copy so each button remembers its own number
            if (slots[i].button != null)
                slots[i].button.onClick.AddListener(() => OpenPack(index));
        }
    }

    // Runs every time the tab is shown, so it always reflects today's state
    private void OnEnable()
    {
        ResetIfNewDay();
        Refresh();
    }

    private void ResetIfNewDay()
    {
        if (PlayerPrefs.GetString(DateKey, "") == Today()) return;

        for (int i = 0; i < slots.Length; i++)
            PlayerPrefs.SetInt(OpenedKey(i), 0);

        PlayerPrefs.SetString(DateKey, Today());
        PlayerPrefs.Save();
    }

    public bool IsOpened(int index)
    {
        return PlayerPrefs.GetInt(OpenedKey(index), 0) == 1;
    }

    public void OpenPack(int index)
    {
        ResetIfNewDay(); // in case midnight passed while the screen was open
        if (index < 0 || index >= slots.Length || IsOpened(index)) return; // already opened: do nothing

        // Save it as opened right away, so it's already switched when the player returns
        PlayerPrefs.SetInt(OpenedKey(index), 1);
        PlayerPrefs.Save();

        LastTappedPack = index;
        onPackOpened.Invoke(index);

        // Make the Inventory scene reopen on this tab when we come back
        InventoryTabs.NextStartTab = packsTabIndex;

        if (!string.IsNullOrEmpty(packOpeningSceneName))
            SceneManager.LoadScene(packOpeningSceneName);
        else
            Refresh(); // no scene set: just update the look
    }

    private void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            PackSlot slot = slots[i];
            if (slot.image == null) continue;

            if (IsOpened(i))
            {
                if (slot.openedSprite != null)
                {
                    slot.image.sprite = slot.openedSprite;
                    slot.image.color = Color.white;
                }
                else
                {
                    slot.image.color = openedTint; // fallback: plain grey tint
                }
            }
            else
            {
                if (slot.normalSprite != null) slot.image.sprite = slot.normalSprite;
                slot.image.color = Color.white;
            }
        }
    }

    // For testing: right-click the component header in the Inspector during Play mode.
    [ContextMenu("Reset Today's Packs")]
    private void ResetForTesting()
    {
        for (int i = 0; i < slots.Length; i++)
            PlayerPrefs.SetInt(OpenedKey(i), 0);
        PlayerPrefs.Save();
        Refresh();
    }
}