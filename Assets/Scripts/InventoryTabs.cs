using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the "My stickers / Sticker packs / Ants" tab row.
/// Each tab has: a Button, an underline Image (shown only when selected),
/// and a content panel (shown only when selected).
/// </summary>
public class InventoryTabs : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public Button button;        // the clickable tab label
        public GameObject underline; // thin line under the label
        public GameObject panel;     // the screen content for this tab
    }

    [SerializeField] private Tab[] tabs;
    [SerializeField] private int startingTab = 0;

    private void Start()
    {
        // Hook up each button so it selects its own tab
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i; // copy for the closure
            if (tabs[i].button != null)
                tabs[i].button.onClick.AddListener(() => SelectTab(index));
        }

        SelectTab(startingTab);
    }

    public void SelectTab(int selectedIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            bool isSelected = (i == selectedIndex);

            if (tabs[i].underline != null) tabs[i].underline.SetActive(isSelected);
            if (tabs[i].panel != null)     tabs[i].panel.SetActive(isSelected);
        }
    }
}
