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
    [SerializeField] float wavePauseTime;
    [SerializeField] float enemyPowerMultiplier;
    [SerializeField] EnemySpawner[] enemySpawners;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] int[] enemyPowers;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject upgradeMenu;
    [SerializeField] public GameObject DeathScreen;
    [SerializeField] public GameObject environmentMenu;
    [SerializeField] public NextWaveIn nextWaveInObject;
    [SerializeField] List<GameObject> listOfUpgrades;
    [SerializeField] GameObject upgradeChest;
    [SerializeField] int waveToLevelRatio = 2;
    [SerializeField] int waveToEnvironmentRatio = 4;
    [SerializeField] float timeBetweenWaves = 15;
    public bool canUpgrade;
    public bool debugMode = false;

    GameObject spawnedChest;
    LevelManager levelManager;

    private int activeEnemyTypes = 1;
    private float nextWaveIn;
     private int currentWaveNumber;
    [SerializeField] public int currentEnemyCount = 0;
    [SerializeField] private int waveNumber = 0;
    GameObject[] chosenTypes;
    private GameObject player;
    private int chestsPickedUp;
    private int chestsSpawned;
    private bool lastWave;


    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
        Resume();
        ScheduleWaveStart();
        nextWaveIn = timeBetweenWaves;
        lastWave = false;
        chestsPickedUp = 0;
        chestsSpawned = 0;
        waveNumber = 1;
    }

    public void Pause()
    {
        canUpgrade = false;
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
        Cursor.lockState = CursorLockMode.Confined;
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
        print("Wave " + waveNumber + " ended");
        SpawnChest();
        waveNumber++;
        if (waveNumber % waveToLevelRatio == 0)
        {
            Debug.Log("NextLevel");
            levelManager.SetupNextLevel();
            if (activeEnemyTypes < enemyTypes.Length)
            {
                activeEnemyTypes++;
            }

        }

       
        if (!lastWave)
        {
            ScheduleWaveStart();
        }
        
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
        nextWaveIn = timeBetweenWaves;
        
        if (waveNumber % waveToLevelRatio == waveToLevelRatio -1)
        {
            lastWave = true;
            nextWaveInObject.SetLastWave();
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
        if(lastWave)
        {
            if(chestsPickedUp == 2)
            {
                Invoke("WaveStart", wavePauseTime);
                chestsPickedUp = 0;
            }
            
        }
        else
        {
            Invoke("WaveStart", wavePauseTime);
        }
        
    }

    public void ActivateUpgradeMenu()
    {
        Cursor.visible = true;
        isGamePaused = true;
        upgradeMenu.SetActive(true);
        Time.timeScale = 0f;
        upgradeMenu.GetComponent<UpgradeScreen>().Init();

    }

    public void ActivateEnvironmentMenu()
    {
        if (upgradeMenu.activeInHierarchy)
        {
            upgradeMenu.GetComponent<UpgradeScreen>().Close();
            upgradeMenu.SetActive(false);
        }
        if ((chestsSpawned+1) % 2 == 0)
        {
            environmentMenu.SetActive(true);
            environmentMenu.GetComponent<EnvironmentScreen>().Init();
        }
        else
        {
            Resume();
            ScheduleWaveStart();
        }

    }

    public void SpawnChest()
    {
        Debug.Log("Trying to spawn chest");
        if((waveNumber % 2 == 0 || lastWave) && chestsSpawned < 2)
        {
            Debug.Log("Chest spawned!");
            chestsSpawned++;
            AStar pathfinding = (AStar)GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
            Vector3 playerPosition = GameObject.FindWithTag(Constants.PLAYER_TAG).transform.position;
            Vector3 chestPosition = pathfinding.FindFreeTileInRange(playerPosition, 5, 7);

            Instantiate(upgradeChest, chestPosition, Quaternion.identity);
        }
        //if (spawnedChest)
        //{
        //    Destroy(spawnedChest);
        //}

        
    }

    public void InitDeathScreen()
    {
        DeathScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Time.timeScale = 0f;
    }

    void Update()
    {
        if (!lastWave)
        {
            nextWaveIn -= Time.deltaTime;
            nextWaveInObject.UpdateTime(nextWaveIn);
            if (nextWaveIn < 0)
            {
                WaveEnded();
            }
        }
        
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
                    chestsPickedUp++;
                    
                    Destroy(spawnedChest); // start destroy animation
                    if (lastWave && chestsPickedUp == 2)
                    {
                        lastWave = false;
                        chestsPickedUp = 0;
                        chestsSpawned = 0;
                        ScheduleWaveStart();
                    }
                    
                }
                
            }
        }
        // rework 
        //if (Input.GetButtonDown("Fire3"))
        //{
        //    SpawnChest();
        //}
        //testing
        
        if (Input.GetButtonDown("LevelSwitch")) {
            levelManager.SetupNextLevel();
        }

        if (!player)
        {
            InitDeathScreen();
        }
    }

    public void SetCanUpgrade( bool canUpgrade, GameObject chest)
    {
        this.canUpgrade = canUpgrade;
        spawnedChest = chest;
    }
}
