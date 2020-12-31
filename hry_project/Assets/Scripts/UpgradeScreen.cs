
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int[] topIndexes; // {2,5,8,9}
    [SerializeField] public int[] boundries; // {3,5,7}

    [Header("Upgrades")]
    [SerializeField] GameObject[] offensiveUpgrades;
    [SerializeField] GameObject[] DefenceUpgrades;
    [SerializeField] GameObject[] UtilityUpgrades;
    [Header("Places")]
    [SerializeField] GameObject offensivePlace;
    [SerializeField] GameObject healthPlace;
    [SerializeField] GameObject movementPlace;

    private GameObject currentOffensive;
    private GameObject currentHealth;
    private GameObject currentMovement;

    //GameManager gameManager;
    //Set up index handling;
    PlayerStats playerStats;
    int lastUsedOffenceIndex;
    int lastUsedDefenceIndex;
    int lastUsedUtilityIndex;

    int currentBoundryOffenceIndex;
    int currentBoundryDefenceIndex;
    int currentBoundryUtilityIndex;



    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        lastUsedOffenceIndex = -1;
        lastUsedDefenceIndex = -1;
        lastUsedUtilityIndex = -1;

        currentBoundryOffenceIndex = 0;
        currentBoundryDefenceIndex = 0;
        currentBoundryUtilityIndex = 0;
    }

    public void Init()
    {      
        int randomIndex = Mathf.RoundToInt(Random.Range(0, offensiveUpgrades.Length - 1));
        currentOffensive = Instantiate(offensiveUpgrades[randomIndex], offensivePlace.transform);
        
        randomIndex = Mathf.RoundToInt(Random.Range(0, DefenceUpgrades.Length - 1));
        currentHealth = Instantiate(DefenceUpgrades[randomIndex], healthPlace.transform);
        
        randomIndex = Mathf.RoundToInt(Random.Range(0, UtilityUpgrades.Length - 1));
        currentMovement = Instantiate(UtilityUpgrades[randomIndex], movementPlace.transform);

    }

    public void Close()
    {
        Destroy(currentMovement);
        Destroy(currentOffensive);
        Destroy(currentHealth);

        //gameManager.ScheduleWaveStart();
    }
}
