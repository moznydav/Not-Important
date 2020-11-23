using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused = false;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject upgradeMenu;
    [SerializeField] List<GameObject> listOfUpgrades;
    public bool canUpgrade;
    public void Pause()
    {
        canUpgrade = false;
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
        if (upgradeMenu.activeInHierarchy)
        {
            upgradeMenu.GetComponent<UpgradeScreen>().Close();
            upgradeMenu.SetActive(false);
        }
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

    public void ActivateUpgradeMenu()
    {
        Cursor.visible = true;
        isGamePaused = true;
        upgradeMenu.SetActive(true);
        Time.timeScale = 0f;
        upgradeMenu.GetComponent<UpgradeScreen>().Init();

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

        if (canUpgrade) {
            if (Input.GetButtonDown("Upgrade"))
            {
                if (!isGamePaused)
                {
                    ActivateUpgradeMenu();
                }
            }
        }
        
    }

    public void SetCanUpgrade( bool canUpgrade)
    {
        this.canUpgrade = canUpgrade;
    }
}
