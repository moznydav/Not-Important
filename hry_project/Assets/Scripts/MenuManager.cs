using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject settings;
    public GameObject mainMenu;

    public void Play()
    {
        SceneManager.LoadScene("Sandbox", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }
}
