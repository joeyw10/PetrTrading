using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of EVERY sticker in the game. Used to (a) look a sticker up by name when loading a save,
/// and (b) pick random stickers for pack openings and catching.
/// Create it via: right-click in Project -> Create -> Scriptable Objects -> Sticker Database.
/// </summary>
[CreateAssetMenu(fileName = "StickerDatabase", menuName = "Scriptable Objects/Sticker Database")]
public class StickerDatabase : ScriptableObject
{
    public List<StickerInfo> allStickers;

    // Finds a sticker by its ASSET name (the file name in the Project window)
   
public StickerInfo GetByName(string assetName)
{
    foreach (StickerInfo sticker in allStickers)
    {
        if (sticker != null &&
            (sticker.name == assetName ||
             sticker.stickerName == assetName))
        {
            return sticker;
        }
    }

    return null;
}

public StickerInfo GetByPrefab(GameObject prefab)
{
    if (prefab == null || allStickers == null)
        return null;

    foreach (StickerInfo sticker in allStickers)
    {
        if (sticker != null && sticker.stickerPrefab == prefab)
        {
            return sticker;
        }
    }

    return null;
}



    // Any random sticker
    public StickerInfo GetRandom()
    {
        if (allStickers == null || allStickers.Count == 0) return null;
        return allStickers[Random.Range(0, allStickers.Count)];
    }

    // A random sticker of one rarity, e.g. GetRandomOfRarity("Rare"). Returns null if none match.
    public StickerInfo GetRandomOfRarity(string rarityName)
    {
        List<StickerInfo> matches = new List<StickerInfo>();
        foreach (StickerInfo sticker in allStickers)
        {
            if (sticker != null &&
                sticker.rarity.ToString().ToLower() == rarityName.ToLower().Trim())
                matches.Add(sticker);
        }

        if (matches.Count == 0) return null;
        return matches[Random.Range(0, matches.Count)];
    }
}
