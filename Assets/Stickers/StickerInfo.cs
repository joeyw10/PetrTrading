using UnityEngine;

[CreateAssetMenu(fileName = "StickerInfo", menuName = "Stickers/StickerInfo")]
public class StickerInfo : ScriptableObject
{
    public string stickerName;
    public Sprite stickerImage;
    public string description;
    public string rarity;

    // instantiate this when a sticker is selected
    public GameObject stickerPrefab;
}
