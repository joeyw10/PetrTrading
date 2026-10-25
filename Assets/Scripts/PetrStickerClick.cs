
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PetrStickerClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject catchButton;
    public GameObject ignoreButton;
    private static PetrStickerClick selectedSticker;
    private void Start()
{
    catchButton.SetActive(false);
    ignoreButton.SetActive(false);
}

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedSticker = this;
        Debug.Log("Petr sticker was clicked!");
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
}
