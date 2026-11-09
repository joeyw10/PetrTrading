
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trading : MonoBehaviour
{
    public Inventory inventory;
    public GameObject firstPopup;
    public GameObject stickerselection;
    public GameObject profile;
    public GameObject requestOne;
    public GameObject requestTwo;

    public GameObject playerOfferPrefab;
    public Transform secondSlot;

    public GameObject playerOfferOne;

    public GameObject option;
    public GameObject option2;
    public GameObject stickerOne;
    public GameObject stickerTwo;

    public GameObject cancel;
    public GameObject confirm;
    public GameObject checkmark;
    public GameObject selectedStickerImage;
    public GameObject stickerSelectionPanel;

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

    public void Confirm()
    {
        confirm.SetActive(true);
        checkmark.SetActive(false);
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

        Image image = selectedStickerImage.GetComponent<Image>();
        image.sprite = selectedSticker.stickerImage;
        selectedStickerImage.SetActive(true);
        stickerSelectionPanel.SetActive(false);
    }
}
