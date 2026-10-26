
using UnityEngine;
using UnityEngine.EventSystems;

public class PetrStickerClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject catchButton;
    public GameObject ignoreButton;
    public GameObject stickerDetectedPanel;
public TMPro.TMP_Text stickerDetectedText;

    public enum StickerType
    {
        Common,
        Rare,
        Legendary
    }

    public StickerType stickerType;

    public StickerInfo stickerInfo;

    private static PetrStickerClick selectedSticker;

    private void Start()
    {
        catchButton.SetActive(false);
        ignoreButton.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
{
    selectedSticker = this;

    stickerDetectedPanel.SetActive(true);

    stickerDetectedText.text =
        stickerInfo.stickerName + " detected! What should we do?";

    catchButton.SetActive(true);
    ignoreButton.SetActive(true);
}

    

    public void IgnoreSticker()
    {
        if (selectedSticker == null)
        {
            return;
        }

        selectedSticker.catchButton.SetActive(false);
        selectedSticker.ignoreButton.SetActive(false);
        selectedSticker.stickerDetectedPanel.SetActive(false);

        Destroy(selectedSticker.gameObject);
        selectedSticker = null;
    }

    public static PetrStickerClick GetSelectedSticker()
    {
        return selectedSticker;
    }

    public void CatchSuccessfully()
    {
        catchButton.SetActive(false);
        ignoreButton.SetActive(false);

        Destroy(gameObject);
        selectedSticker = null;
    }
    public void HideActionButtons()
{
    catchButton.SetActive(false);
    ignoreButton.SetActive(false);
}
}
