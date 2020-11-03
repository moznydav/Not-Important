using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused = false;
    public GameObject pauseMenu;

    public void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
    }

    public void Resume()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void ToMainMenu()
    {
        Resume();
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Start()
    {
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isGamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
}
