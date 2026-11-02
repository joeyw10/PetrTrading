using UnityEngine;

public class StickerPack : MonoBehaviour
{
    [SerializeField] private HomeController home;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator packAnimator;
    [SerializeField] private StickerRandomizer randomizeStickers;
    [SerializeField] private GameObject stash;

    StickerInfo sticker1;
    StickerInfo sticker2;
    StickerInfo sticker3;
    StickerInfo sticker4;
    private bool packOpened = false;

    GameObject stickerPrefab1;
    GameObject stickerPrefab2;
    GameObject stickerPrefab3;
    GameObject stickerPrefab4;



    void Start() {
        stash.SetActive(false);
    }
    void Update() {
        if(packOpened) {
            packAnimator.Play("PackOpen");
        }
    }

    private void OnMouseDown()
    {
        if(packOpened == false) {
        if (!CanOpen()) return; // missing setup: log an error and keep the pack unopened
        ChooseStickers();
        Debug.Log("Mouse down detected");
        packOpened = true;
        }
    }

    // A pack made from a prefab can't hold a link to a scene object, so look the randomizer up if it's empty
    private bool CanOpen()
    {
        if (randomizeStickers == null)
            randomizeStickers = FindFirstObjectByType<StickerRandomizer>();

        if (randomizeStickers == null)
        {
            Debug.LogError("StickerPack: no StickerRandomizer is assigned or found in the scene, so the pack can't be opened.");
            return false;
        }

        if (inventory == null)
        {
            Debug.LogError("StickerPack: the Inventory slot is empty.");
            return false;
        }

        return true;
    }

    public void ChooseStickers()
    {
        sticker1 = randomizeStickers.GetRandomSticker();
        inventory.addSticker(sticker1);

        sticker2 = randomizeStickers.GetRandomSticker();
        inventory.addSticker(sticker2);

        sticker3 = randomizeStickers.GetRandomSticker();
        inventory.addSticker(sticker3);

        sticker4 = randomizeStickers.GetRandomSticker();
        inventory.addSticker(sticker4);

        // Use up the pack only once the stickers were actually added
        inventory.setStickerPacks(-1);

        DisplayStickers();
    }

    private Vector3 spawnPosition1 = new Vector3(-0.6f, 0.9f, 5);
    private Vector3 spawnPosition2 = new Vector3(0.4f, 0.9f, 4);
    private Vector3 spawnPosition3 = new Vector3(-0.6f, -1.2f, 3);
    private Vector3 spawnPosition4 = new Vector3(0.4f, -1.2f, 2);

    public void DisplayStickers() {
        stickerPrefab1 = Instantiate(sticker1.stickerPrefab, spawnPosition1, transform.rotation,transform);
        stickerPrefab2 = Instantiate(sticker2.stickerPrefab, spawnPosition2, transform.rotation,transform);
        stickerPrefab3 = Instantiate(sticker3.stickerPrefab, spawnPosition3, transform.rotation,transform);
        stickerPrefab4 = Instantiate(sticker4.stickerPrefab, spawnPosition4, transform.rotation,transform);

        stash.SetActive(true);
    }

    // this didn't work so im giving up
    // public void SwapStickers() {
    //     if(stickerPrefab1.transform.position.z == 5) {
    //         stickerPrefab1.transform.position += new Vector3(0, 0, -3);
    //         stickerPrefab4.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab3.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab2.transform.position += new Vector3(0, 0, 1);
    //     } else if(stickerPrefab2.transform.position.z == 5) {
    //         stickerPrefab2.transform.position += new Vector3(0, 0, -3);
    //         stickerPrefab1.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab4.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab3.transform.position += new Vector3(0, 0, 1);
    //     } else if(stickerPrefab3.transform.position.z == 5) {
    //         stickerPrefab3.transform.position += new Vector3(0, 0, -3);
    //         stickerPrefab2.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab1.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab4.transform.position += new Vector3(0, 0, 1);
    //     } else if(stickerPrefab4.transform.position.z == 5) {
    //         stickerPrefab4.transform.position += new Vector3(0, 0, -3);
    //         stickerPrefab3.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab2.transform.position += new Vector3(0, 0, 1);
    //         stickerPrefab1.transform.position += new Vector3(0, 0, 1);
    //     } else {
    //         Debug.Log("Broken.");
    //     }
    // }
}