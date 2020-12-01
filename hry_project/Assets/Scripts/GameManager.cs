using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused = false;
    [SerializeField] float waveInterval;
    [SerializeField] int enemyCountMultiplier = 3;
    [SerializeField] EnemySpawner[] enemySpawners;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject upgradeMenu;
    [SerializeField] List<GameObject> listOfUpgrades;
    [SerializeField] GameObject upgradeChest;
    [SerializeField] int waveToLevelRatio = 2;
    public bool canUpgrade;

    GameObject spawnedChest;
    LevelManager levelManager;

    private int activeEnemyTypes = 1;
    private int currentWaveNumber;
    [SerializeField] public int currentEnemyCount = 0;
    private int waveNumber = 0;
    GameObject[] chosenTypes;


    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        Resume();
        ScheduleWaveStart();
    }

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

    public void WaveEnded()
    {
        // TODO: Show upgrade menu
        print("Wave " + waveNumber + " ended");
        SpawnChest();
    }

    public void EnemyKilled()
    {
        currentEnemyCount -= 1;
        if (currentEnemyCount == 0) {
            WaveEnded();
        }
    }

    void WaveStart()
    {
        waveNumber++;
        if (waveNumber % waveToLevelRatio == 0) {
            levelManager.SetupNextLevel();
            if (activeEnemyTypes < enemyTypes.Length) {
                activeEnemyTypes++;
            }

        }
        print("Wave " + waveNumber + " started");

        MakeNewChosenTypes(activeEnemyTypes);

        //waveNumber * enemyCountMultiplier

        foreach (EnemySpawner spawner in enemySpawners) {
            spawner.Spawn(chosenTypes, activeEnemyTypes);
        }
    }

    public void SetupNextSpawners(EnemySpawner[] newEnemySpawners) {

        enemySpawners = newEnemySpawners;
    }

    void MakeNewChosenTypes(int length) {
        Debug.Log("Active enemy types" + length);
        chosenTypes = new GameObject[length];
        for(int i = 0; i < length; i++) {
            chosenTypes[i] = enemyTypes[UnityEngine.Random.Range(0, length)];
        }
    }

    public void ScheduleWaveStart()
    {
        //print("Scheduling wave start"); 
        Invoke("WaveStart", waveInterval);

        //waveManager.WaveStart();
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
        //testing
        
        if (Input.GetButtonDown("LevelSwitch")) {
            levelManager.SetupNextLevel();
        }
        
    }

    public void SetCanUpgrade( bool canUpgrade)
    {
        this.canUpgrade = canUpgrade;
    }
}
