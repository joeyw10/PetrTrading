
using UnityEngine;
using UnityEngine.EventSystems;

public class PetrStickerClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject catchButton;
    public GameObject ignoreButton;

    public enum StickerType
    {
        Common,
        Rare,
        Legendary
    }

    public StickerType stickerType;

    private static PetrStickerClick selectedSticker;

    private void Start()
    {
        catchButton.SetActive(false);
        ignoreButton.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedSticker = this;
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
