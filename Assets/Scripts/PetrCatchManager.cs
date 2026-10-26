
using UnityEngine;
using TMPro;
public class PetrCatchManager : MonoBehaviour
{
    public GameObject catchPanel;
    public GameObject catchResultPanel;
    public TMP_Text catchResultText;

    public GameObject stickerDetectedPanel;

    public Inventory inventory;

    public void ChooseCommonAnt()
    {
        TryCatchSticker("Common Ant");
    }

    public void ChooseGoldenAnt()
    {
        TryCatchSticker("Golden Ant");
    }

    public void ChooseDiamondAnt()
    {
        TryCatchSticker("Diamond Ant");
    }

    
private void TryCatchSticker(string antType)
{
    Debug.Log("TryCatchSticker is running!");
    catchPanel.SetActive(false);

    if (stickerDetectedPanel != null)
{
    stickerDetectedPanel.SetActive(false);

    Debug.Log("StickerDetectedPanel active: "
        + stickerDetectedPanel.activeSelf);
}
else
{
    Debug.LogWarning("StickerDetectedPanel is not assigned!");
}

    PetrStickerClick sticker =
        PetrStickerClick.GetSelectedSticker();

    if (sticker == null)
    {
        return;
    }

    bool success = false;

    if (sticker.stickerType ==
        PetrStickerClick.StickerType.Common)
    {
        success = true;
    }
    else if (sticker.stickerType ==
             PetrStickerClick.StickerType.Rare)
    {
        success = antType == "Golden Ant" ||
                  antType == "Diamond Ant";
    }
    else if (sticker.stickerType ==
             PetrStickerClick.StickerType.Legendary)
    {
        success = antType == "Diamond Ant";
    }

    if (success)
{
    catchResultText.text = "Catch successful!";

    if (inventory != null && sticker.stickerInfo != null)
    {
        inventory.addSticker(sticker.stickerInfo);
        Debug.Log("Added to inventory: " +
                  sticker.stickerInfo.stickerName);
    }
    else
    {
        Debug.LogWarning(
            "Inventory or StickerInfo is missing!");
    }

    sticker.CatchSuccessfully();
}
    else
    {
        catchResultText.text =
            "Catch failed! Try a different ant.";

        sticker.HideActionButtons();
    }

    catchResultPanel.SetActive(true);
    
}

    
}
