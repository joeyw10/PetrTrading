using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] StickerInfo beachPetr;
    [SerializeField] GameObject one;
    [SerializeField] GameObject two;
    [SerializeField] GameObject three;
    [SerializeField] GameObject four;
    [SerializeField] GameObject five;
    [SerializeField] GameObject six;
    [SerializeField] GameObject seven;
    [SerializeField] GameObject eight;
    [SerializeField] GameObject nine;
    [SerializeField] GameObject ten;
    [SerializeField] GameObject eleven;
    [SerializeField] GameObject twelve;
    [SerializeField] GameObject thirteen;
    [SerializeField] GameObject fourteen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        one.SetActive(true);
        Invoke(nameof(Two), 5.0f);
    }

    public void Home1() {
        SceneManager.LoadScene("Home 1");
    }

    public void Tutorial() {
        SceneManager.LoadScene("Tutorial 1");
    }

    public void addPetr() {
        inventory.addSticker(beachPetr);
    }

    // Update is called once per frame
    public void Two() {
        two.SetActive(true);
    }

    public void Three() {
        three.SetActive(true);
    }

    public void Four() {
        four.SetActive(true);
    }

    public void Five() {
        five.SetActive(true);
    }

    public void Six() {
        six.SetActive(true);
    }

    public void Seven() {
        seven.SetActive(true);
    }

    public void Eight() {
        eight.SetActive(true);
    }

    public void Nine() {
        nine.SetActive(true);
    }

    public void Ten() {
        ten.SetActive(true);
    }

    public void Eleven() {
        eleven.SetActive(true);
    }

    public void Twelve() {
        twelve.SetActive(true);
    }

    public void Thirteen() {
        thirteen.SetActive(true);
    }

    public void Fourteen() {
        fourteen.SetActive(true);
    }
}
