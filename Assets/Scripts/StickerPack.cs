using UnityEngine;

public class StickerPack : MonoBehaviour
{
    StickerInfo sticker1;
    StickerInfo sticker2;
    StickerInfo sticker3;
    StickerInfo sticker4;
    private bool packOpened = false;

    [SerializeField] private Animator packAnimator;

    [SerializeField] private StickerRandomizer randomizeStickers;
    [SerializeField] private GameObject stash;

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
        sticker2 = randomizeStickers.GetRandomSticker();
        sticker3 = randomizeStickers.GetRandomSticker();
        sticker4 = randomizeStickers.GetRandomSticker();

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
