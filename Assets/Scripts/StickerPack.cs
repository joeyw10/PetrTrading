using UnityEngine;

public class StickerPack : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator packAnimator;
    [SerializeField] private StickerRandomizer randomizeStickers;
    [SerializeField] private GameObject stash;

    StickerInfo sticker1;
    StickerInfo sticker2;
    StickerInfo sticker3;
    StickerInfo sticker4;
    private bool packOpened = false;

    

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
        ChooseStickers();
        Debug.Log("Mouse down detected");
        packOpened = true;
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

        DisplayStickers();
    }

    public void DisplayStickers() {
        Instantiate(sticker1.stickerPrefab, transform.position, transform.rotation,transform);
        Instantiate(sticker2.stickerPrefab, transform.position, transform.rotation,transform);
        Instantiate(sticker2.stickerPrefab, transform.position, transform.rotation,transform);
        Instantiate(sticker2.stickerPrefab, transform.position, transform.rotation,transform);

        stash.SetActive(true);
    }
}
