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
    [SerializeField] float enemyPowerMultiplier;
    [SerializeField] EnemySpawner[] enemySpawners;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] int[] enemyPowers;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject upgradeMenu;
    [SerializeField] public GameObject DeathScreen;
    [SerializeField] public GameObject environmentMenu;
    [SerializeField] List<GameObject> listOfUpgrades;
    [SerializeField] GameObject upgradeChest;
    [SerializeField] int waveToLevelRatio = 2;
    public bool canUpgrade;
    public bool debugMode = false;

    GameObject spawnedChest;
    LevelManager levelManager;

    private int activeEnemyTypes = 1;
    private int currentWaveNumber;
    [SerializeField] public int currentEnemyCount = 0;
    private int waveNumber = 0;
    GameObject[] chosenTypes;
    private GameObject player;


    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
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
        if (environmentMenu.activeInHierarchy)
        {
            environmentMenu.GetComponent<EnvironmentScreen>().Close();
            environmentMenu.SetActive(false);
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
            Debug.Log("NextLevel");
            levelManager.SetupNextLevel();
            if (activeEnemyTypes < enemyTypes.Length) {
                activeEnemyTypes++;
            }

        }
        print("Wave " + waveNumber + " started");

        int[] chosenPowers = MakeNewChosenTypes(activeEnemyTypes);
        float desiredTotalPower = waveNumber * enemyPowerMultiplier;
        int totalBatches = (int)Math.Round(desiredTotalPower / chosenPowers.Sum());
        print("total batches: " + totalBatches);

        int index = 0;
        int counter = 0;
        while (counter < totalBatches) {
            print("spawning...");
            StartCoroutine(enemySpawners[index].Spawn(chosenTypes)) ;
            index += 1;
            counter += 1;
            if(index > enemySpawners.Length - 1)
            {
                index = 0;
            }
        }
    }

    public void SetupNextSpawners(EnemySpawner[] newEnemySpawners) {

        enemySpawners = newEnemySpawners;
    }

    int[] MakeNewChosenTypes(int length) {
        Debug.Log("Active enemy types" + length);
        chosenTypes = new GameObject[length];
        int[] chosenPowers = new int[length];
        for(int i = 0; i < length; i++) {
            int index = UnityEngine.Random.Range(0, length);
            chosenTypes[i] = enemyTypes[index];
            chosenPowers[i] = enemyPowers[index];
        }
        return chosenPowers;
    }

    public void ScheduleWaveStart()
    {
        Invoke("WaveStart", waveInterval);
    }

    public void ActivateUpgradeMenu()
    {
        Cursor.visible = true;
        isGamePaused = true;
        upgradeMenu.SetActive(true);
        Time.timeScale = 0f;
        upgradeMenu.GetComponent<UpgradeScreen>().Init();

    }

    public void ActivateEnvironmentMenu() {
        if (upgradeMenu.activeInHierarchy) {
            upgradeMenu.GetComponent<UpgradeScreen>().Close();
            upgradeMenu.SetActive(false);
        }
        environmentMenu.SetActive(true);
        environmentMenu.GetComponent<EnvironmentScreen>().Init();
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

    public void InitDeathScreen()
    {
        DeathScreen.SetActive(true);
        Cursor.visible = true;
        //Time.timeScale = 0f;
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

        if (!player)
        {
            InitDeathScreen();
        }
    }

    public void SetCanUpgrade( bool canUpgrade)
    {
        this.canUpgrade = canUpgrade;
    }
}
