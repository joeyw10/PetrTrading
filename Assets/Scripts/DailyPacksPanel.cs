using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The "Sticker packs" tab: shows today's free packs.
/// The Inventory hands out the free packs each day. This panel just shows them:
/// packs that have been opened today show their "opened" sprite, the rest look normal.
/// Tapping an unopened pack takes the player to the pack opening scene.
/// </summary>
public class DailyPacksPanel : MonoBehaviour
{
    [System.Serializable]
    public class PackSlot
    {
        public Button button;        // the pack's button (tap to go open it)
        public Image image;          // the pack's picture (usually the button's own Image)
        public Sprite openedSprite;  // same art with a grey overlay, shown once opened

        [HideInInspector] public Sprite normalSprite; // remembered automatically at start
    }

    [Header("The free packs (make this match Inventory's Daily Free Packs)")]
    [SerializeField] private PackSlot[] slots;

    [Header("Inventory")]
    [Tooltip("Tells this panel how many packs were opened today.")]
    [SerializeField] private Inventory inventory;

    [Header("Shop button (the 'Unlock more packs!' tile)")]
    [Tooltip("Optional. Goes to the same scene but is NOT a daily pack, so it never greys out.")]
    [SerializeField] private Button shopButton;

    [Header("Scene to load when a pack is tapped")]
    [Tooltip("Exact name of your home / pack opening scene. It must be in the build's scene list.")]
    [SerializeField] private string packOpeningSceneName = "Home";

    [Header("Coming back")]
    [Tooltip("Which tab number to show when returning to the Inventory scene (Sticker packs = 1).")]
    [SerializeField] private int packsTabIndex = 1;

    [Header("Fallback look (only used if a slot has no Opened Sprite)")]
    [SerializeField] private Color openedTint = new Color(0.45f, 0.45f, 0.45f, 1f);

    [Header("When a pack is tapped")]
    [Tooltip("Runs with the pack's number (0-2) just before the scene loads. Optional.")]
    public UnityEvent<int> onPackOpened;

    // Other scenes can read which pack was tapped (0 = first pack). -1 = none.
    public static int LastTappedPack = -1;

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Remember the normal picture so we can switch back on a new day
            if (slots[i].image != null)
                slots[i].normalSprite = slots[i].image.sprite;

            int index = i; // copy so each button remembers its own number
            if (slots[i].button != null)
                slots[i].button.onClick.AddListener(() => OpenPack(index));
        }

        if (shopButton != null)
            shopButton.onClick.AddListener(OpenShop);
    }

    // Runs every time the tab is shown, so it always reflects today's state
    private void OnEnable()
    {
        Refresh();
    }

    // The first N packs count as opened, where N = packs opened today
    public bool IsOpened(int index)
    {
        if (inventory == null) return false;
        return index < inventory.getPacksOpenedToday();
    }

    // The shop tile just travels to the scene; it is never greyed out
    public void OpenShop()
    {
        LastTappedPack = -1;
        GoToPackScene();
    }

    public void OpenPack(int index)
    {
        if (index < 0 || index >= slots.Length || IsOpened(index)) return; // already opened: do nothing

        LastTappedPack = index;
        onPackOpened.Invoke(index);

        GoToPackScene();
    }

    private void GoToPackScene()
    {
        // Make the Inventory scene reopen on this tab when we come back
        InventoryTabs.NextStartTab = packsTabIndex;

        if (!string.IsNullOrEmpty(packOpeningSceneName))
            SceneManager.LoadScene(packOpeningSceneName);
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

    // For testing: right-click the component header in the Inspector.
    // Gives a fresh day's packs right away (3 packs, none opened).
    [ContextMenu("Reset Today's Packs")]
    private void ResetForTesting()
    {
        if (inventory != null) inventory.ResetDailyPacksForTesting();
        Refresh();
    }
}