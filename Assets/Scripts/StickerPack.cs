using UnityEngine;

public class StickerPack : MonoBehaviour
{
    StickerInfo sticker1;
    StickerInfo sticker2;
    StickerInfo sticker3;
    StickerInfo sticker4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public StickerRandomizer randomizeStickers;

    public void ChooseStickers()
    {
        sticker1 = randomizeStickers.GetRandomSticker();
        sticker2 = randomizeStickers.GetRandomSticker();
        sticker3 = randomizeStickers.GetRandomSticker();
        sticker4 = randomizeStickers.GetRandomSticker();
    }

    public void RevealStickers() {

    }
}
