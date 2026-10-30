using System;
using UnityEngine;

// Remembers WHEN each sticker was first added to the player's inventory.
// Saved with PlayerPrefs, keyed by the StickerInfo asset's name (so names must be unique).
public static class StickerDates
{
    private static string Key(StickerInfo sticker)
    {
        return "added_" + sticker.name;
    }

    // Call when a sticker enters the inventory. Only the FIRST time counts,
    // so getting a duplicate later doesn't change its date.
    public static void RecordAdded(StickerInfo sticker)
    {
        if (sticker == null) return;
        if (PlayerPrefs.HasKey(Key(sticker))) return;

        PlayerPrefs.SetString(Key(sticker), DateTime.UtcNow.Ticks.ToString());
        PlayerPrefs.Save();
    }

    // Returns the saved time (bigger = newer). 0 means no date was ever recorded.
    public static long GetAddedTicks(StickerInfo sticker)
    {
        if (sticker == null) return 0;

        string saved = PlayerPrefs.GetString(Key(sticker), "");
        long ticks;
        return long.TryParse(saved, out ticks) ? ticks : 0;
    }
}
