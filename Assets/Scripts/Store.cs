using UnityEngine;

public class Store : MonoBehaviour
{
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
        StoreModal.SetActive(false);
    }

    public void OpenStore() {
        StoreModal.SetActive(true);
    }

    [SerializeField] Inventory inventory;

    // buying packs
    public void Purchase1Pack() {
        inventory.setStickerPacks(1);
    }

    public void Purchase3Pack() {
        inventory.setStickerPacks(3);
    }

    public void Purchase5Pack() {
        inventory.setStickerPacks(5);
    }

    public void Purchase10Pack() {
        inventory.setStickerPacks(10);
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
