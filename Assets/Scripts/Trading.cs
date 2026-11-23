
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

    public Button checkmarkButton;

    [Header("Sticker Selection")]
    public GameObject selectedStickerImage;
    public GameObject stickerSelectionPanel;

    [Header("Request Buttons")]
    public GameObject requestbutton;
    public GameObject requestbutton2;

    public GameObject parentpopup;

    public void CloseParentpopup()
    {
        parentpopup.SetActive(false);
    }

    private void Start()
    {
        UpdateCheckmarkButton();
    }

    private void UpdateCheckmarkButton()
    {
        if (checkmarkButton != null)
        {
            checkmarkButton.interactable = currentOfferCount > 0;
        }
    }

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
        if (currentOfferCount == 0)
        {
            Debug.Log("Add at least one sticker before continuing!");
            return;
        }

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

        if (tradeBot.offeredStickers == null ||
            tradeBot.offeredStickers.Count == 0)
        {
            Debug.LogError("The bot has no stickers to offer!");
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

        // Make sure all player-offered stickers are valid.
        for (int i = 0; i < currentOfferCount; i++)
        {
            if (playerOfferedStickers[i] == null)
            {
                Debug.LogError("A player-offered sticker is missing!");
                return;
            }
        }

        // Make sure all bot-offered stickers are valid.
        foreach (StickerInfo sticker in tradeBot.offeredStickers)
        {
            if (sticker == null)
            {
                Debug.LogError("A bot-offered sticker is missing!");
                return;
            }
        }

        // Check that the player still owns every offered sticker.
        Dictionary<StickerInfo, int> requiredStickers =
            new Dictionary<StickerInfo, int>();

        for (int i = 0; i < currentOfferCount; i++)
        {
            StickerInfo sticker = playerOfferedStickers[i];

            if (!requiredStickers.ContainsKey(sticker))
                requiredStickers[sticker] = 0;

            requiredStickers[sticker]++;
        }

        foreach (KeyValuePair<StickerInfo, int> entry in requiredStickers)
        {
            int ownedCount = 0;

            foreach (StickerInfo ownedSticker in inventory.myStickers)
            {
                if (ownedSticker == entry.Key)
                    ownedCount++;
            }

            if (ownedCount < entry.Value)
            {
                Debug.LogError(
                    "You no longer own enough copies of " +
                    entry.Key.stickerName
                );
                return;
            }
        }

        // Remove all stickers offered by the player.
        for (int i = 0; i < currentOfferCount; i++)
        {
            inventory.removeSticker(playerOfferedStickers[i]);
        }

        // Add ALL stickers offered by the bot.
        foreach (StickerInfo sticker in tradeBot.offeredStickers)
        {
            inventory.addSticker(sticker);
        }

        // Mark trade as completed.
        tradeBot.tradeCompleted = true;

        confirm.SetActive(false);

        Debug.Log(
            "Trade completed! Player gave " +
            currentOfferCount +
            " stickers and received " +
            tradeBot.offeredStickers.Count +
            " stickers."
        );

        // Clear all player offer slots.
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

        // Disable checkmark again.
        UpdateCheckmarkButton();
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

        // Count how many copies are already offered.
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

        UpdateCheckmarkButton();

        // Close the selection panel.
        stickerSelectionPanel.SetActive(false);
        selectedStickerImage.SetActive(false);
    }
}
