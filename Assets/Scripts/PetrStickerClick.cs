
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetrStickerClick : MonoBehaviour
{
    public GameObject catchButton;
    public GameObject ignoreButton;
    public GameObject stickerDetectedPanel;
    public TMP_Text stickerDetectedText;
    public Image detectedStickerImage;
    public ButtonSound buttonSound;

    [Header("Automatic Detection")]
    public Transform playerMarker;
    public float detectionRadius = 200f;

    public enum StickerType
    {
        Common,
        Rare,
        Legendary
    }

    public StickerType stickerType;
    public StickerInfo stickerInfo;

    private static PetrStickerClick selectedSticker;

    void Update()
    {
        // Only detect a sticker when no other sticker is selected.
        if (selectedSticker != null)
        {
            return;
        }

        if (playerMarker == null)
        {
            return;
        }

        // Compare the sticker's screen position with the player marker.
        Vector2 stickerPosition =
            RectTransformUtility.WorldToScreenPoint(
                null, transform.position);

        Vector2 markerPosition =
            RectTransformUtility.WorldToScreenPoint(
                null, playerMarker.position);

        float distance = Vector2.Distance(
            stickerPosition, markerPosition);

        if (distance <= detectionRadius)
        {
            DetectSticker();
        }
    }

    void DetectSticker()
    {
        selectedSticker = this;

        stickerDetectedPanel.SetActive(true);
        if (buttonSound != null)
{
    buttonSound.PlayDetectionMusic();
}
        Image myImage = GetComponent<Image>();

if (detectedStickerImage != null && myImage != null)
{
    detectedStickerImage.sprite = myImage.sprite;
}

        stickerDetectedText.text =
            stickerInfo.stickerName + " detected!";

        catchButton.SetActive(true);
        ignoreButton.SetActive(true);
    }

    public void IgnoreSticker()
    {
        if (selectedSticker == null)
        {
            return;
        }

        PetrStickerClick sticker = selectedSticker;

        sticker.catchButton.SetActive(false);
        sticker.ignoreButton.SetActive(false);
        sticker.stickerDetectedPanel.SetActive(false);

        selectedSticker = null;

        Destroy(sticker.gameObject);
    }

    public static PetrStickerClick GetSelectedSticker()
    {
        return selectedSticker;
    }

    public void CatchSuccessfully()
    {
        catchButton.SetActive(false);
        ignoreButton.SetActive(false);
        stickerDetectedPanel.SetActive(false);

        selectedSticker = null;

        Destroy(gameObject);
    }

    public void HideActionButtons()
    {
        catchButton.SetActive(false);
        ignoreButton.SetActive(false);
    }
}
