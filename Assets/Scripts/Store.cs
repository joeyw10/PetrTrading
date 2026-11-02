using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    [SerializeField] private HomeController home;
    [SerializeField] GameObject StickerPackObjects;
    [SerializeField] GameObject StoreModal;
    [SerializeField] GameObject PacksTab;
    [SerializeField] GameObject AntsTab;

    void Start() {
        PacksTab.SetActive(true);
        AntsTab.SetActive(false);
    }

    public void OpenPackTab() {
        PacksTab.SetActive(true);
        AntsTab.SetActive(false);
    }

    public void OpenAntsTab() {
        PacksTab.SetActive(false);
        AntsTab.SetActive(true);
    }

    public void CloseStore() {
        StickerPackObjects.SetActive(true);
        StoreModal.SetActive(false);
    }

    public void OpenStore() {
        StickerPackObjects.SetActive(false);
        StoreModal.SetActive(true);
    }

    [SerializeField] Inventory inventory;
    [SerializeField] private TMP_Text numPacks;
    [SerializeField] private TMP_Text numCommon;
    [SerializeField] private TMP_Text numGolden;
    [SerializeField] private TMP_Text numDiamond;

    void Update()
    {
        numPacks.text = inventory.getStickerPacks().ToString() + " Packs Left";
        numCommon.text = inventory.getCommonAnts().ToString();
        numGolden.text = inventory.getGoldenAnts().ToString();
        numDiamond.text = inventory.getDiamondAnts().ToString();
    }

    // buying packs
    public void Purchase1Pack() {
        if(inventory.getStickerPacks() == 0) {
            home.AddStickerPack();
        }
        inventory.setStickerPacks(1);
        home.SetFalse();
    }

    public void Purchase3Pack() {
        if(inventory.getStickerPacks() == 0) {
            home.AddStickerPack();
        }
        inventory.setStickerPacks(3);
        home.SetFalse();
    }

    public void Purchase5Pack() {
        if(inventory.getStickerPacks() == 0) {
            home.AddStickerPack();
        }
        inventory.setStickerPacks(5);
        home.SetFalse();
    }

    public void Purchase10Pack() {
        if(inventory.getStickerPacks() == 0) {
            home.AddStickerPack();
        }
        inventory.setStickerPacks(10);
        home.SetFalse();
    }

    // buying ants

    public void PurchaseCommonAnts() {
        inventory.setCommonAnts(5);
    }

    public void PurchaseGoldenAnts() {
        inventory.setGoldenAnts(5);
    }

    public void PurchaseDiamondAnts() {
        inventory.setDiamondAnts(5);
    }
}
