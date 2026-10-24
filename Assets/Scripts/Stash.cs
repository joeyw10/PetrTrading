using UnityEngine;

public class Stash : MonoBehaviour
{
    private
    //[SerializeField] private GameObject stickerPack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnMouseDown() {
        Destroy(transform.parent.gameObject);
        Debug.Log("Mouse down detected on stash");
    }
}
