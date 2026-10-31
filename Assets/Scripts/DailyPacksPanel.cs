using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// The "Sticker packs" tab: shows today's free packs.
/// Unopened packs look normal and can be tapped; opened packs are greyed out and locked.
/// Which packs were opened is saved, and everything resets when the date changes.
/// </summary>
public class DailyPacksPanel : MonoBehaviour
{
    [System.Serializable]
    public class PackSlot
    {
        public Button button;  // the pack's button (tap to open)
        public Image image;    // the pack's picture (usually the button's own Image)
    }

    [Header("The 4 free packs")]
    [SerializeField] private PackSlot[] slots;

    [Header("Look")]
    [SerializeField] private Color unopenedColor = Color.white;
    [SerializeField] private Color openedColor = new Color(0.45f, 0.45f, 0.45f, 0.7f);

    [Header("When a pack is tapped")]
    [Tooltip("Runs with the pack's number (0-3). Hook your pack-opening code here.")]
    public UnityEvent<int> onPackOpened;

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
        if (index < 0 || index >= slots.Length || IsOpened(index)) return;

        PlayerPrefs.SetInt(OpenedKey(index), 1);
        PlayerPrefs.Save();
        Refresh();

        onPackOpened.Invoke(index);
    }

    private void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            bool opened = IsOpened(i);

            if (slots[i].image != null)
                slots[i].image.color = opened ? openedColor : unopenedColor;

            // Opened packs can't be tapped again
            if (slots[i].button != null)
                slots[i].button.interactable = !opened;
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
