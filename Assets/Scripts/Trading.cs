using UnityEngine;
using UnityEngine.SceneManagement;

public class Trading : MonoBehaviour
{
    public GameObject firstPopup;
    public GameObject secondPopup;
    public GameObject sticker;
    public GameObject sticker2;

    public GameObject cancel;
    public GameObject confirm;
    public GameObject checkmark;

    public void OpenPopupOne()
    {
        firstPopup.SetActive(true);
    }

    public void ClosePopupOne()
    {
        firstPopup.SetActive(false);
    }

    public void OpenPopupTwo()
    {
        secondPopup.SetActive(true);
        firstPopup.SetActive(false);
    }

    public void ClosePopupTwo()
    {
        secondPopup.SetActive(false);
    }

    public void AddSticker()
    {
        firstPopup.SetActive(true);
        sticker.SetActive(true);
    }

    public void SecondSticker()
    {
        sticker2.SetActive(true);
    }

    public void DisplaySecondSticker()

    {
        Invoke(nameof(SecondSticker), 3f);
    }

    public void Cancel()
    {
        cancel.SetActive(true);
    }

    public void Checkmark()
    {
        checkmark.SetActive(true);
    }

    public void Confirm()
    {
        confirm.SetActive(true);
        checkmark.SetActive(false);
    }

    public void Back()
    {
        checkmark.SetActive(false);
    }

    public void Home(string name)
    {
        SceneManager.LoadScene(name);
    }


}
