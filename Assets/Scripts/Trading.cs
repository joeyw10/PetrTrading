using UnityEngine;
using UnityEngine.SceneManagement;

public class Trading : MonoBehaviour
{
    public Inventory inventory;
    public GameObject firstPopup;
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

    public void OpenPopupOne()
    {
        firstPopup.SetActive(true);
        
    }

    public void BotRequestOne()

    {
        //botOffer.SetActive(true);
       stickerOfferOne = Instantiate(botOfferPrefab, targetSlot);

    
        requestOne.SetActive(true);
        profile.SetActive(true);
        option.SetActive(true);
        


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
