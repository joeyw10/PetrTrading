
using UnityEngine;

public class StickerButtonManager : MonoBehaviour
{
    public GameObject catchPanel;

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
}
