using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    // SET TO PUBLIC SO WE CAN SEE IT IN PROPERTIES, BUT DO NOT ADD STICKERS MANUALLY UNLESS IT
    // IS FOR THE PROTOTYPE
    public List<StickerInfo> myStickers;

    [Header("Saving / random rewards")]
    [Tooltip("The list of every sticker. Needed to load saved stickers and to pick random ones for packs.")]
    public StickerDatabase database;

    [Header("Pack odds (relative weights, not percentages)")]
    public int commonWeight = 70;
    public int rareWeight = 25;
    public int legendaryWeight = 5;

    [Header("Daily free packs")]
    [Tooltip("Each new day the player is topped up to this many packs (never goes down, never stacks).")]
    public int dailyFreePacks = 3;

    private int commonAnts;
    private int goldenAnts;
    private int diamondAnts;

    private int stickerPacks;
    private int packsOpenedToday;

    private const string StickersKey = "inventory_stickers";
    private const string CommonAntsKey = "inventory_commonAnts";
    private const string GoldenAntsKey = "inventory_goldenAnts";
    private const string DiamondAntsKey = "inventory_diamondAnts";
    private const string PacksKey = "inventory_stickerPacks";
    private const string OpenedTodayKey = "inventory_packsOpenedToday";
    private const string DailyDateKey = "inventory_dailyDate";

    private static string Today()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd");
    }

    // Runs when the game starts: bring back everything that was saved
    private void OnEnable()
    {
        commonAnts = PlayerPrefs.GetInt(CommonAntsKey, 0);
        goldenAnts = PlayerPrefs.GetInt(GoldenAntsKey, 0);
        diamondAnts = PlayerPrefs.GetInt(DiamondAntsKey, 0);
        stickerPacks = PlayerPrefs.GetInt(PacksKey, 0);

        LoadStickersFromSave();
        RefreshDaily();
    }

    // ---------- Daily free packs ----------
    // On the first run of a new day (or the very first launch), top the player up to dailyFreePacks.
    // Called whenever packs are read, so it also works if the game was left open past midnight.
    private void RefreshDaily()
    {
        packsOpenedToday = PlayerPrefs.GetInt(OpenedTodayKey, 0);

        if (PlayerPrefs.GetString(DailyDateKey, "") == Today()) return;

        // Top up, don't stack: leftover free packs aren't added on top, and packs the player
        // bought (more than the daily amount) are left alone.
        if (stickerPacks < dailyFreePacks)
        {
            stickerPacks = dailyFreePacks;
            SaveCount(PacksKey, stickerPacks);
        }

        packsOpenedToday = 0;
        SaveCount(OpenedTodayKey, 0);

        PlayerPrefs.SetString(DailyDateKey, Today());
        PlayerPrefs.Save();
    }

    // How many packs have been opened since the day started (used to grey out packs in the Sticker packs tab)
    public int getPacksOpenedToday()
    {
        RefreshDaily();
        return packsOpenedToday;
    }

    // For testing: gives you a fresh day's worth of packs right now
    public void ResetDailyPacksForTesting()
    {
        stickerPacks = dailyFreePacks;
        packsOpenedToday = 0;
        SaveCount(PacksKey, stickerPacks);
        SaveCount(OpenedTodayKey, 0);
        PlayerPrefs.SetString(DailyDateKey, Today());
        PlayerPrefs.Save();
    }

    public void addSticker(StickerInfo sticker) {
        if (sticker == null) return;

        myStickers.Add(sticker);

        // Remember the date this sticker was first added (used by the "Newest" sort)
        StickerDates.RecordAdded(sticker);

        SaveStickers();
    }

    // Picks a random sticker using the pack odds above, adds it to the inventory, and returns it
    // so the pack-opening screen can show what was won. Returns null if there's no database.
    public StickerInfo AddRandomSticker() {
        if (database == null) {
            Debug.LogWarning("Inventory has no Sticker Database assigned, can't pick a random sticker!");
            return null;
        }

        int total = Mathf.Max(0, commonWeight) + Mathf.Max(0, rareWeight) + Mathf.Max(0, legendaryWeight);

        StickerInfo won = null;
        if (total > 0) {
            int roll = Random.Range(0, total);
            string rarity;
            if (roll < commonWeight) rarity = "Common";
            else if (roll < commonWeight + rareWeight) rarity = "Rare";
            else rarity = "Legendary";

            won = database.GetRandomOfRarity(rarity);
        }

        // If nothing of that rarity exists (or odds are all 0), fall back to any sticker
        if (won == null) won = database.GetRandom();

        if (won != null) addSticker(won);
        return won;
    }

    // edit needed probably
    public void removeSticker(StickerInfo sticker) {
        myStickers.Remove(sticker);
        SaveStickers();
    }

    // ---------- Saving (sticker asset NAMES and the counts are stored on the device) ----------
    private void SaveStickers()
    {
        List<string> names = new List<string>();
        foreach (StickerInfo sticker in myStickers)
        {
            if (sticker != null) names.Add(sticker.name);
        }

        PlayerPrefs.SetString(StickersKey, string.Join("|", names));
        PlayerPrefs.Save();
    }

    private void LoadStickersFromSave()
    {
        // Nothing saved yet (or no database): leave the list alone, so prototype stickers still work
        if (database == null || !PlayerPrefs.HasKey(StickersKey)) return;

        string saved = PlayerPrefs.GetString(StickersKey, "");
        myStickers = new List<StickerInfo>();

        if (saved.Length == 0) return;

        foreach (string stickerName in saved.Split('|'))
        {
            StickerInfo sticker = database.GetByName(stickerName);
            if (sticker != null) myStickers.Add(sticker);
        }
    }

    private void SaveCount(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    // For testing: right-click the Inventory asset's header in the Inspector.
    // Wipes everything. The next time packs are read, the daily free packs are handed out again.
    [ContextMenu("Clear Saved Inventory")]
    private void ClearSavedInventory()
    {
        PlayerPrefs.DeleteKey(StickersKey);
        PlayerPrefs.DeleteKey(CommonAntsKey);
        PlayerPrefs.DeleteKey(GoldenAntsKey);
        PlayerPrefs.DeleteKey(DiamondAntsKey);
        PlayerPrefs.DeleteKey(PacksKey);
        PlayerPrefs.DeleteKey(OpenedTodayKey);
        PlayerPrefs.DeleteKey(DailyDateKey);
        PlayerPrefs.Save();

        myStickers = new List<StickerInfo>();
        commonAnts = 0;
        goldenAnts = 0;
        diamondAnts = 0;
        stickerPacks = 0;
        packsOpenedToday = 0;
    }

    public int getCommonAnts() {
        return commonAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setCommonAnts(int antModifier) {
        commonAnts = commonAnts + antModifier;
        SaveCount(CommonAntsKey, commonAnts);
    }

    public int getGoldenAnts() {
        return goldenAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setGoldenAnts(int antModifier) {
        goldenAnts = goldenAnts + antModifier;
        SaveCount(GoldenAntsKey, goldenAnts);
    }

    public int getDiamondAnts() {
        return diamondAnts;
    }

    // when using this, positive number adds while negative subtracts
    public void setDiamondAnts(int antModifier) {
        diamondAnts = diamondAnts + antModifier;
        SaveCount(DiamondAntsKey, diamondAnts);
    }

    public int getStickerPacks() {
        RefreshDaily();
        return stickerPacks;
    }

    // when using this, positive number adds while negative subtracts
    public void setStickerPacks(int packsModifier) {
        RefreshDaily();

        int before = stickerPacks;
        stickerPacks = stickerPacks + packsModifier;

        if(stickerPacks < 0) {
            stickerPacks = 0;
        }

        // Packs going down means the player opened some: remember that for today
        if (stickerPacks < before)
        {
            packsOpenedToday += before - stickerPacks;
            SaveCount(OpenedTodayKey, packsOpenedToday);
        }

        SaveCount(PacksKey, stickerPacks);
    }

    public List<StickerInfo> GetThreeRandomStickers()
    {
    List<StickerInfo> available = new List<StickerInfo>();
    List<StickerInfo> selected = new List<StickerInfo>();

    if (database == null || database.allStickers == null)
        {
        Debug.LogWarning("Sticker Database is missing!");
        return selected;
        }

    // Copy all valid stickers from the database
    foreach (StickerInfo sticker in database.allStickers)
        {
        if (sticker != null)
        {
            available.Add(sticker);
        }
        }

    // Pick up to 3 unique stickers
    int amount = Mathf.Min(3, available.Count);

    for (int i = 0; i < amount; i++)
        {
        int randomIndex = Random.Range(0, available.Count);

        selected.Add(available[randomIndex]);

        // Prevent duplicate selections
        available.RemoveAt(randomIndex);
        }

    return selected;
    }

}