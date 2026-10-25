using UnityEngine;

[CreateAssetMenu(fileName = "StickerInfo", menuName = "Stickers/StickerInfo")]
public class StickerInfo : ScriptableObject
{
    public Sprite stickerImage;
    public string rarity;
    public string stickerName;
    public string description;

    // instantiate this when a sticker is selected
    public GameObject stickerPrefab;
}
