using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    // opens the inventory screen
    public void Inventory() {
        SceneManager.LoadScene("Inventory");
    }

    // opens the home/opens packs screen
    public void Home() {
        SceneManager.LoadScene("Home");
    }

    public void Map() {
        SceneManager.LoadScene("Map");
    }

    public void Trade() {
        SceneManager.LoadScene("Trading");
    }

    // public void Trading() {
    //     SceneManager.LoadScene("Trading");
    // }
}
