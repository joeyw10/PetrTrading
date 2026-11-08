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
    //public GameObjectbotOffer;

    public GameObject botOfferPrefab;
    public Transform targetSlot;

    public GameObject playerOfferPrefab;
    public Transform secondSlot;


    public GameObject stickerOfferOne;
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

public GameObject stickerslot;


    public void OpenPopupOne()
    {
        firstPopup.SetActive(true);
        requestbutton.SetActive(false);
        
    }

    public void BotRequestOne()

    {
        //botOffer.SetActive(true);
       stickerOfferOne = Instantiate(botOfferPrefab, targetSlot);

    
        requestOne.SetActive(true);
        profile.SetActive(true);
        
        //option.SetActive(true);
        


    }

    public void Openstickerselection()
    {
        stickerslot.SetActive(false);
        stickerselection.SetActive(true);
    }

    public void have()
    {
        // check if in inventory, if it is, show prefab
        
        //if (inventory.inInventory("NASAPetr") == true)
        //{
            option.SetActive(false);
            //stickerOne.SetActive(true);
            playerOfferOne = Instantiate(playerOfferPrefab, secondSlot);
            Invoke("Confirm", 2f);
            Invoke("stickerOneFalse", 2f);
            
            
            
        //}

    }

    public void stickerOneFalse()
    {
        stickerOne.SetActive(false);
        profile.SetActive(false);
        Destroy(stickerOfferOne);
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
        Destroy(stickerOfferOne);
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
        stickerslot.SetActive(false);
        requestOne.SetActive(false);
        requestTwo.SetActive(false);
        profile.SetActive(false);
    }

    public void Checkmark()
    {
        checkmark.SetActive(true);
        stickerslot.SetActive(false);
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
        stickerslot.SetActive(true);
        requestOne.SetActive(true);
        requestTwo.SetActive(true);
        profile.SetActive(true);
    }

    public void Home(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SelectSticker(StickerInfo selectedSticker)
{
    if (selectedSticker == null)
        return;

    // Display the selected sticker's image
    Image image = selectedStickerImage.GetComponent<Image>();
    image.sprite = selectedSticker.stickerImage;

    // Make the selected sticker visible
    selectedStickerImage.SetActive(true);

    // Close the sticker selection panel
    stickerSelectionPanel.SetActive(false);
    stickerslot.SetActive(true);
}


}
