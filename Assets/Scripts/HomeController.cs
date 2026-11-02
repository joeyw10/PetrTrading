using UnityEngine;

public class HomeController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject stickerPackPrefab;
    [SerializeField] private GameObject getpacks;
    [SerializeField] private GameObject storebutton;

    private GameObject currentPack;
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(inventory.getStickerPacks() == 0) {
            getpacks.SetActive(true);
            storebutton.SetActive(true);
            Debug.Log("Start - There are no sticker packs.");
        } else if(inventory.getStickerPacks() > 0) {
            getpacks.SetActive(false);
            storebutton.SetActive(false);
            Instantiate(stickerPackPrefab, transform.position, transform.rotation, transform);
            Debug.Log("Start - There are sticker packs, so added one.");
            
            // for(int i = 0; i < inventory.getStickerPacks(); i++) {
            //     currentPack = Instantiate(stickerPackPrefab, transform.position, transform.rotation, transform);
            //     sr = currentPack.GetComponent<SpriteRenderer>();
            //     sr.sortingOrder = i + 1;
            // }
        }
    }

    // void Update() {
    //     if(inventory.getStickerPacks() > 0) {
    //         SetFalse();
    //     } else {
    //         SetTrue();
    //     }
    // }

    public void AddStickerPack() {
        Instantiate(stickerPackPrefab, transform.position, transform.rotation, transform);
    //     for(int i = inventory.getStickerPacks(); i < inventory.getStickerPacks() + num; i++) {
    //         currentPack = Instantiate(stickerPackPrefab, transform.position, transform.rotation, transform);
    //         sr = currentPack.GetComponent<SpriteRenderer>();
    //         sr.sortingOrder = i + 1;
    //     }
        Debug.Log("Added a Sticker Pack.");
    }

    public void SetFalse() {
        getpacks.SetActive(false);
        storebutton.SetActive(false);
    }

    public void SetTrue() {
        getpacks.SetActive(true);
        storebutton.SetActive(true);
    }
}
