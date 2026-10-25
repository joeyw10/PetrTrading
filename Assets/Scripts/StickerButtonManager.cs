
using UnityEngine;

public class StickerButtonManager : MonoBehaviour
{
    public GameObject catchPanel;
    public GameObject catchResultPanel;

    public void OpenCatchPanel()
    {
        catchPanel.SetActive(true);
    }

    public void IgnoreSelectedSticker()
    {
        PetrStickerClick selected =
            FindFirstObjectByType<PetrStickerClick>();

        if (selected != null)
        {
            selected.IgnoreSticker();
        }
    }
    public void BackToMap()
{
    catchResultPanel.SetActive(false);
}
}
