
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Trading : MonoBehaviour
{
    [Header("Inventory")]
    public Inventory inventory;

    [Header("Bot")]
    public TradeBot tradeBot;

    [Header("Popups")]
    public GameObject firstPopup;
    public GameObject stickerselection;
    public GameObject profile;
    public GameObject requestOne;
    public GameObject requestTwo;

    [Header("Player Offer")]
    public GameObject playerOfferPrefab;

    public Transform[] playerOfferSlots = new Transform[3];

    public GameObject[] playerOfferObjects = new GameObject[3];

    public StickerInfo[] playerOfferedStickers = new StickerInfo[3];

    private int currentOfferCount = 0;

    [Header("Options")]
    public GameObject option;
    public GameObject option2;
    public GameObject stickerOne;
    public GameObject stickerTwo;

    [Header("Trade Buttons and Panels")]
    public GameObject cancel;
    public GameObject confirm;
    public GameObject checkmark;

    [Header("Sticker Selection")]
    public GameObject selectedStickerImage;
    public GameObject stickerSelectionPanel;

    [Header("Request Buttons")]
    public GameObject requestbutton;
    public GameObject requestbutton2;


    public void OpenPopupOne()
    {
        firstPopup.SetActive(true);
        requestbutton.SetActive(false);
        requestbutton2.SetActive(false);
    }

    public void BotRequestOne()
    {
        requestOne.SetActive(false);
        profile.SetActive(false);
    }

    public void Openstickerselection()
    {
        if (currentOfferCount >= 3)
        {
            Debug.Log("You can only add 3 stickers!");
            return;
        }

        stickerselection.SetActive(true);
    }

    public void Closestickerselection()
    {
        stickerselection.SetActive(false);
    }

    public void have()
    {
        option.SetActive(false);

        Invoke("Confirm", 2f);
        Invoke("stickerOneFalse", 2f);
    }

    public void stickerOneFalse()
    {
        stickerOne.SetActive(false);
        profile.SetActive(false);

        if (playerOfferObjects[0] != null)
        {
            Destroy(playerOfferObjects[0]);
            playerOfferObjects[0] = null;
        }
    }

    public void dontHave()
    {
        requestOne.SetActive(false);
        option.SetActive(false);
        requestTwo.SetActive(true);
        option2.SetActive(true);
    }

    public void dontHaveAgain()
    {
        requestTwo.SetActive(false);
        option2.SetActive(false);
        profile.SetActive(false);

        Cancel();
    }

    public void haveAgain()
    {
        stickerTwo.SetActive(true);
        Confirm();
    }

    public void ClosePopupOne()
    {
        firstPopup.SetActive(false);
    }

    public void Cancel()
    {
        cancel.SetActive(true);
        requestOne.SetActive(false);
        requestTwo.SetActive(false);
        profile.SetActive(false);
    }

    public void Checkmark()
    {
        checkmark.SetActive(true);
        requestOne.SetActive(false);
        requestTwo.SetActive(false);
        profile.SetActive(false);
    }

    public void Confirm()
    {
        confirm.SetActive(true);
        checkmark.SetActive(false);
    }

    public void CompleteTrade()
    {
        Debug.Log("COMPLETE TRADE FUNCTION WAS CALLED!");

        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned!");
            return;
        }

        if (tradeBot == null)
        {
            Debug.LogError("TradeBot is not assigned!");
            return;
        }

        if (tradeBot.currentStickerInfo == null)
        {
            Debug.LogError("Bot sticker information is missing!");
            return;
        }

        if (currentOfferCount == 0)
        {
            Debug.LogError("No player stickers were selected!");
            return;
        }

        if (tradeBot.tradeCompleted)
        {
            Debug.LogWarning("This trade has already been completed!");
            return;
        }

        // Remove all stickers offered by the player.
        for (int i = 0; i < currentOfferCount; i++)
        {
            if (playerOfferedStickers[i] != null)
            {
                inventory.removeSticker(playerOfferedStickers[i]);
            }
        }

        // Add the bot's sticker to the player's inventory.
        inventory.addSticker(tradeBot.currentStickerInfo);

        // Mark trade as completed.
        tradeBot.tradeCompleted = true;

        confirm.SetActive(false);

        Debug.Log("Successfully traded " + currentOfferCount + " stickers!");

        // Clear all offer slots.
        for (int i = 0; i < 3; i++)
        {
            playerOfferedStickers[i] = null;

            if (playerOfferObjects[i] != null)
            {
                Destroy(playerOfferObjects[i]);
                playerOfferObjects[i] = null;
            }
        }

        currentOfferCount = 0;
    }

    public void Back()
    {
        checkmark.SetActive(false);
        requestOne.SetActive(false);
        requestTwo.SetActive(false);
        profile.SetActive(false);
    }

    public void Home(string name)
    {
        SceneManager.LoadScene(name);
    }

    


public void SelectSticker(StickerInfo selectedSticker)
{
    if (selectedSticker == null)
        return;

    if (currentOfferCount >= 3)
    {
        Debug.Log("You can only add 3 stickers!");
        return;
    }

    // Count how many copies of this sticker the player owns.
    int ownedCount = 0;

    foreach (StickerInfo sticker in inventory.myStickers)
    {
        if (sticker == selectedSticker)
        {
            ownedCount++;
        }
    }

    // Count how many copies of this sticker are already offered.
    int offeredCount = 0;

    foreach (StickerInfo sticker in playerOfferedStickers)
    {
        if (sticker == selectedSticker)
        {
            offeredCount++;
        }
    }

    // Prevent offering more copies than the player owns.
    if (offeredCount >= ownedCount)
    {
        Debug.Log("You don't have another copy of this sticker!");
        return;
    }

    // Save the selected sticker.
    playerOfferedStickers[currentOfferCount] = selectedSticker;

    // Create the sticker in the next available slot.
    GameObject newSticker = Instantiate(
        playerOfferPrefab,
        playerOfferSlots[currentOfferCount]
    );

    // Set its image.
    Image image = newSticker.GetComponent<Image>();

    if (image != null)
    {
        image.sprite = selectedSticker.stickerImage;
    }

    playerOfferObjects[currentOfferCount] = newSticker;

    // Increase the offer count.
    currentOfferCount++;

    // Close the selection panel.
    stickerSelectionPanel.SetActive(false);
    selectedStickerImage.SetActive(false);
}


}
