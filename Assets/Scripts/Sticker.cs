using UnityEngine;

public class Sticker : MonoBehaviour
{
    private StickerInfo sticker;
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
        sticker = randomizeStickers.GetRandomSticker();
    }
}
