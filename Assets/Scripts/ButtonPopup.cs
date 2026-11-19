using UnityEngine;

public class ButtonPopup : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OpenPanel() {
        infoPanel.SetActive(true);
    }

    public void ClosePanel() {
        infoPanel.SetActive(false);
    }
}
