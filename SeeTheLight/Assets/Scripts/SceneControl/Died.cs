using UnityEngine;
using UnityEngine.SceneManagement;

public class Died : MonoBehaviour
{

    // Update is called once per frame
    public void retry_click()
    {
        SceneManager.LoadScene(1);
    }
    public void menu_click()
    {
        SceneManager.LoadScene(0);
    }
}
