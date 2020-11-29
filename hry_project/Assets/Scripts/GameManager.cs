using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused = false;
    [SerializeField] float waveInterval;
    [SerializeField] int enemyCountMultiplier = 3;
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] List<GameObject> enemyTypes;
    [SerializeField] int enemyTypeInterval = 3;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject upgradeMenu;
    [SerializeField] List<GameObject> listOfUpgrades;
    [SerializeField] GameObject upgradeChest;
    public bool canUpgrade;
    GameObject spawnedChest;

    private int activeEnemyTypes = 1;
    private int currentWaveNumber;
    private int currentEnemyCount = 0;
    private int waveNumber = 0;

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

    void WaveEnded()
    {
        // TODO: Show upgrade menu
        print("Wave " + waveNumber + " ended");
        SpawnChest();
        ScheduleWaveStart();
    }

    public void EnemyKilled()
    {
        currentEnemyCount -= 1;
        print("Current enemy count: " + currentEnemyCount);
        if (currentEnemyCount == 0) {
            WaveEnded();
        }
    }

    void WaveStart()
    {
        waveNumber += 1;
        print("Wave " + waveNumber + " started");
        if (waveNumber % enemyTypeInterval == 0)
            //if(activeEnemyTypes + 1 < enemyTypes.Capacity)
            //{
                activeEnemyTypes += 1;
           // }
        List<GameObject> availableTypes = enemyTypes.GetRange(0, activeEnemyTypes);
        int typesToChoose = (int)Math.Ceiling((double)availableTypes.Count / 2);
        // TODO: randomly choose enemy types
        // and distribute them evenly across all spawners
        List<GameObject> chosenTypes = availableTypes.GetRange(0, typesToChoose);
        currentEnemyCount = chosenTypes.Count * waveNumber * enemyCountMultiplier * enemySpawners.Count;
        print("spawning... current enemy count: " + currentEnemyCount);
        foreach (EnemySpawner spawner in enemySpawners) {
            spawner.Spawn(chosenTypes, waveNumber * enemyCountMultiplier);
        }
    }

    void ScheduleWaveStart()
    {
        print("Scheduling wave start");
        Invoke("WaveStart", waveInterval);
    }

    void Start()
    {
        Resume();
        ScheduleWaveStart();
    }

    public void ActivateUpgradeMenu()
    {
        Cursor.visible = true;
        isGamePaused = true;
        upgradeMenu.SetActive(true);
        Time.timeScale = 0f;
        upgradeMenu.GetComponent<UpgradeScreen>().Init();

    }

    public void SpawnChest()
    {
        if (spawnedChest)
        {
            Destroy(spawnedChest);
        }
        Vector3 chestPosition = GameObject.FindWithTag(Constants.PLAYER_TAG).transform.position;
        chestPosition.x += 5;
        spawnedChest = Instantiate(upgradeChest, chestPosition, Quaternion.identity);
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
                    Destroy(spawnedChest); // start destroy animation
                }
                
            }
        }
        // rework 
        if (Input.GetButtonDown("Fire3"))
        {
            SpawnChest();
        }
    }

    public void SetCanUpgrade( bool canUpgrade)
    {
        this.canUpgrade = canUpgrade;
    }
}
