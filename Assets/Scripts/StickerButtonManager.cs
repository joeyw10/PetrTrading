
using UnityEngine;

public class StickerButtonManager : MonoBehaviour
{
    public GameObject catchPanel;
    public GameObject stickerDetectedPanel;
    public GameObject catchResultPanel;

    public void OpenCatchPanel()
    {
        catchPanel.SetActive(true);
        stickerDetectedPanel.SetActive(false);
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
