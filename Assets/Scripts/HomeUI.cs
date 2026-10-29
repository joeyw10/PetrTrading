using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TMP_Text numPacks;
    [SerializeField] private TMP_Text numCommon;
    [SerializeField] private TMP_Text numGolden;
    [SerializeField] private TMP_Text numDiamond;

    void Update()
    {
        numPacks.text = inventory.getStickerPacks().ToString() + " Packs Left";
        numCommon.text = inventory.getCommonAnts().ToString();
        numGolden.text = inventory.getGoldenAnts().ToString();
        numDiamond.text = inventory.getDiamondAnts().ToString();
    }
}
