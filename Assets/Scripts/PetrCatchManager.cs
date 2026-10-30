
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

        // Make sure the inventory exists
        if (inventory == null)
        {
            Debug.LogWarning("Inventory is not assigned!");
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

        // Get the currently selected sticker
        PetrStickerClick sticker =
            PetrStickerClick.GetSelectedSticker();

        if (sticker == null)
        {
            Debug.LogWarning("No sticker is selected!");
            return;
        }

        bool success = false;

        // Common stickers can be caught with any ant
        if (sticker.stickerType ==
            PetrStickerClick.StickerType.Common)
        {
            success = true;
        }
        // Rare stickers require Golden or Diamond Ants
        else if (sticker.stickerType ==
                 PetrStickerClick.StickerType.Rare)
        {
            success = antType == "Golden Ant" ||
                      antType == "Diamond Ant";
        }
        // Legendary stickers require Diamond Ants
        else if (sticker.stickerType ==
                 PetrStickerClick.StickerType.Legendary)
        {
            success = antType == "Diamond Ant";
        }

        if (success)
        {
            catchResultText.text = "Catch successful!";

            // Add the caught sticker to inventory
            if (sticker.stickerInfo != null)
            {
                inventory.addSticker(sticker.stickerInfo);

                Debug.Log("Added to inventory: " +
                          sticker.stickerInfo.stickerName);
            }
            else
            {
                Debug.LogWarning(
                    "StickerInfo is missing!");
            }

            // Subtract one ant of the type used
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

            // Complete the catch
            sticker.CatchSuccessfully();
        }
        else
        {
            catchResultText.text =
                "Catch failed! Try a different ant.";

            // Failed catches do not consume ants
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
}
