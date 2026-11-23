
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetrStickerRequestUI : MonoBehaviour
{
    [Header("Popup & Add Button")]
    public GameObject stickerSelectionPopup;
    public Button mainAddButton;

    [Header("Bot Trade Connection")]
    public TradeBot tradeBot;

    [Header("Request Display Image")]
    public Image requestDisplayImage;

    [Header("The 4 Petr Sticker Prefabs")]
    public List<GameObject> petrStickerPrefabs;

    [Header("Popup Button Images")]
    public List<Image> popupButtonImages;

    private Coroutine hideImageCoroutine;

    private void Start()
    {
        if (stickerSelectionPopup != null)
            stickerSelectionPopup.SetActive(false);

        SetBubbleVisible(false);
        UpdatePopupButtonSprites();
    }

    private void UpdatePopupButtonSprites()
    {
        for (int i = 0; i < popupButtonImages.Count; i++)
        {
            if (i < petrStickerPrefabs.Count &&
                petrStickerPrefabs[i] != null &&
                popupButtonImages[i] != null)
            {
                Sprite stickerSprite = GetSpriteFromPrefab(petrStickerPrefabs[i]);

                if (stickerSprite != null)
                {
                    popupButtonImages[i].sprite = stickerSprite;
                    popupButtonImages[i].preserveAspect = true;
                }
            }
        }
    }

    public void OpenStickerPopup()
    {
        if (stickerSelectionPopup != null)
            stickerSelectionPopup.SetActive(true);
    }

    public void SelectPetrStickerByIndex(int index)
    {
        if (index < 0 || index >= petrStickerPrefabs.Count)
            return;

        GameObject chosenPrefab = petrStickerPrefabs[index];

        if (chosenPrefab == null)
            return;

        if (tradeBot == null)
        {
            Debug.LogWarning("TradeBot is not assigned!");
            return;
        }

        // Ask the bot to add the exact requested sticker
        bool successfullyAdded = tradeBot.AddRequestedSticker(chosenPrefab);

        if (!successfullyAdded)
        {
            Debug.Log("The bot could not add this requested sticker.");
            return;
        }

        // Close the popup after a successful request
        if (stickerSelectionPopup != null)
            stickerSelectionPopup.SetActive(false);

        // Display the requested sticker in the chat bubble
        Sprite stickerSprite = GetSpriteFromPrefab(chosenPrefab);
        ShowRequestImage(stickerSprite);
    }

    private Sprite GetSpriteFromPrefab(GameObject prefab)
    {
        Image img = prefab.GetComponentInChildren<Image>();

        if (img != null)
            return img.sprite;

        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
            return sr.sprite;

        return null;
    }

    private void ShowRequestImage(Sprite stickerSprite)
    {
        if (requestDisplayImage == null || stickerSprite == null)
            return;

        requestDisplayImage.sprite = stickerSprite;
        requestDisplayImage.preserveAspect = true;

        SetBubbleVisible(true);

        if (hideImageCoroutine != null)
            StopCoroutine(hideImageCoroutine);

        hideImageCoroutine = StartCoroutine(HideImageAfterDelay(4f));
    }

    private IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetBubbleVisible(false);
        hideImageCoroutine = null;
    }

    private void SetBubbleVisible(bool isVisible)
    {
        if (requestDisplayImage == null)
            return;

        if (requestDisplayImage.transform.parent != null)
        {
            requestDisplayImage.transform.parent.gameObject.SetActive(isVisible);
        }
        else
        {
            requestDisplayImage.gameObject.SetActive(isVisible);
        }
    }
}
