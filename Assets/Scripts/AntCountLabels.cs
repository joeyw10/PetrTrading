using UnityEngine;
using TMPro;

/// <summary>
/// Shows how many of each ant the player owns on the Ants tab.
/// Put this on the Ants Panel. It updates whenever the panel is shown,
/// and again whenever a count changes while it's open.
/// </summary>
public class AntCountLabels : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [Header("The number labels (the blue circles)")]
    [SerializeField] private TMP_Text commonLabel;
    [SerializeField] private TMP_Text goldenLabel;
    [SerializeField] private TMP_Text diamondLabel;

    // Last numbers shown, so the text is only rewritten when a count actually changes
    private int _common = -1;
    private int _golden = -1;
    private int _diamond = -1;

    private void OnEnable()
    {
        // Force a redraw each time the tab is opened
        _common = _golden = _diamond = -1;
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (inventory == null) return;

        int common = inventory.getCommonAnts();
        int golden = inventory.getGoldenAnts();
        int diamond = inventory.getDiamondAnts();

        if (common != _common)
        {
            _common = common;
            if (commonLabel != null) commonLabel.text = common.ToString();
        }

        if (golden != _golden)
        {
            _golden = golden;
            if (goldenLabel != null) goldenLabel.text = golden.ToString();
        }

        if (diamond != _diamond)
        {
            _diamond = diamond;
            if (diamondLabel != null) diamondLabel.text = diamond.ToString();
        }
    }
}
