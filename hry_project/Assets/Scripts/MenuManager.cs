using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    public void Play()
    {
        SceneManager.LoadScene("Sandbox", LoadSceneMode.Single);
        print("Play game scene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
