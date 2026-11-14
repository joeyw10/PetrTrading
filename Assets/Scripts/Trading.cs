
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
    public Transform secondSlot;
    public GameObject playerOfferOne;

    // The sticker selected by the player to trade away
    public StickerInfo playerOfferedSticker;

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
        stickerselection.SetActive(true);
    }

    public void Closestickerselection()
    {
        stickerselection.SetActive(false);
    }

    public void have()
    {
        option.SetActive(false);

        playerOfferOne = Instantiate(playerOfferPrefab, secondSlot);

        Invoke("Confirm", 2f);
        Invoke("stickerOneFalse", 2f);
    }

    public void stickerOneFalse()
    {
        stickerOne.SetActive(false);
        profile.SetActive(false);

        Destroy(playerOfferOne);
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

    // Opens the confirmation panel.
    public void Confirm()
    {
        confirm.SetActive(true);
        checkmark.SetActive(false);
    }

    // Completes the trade and updates the player's inventory.
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

        if (playerOfferedSticker == null)
        {
            Debug.LogError("No player sticker was selected for trading!");
            return;
        }

        if (tradeBot.tradeCompleted)
        {
            Debug.LogWarning("This trade has already been completed!");
            return;
        }

        // Remove the sticker offered by the player.
        inventory.removeSticker(playerOfferedSticker);

        // Add the bot's sticker to the player's inventory.
        inventory.addSticker(tradeBot.currentStickerInfo);

        // Mark the trade as completed.
        tradeBot.tradeCompleted = true;

        confirm.SetActive(false);

        Debug.Log("Traded away: " + playerOfferedSticker.stickerName);
        Debug.Log("Successfully received: " +
                  tradeBot.currentStickerInfo.stickerName);

        // Clear the selected sticker.
        playerOfferedSticker = null;
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

        // Remember which sticker the player is offering.
        playerOfferedSticker = selectedSticker;

        Image image = selectedStickerImage.GetComponent<Image>();

        image.sprite = selectedSticker.stickerImage;

        selectedStickerImage.SetActive(true);
        stickerSelectionPanel.SetActive(false);
    }
}
