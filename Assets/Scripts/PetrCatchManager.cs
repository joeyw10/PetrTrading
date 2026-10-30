
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
        }

        if (inventory == null)
        {
            Debug.LogWarning("Inventory is not assigned!");
            return;
        }

        PetrStickerClick sticker =
            PetrStickerClick.GetSelectedSticker();

        if (sticker == null)
        {
            Debug.LogWarning("No sticker is selected!");
            return;
        }

        // Check whether the player has the selected ant
        if (!HasAnts(antType))
        {
            catchResultText.text =
                "You don't have any " + antType + "s left!";

            catchResultPanel.SetActive(true);
            Debug.LogWarning("Not enough ants: " + antType);
            return;
        }

        // Consume one ant for every catch attempt,
        // regardless of whether the catch succeeds.
        UseOneAnt(antType);

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

            if (sticker.stickerInfo != null)
            {
                inventory.addSticker(sticker.stickerInfo);

                Debug.Log("Added to inventory: " +
                          sticker.stickerInfo.stickerName);
            }
            else
            {
                Debug.LogWarning("StickerInfo is missing!");
            }

            sticker.CatchSuccessfully();
        }
        else
        {
            catchResultText.text =
                "Catch failed! Try using a different ant!";

            sticker.HideActionButtons();
        }

        catchResultPanel.SetActive(true);
    }

    private bool HasAnts(string antType)
    {
        if (antType == "Common Ant")
        {
            return inventory.getCommonAnts() > 0;
        }
        else if (antType == "Golden Ant")
        {
            return inventory.getGoldenAnts() > 0;
        }
        else if (antType == "Diamond Ant")
        {
            return inventory.getDiamondAnts() > 0;
        }

        return false;
    }

    private void UseOneAnt(string antType)
    {
        if (antType == "Common Ant")
        {
            inventory.setCommonAnts(-1);
        }
        else if (antType == "Golden Ant")
        {
            inventory.setGoldenAnts(-1);
        }
        else if (antType == "Diamond Ant")
        {
            inventory.setDiamondAnts(-1);
        }

        Debug.Log("Used one " + antType);
    }
}
