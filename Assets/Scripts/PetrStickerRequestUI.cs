using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetrStickerRequestUI : MonoBehaviour
{
    [Header("Popup & Add Button")]
    public GameObject stickerSelectionPopup; // Your 2x2 popup panel
    public Button mainAddButton;             // Your main "Add" / "Request" button

    [Header("Request Display Image")]
    public Image requestDisplayImage;        // The child Image inside ChatBubbleBackground

    [Header("The 4 Petr Sticker Prefabs")]
    public List<GameObject> petrStickerPrefabs; // Drag your 4 sticker PREFABS here

    [Header("Popup Button Images")]
    public List<Image> popupButtonImages;       // Drag your 4 popup buttons here

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
            if (i < petrStickerPrefabs.Count && petrStickerPrefabs[i] != null && popupButtonImages[i] != null)
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

    // 1. Hook your main button to this method
    public void OpenStickerPopup()
    {
        if (stickerSelectionPopup != null)
            stickerSelectionPopup.SetActive(true);
    }

    // 2. Hook your 4 popup buttons to this method (using index 0, 1, 2, and 3)
    public void SelectPetrStickerByIndex(int index)
    {
        if (index < 0 || index >= petrStickerPrefabs.Count) return;

        GameObject chosenPrefab = petrStickerPrefabs[index];
        if (chosenPrefab == null) return;

        // Close the popup
        if (stickerSelectionPopup != null)
            stickerSelectionPopup.SetActive(false);

        // Display the chosen sticker sprite inside the chat bubble
        Sprite stickerSprite = GetSpriteFromPrefab(chosenPrefab);
        ShowRequestImage(stickerSprite);
    }

    private Sprite GetSpriteFromPrefab(GameObject prefab)
    {
        Image img = prefab.GetComponentInChildren<Image>();
        if (img != null) return img.sprite;

        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) return sr.sprite;

        return null;
    }

    private void ShowRequestImage(Sprite stickerSprite)
    {
        if (requestDisplayImage == null || stickerSprite == null) return;

        requestDisplayImage.sprite = stickerSprite;
        requestDisplayImage.preserveAspect = true;
        SetBubbleVisible(true);

        // Restart the 2-second hide timer if the player clicks another sticker quickly
        if (hideImageCoroutine != null)
            StopCoroutine(hideImageCoroutine);

        hideImageCoroutine = StartCoroutine(HideImageAfterDelay(8f));
    }

    private IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetBubbleVisible(false);
    }

    private void SetBubbleVisible(bool isVisible)
    {
        if (requestDisplayImage == null) return;

        // Toggles the parent ChatBubbleBackground (and the sticker inside it) together
        if (requestDisplayImage.transform.parent != null)
            requestDisplayImage.transform.parent.gameObject.SetActive(isVisible);
        else
            requestDisplayImage.gameObject.SetActive(isVisible);
    }
}