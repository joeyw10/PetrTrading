using UnityEngine;

// Shared favorites storage, used by both the Inventory binder and the Sticker Info screen.
// Favorites are saved on the device with PlayerPrefs, keyed by the StickerInfo asset's name,
// so every StickerInfo asset needs a unique name.
public static class StickerFavorites
{
    private static string Key(StickerInfo sticker)
    {
        return "fav_" + sticker.name;
    }

    public static bool IsFavorited(StickerInfo sticker)
    {
        if (sticker == null) return false;
        return PlayerPrefs.GetInt(Key(sticker), 0) == 1;
    }

    public static void SetFavorited(StickerInfo sticker, bool value)
    {
        if (sticker == null) return;
        PlayerPrefs.SetInt(Key(sticker), value ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Flips the state and returns the new value
    public static bool Toggle(StickerInfo sticker)
    {
        bool newValue = !IsFavorited(sticker);
        SetFavorited(sticker, newValue);
        return newValue;
    }
}
