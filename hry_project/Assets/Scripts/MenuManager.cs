using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject settings;
    public GameObject mainMenu;
    public Toggle fullscreenToggle;
    public Dropdown resDropdown;

    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        resDropdown.ClearOptions();
        List<string> resOptions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            Resolution currRes = Screen.resolutions[i];
            string resOption = currRes.width + " x " + currRes.height;
            resOptions.Add(resOption);
        }
        resDropdown.AddOptions(resOptions);
    }

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

    public void SetFullscreen(bool isFullscreen)
    {
        print(isFullscreen);
        Screen.fullScreen = isFullscreen;
    }
}
