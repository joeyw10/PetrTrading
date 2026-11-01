using UnityEngine;

public class Stash : MonoBehaviour
{
    [SerializeField] HomeController home;
    [SerializeField] Inventory inventory;
    //[SerializeField] private GameObject stickerPack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnMouseDown() {
        CheckStickerPacks();
    }

    private void CheckStickerPacks() {
        if(inventory.getStickerPacks() > 0) {
            home.AddStickerPack();
            home.SetFalse();
        } else {
            home.SetTrue();
            Debug.Log("No more packs - stash");
        }

        Destroy(transform.parent.gameObject);
        Debug.Log("Mouse down detected on stash");
    }
}
