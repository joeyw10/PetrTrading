
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PetrStickerClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject catchButton;
    public GameObject ignoreButton;
    public GameObject stickerDetectedPanel;
    public TMP_Text stickerDetectedText;

    public enum StickerType
    {
        Common,
        Rare,
        Legendary
    }

    public StickerType stickerType;
    public StickerInfo stickerInfo;

    private static PetrStickerClick selectedSticker;

    // Do not hide shared buttons in Start().
    // Another sticker spawning should not affect the open panel.

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedSticker = this;

        stickerDetectedPanel.SetActive(true);

        stickerDetectedText.text =
            stickerInfo.stickerName +
            " detected! What should we do?";

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
