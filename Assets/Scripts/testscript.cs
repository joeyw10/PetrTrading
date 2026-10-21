using UnityEngine;
using UnityEngine.SceneManagement;

public class testscript : MonoBehaviour
{
    [SerializeField] GameObject Circle;

    public void popup() {
        Circle.SetActive(true);
    }

}
