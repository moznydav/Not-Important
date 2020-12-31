
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
        /*
         *  int randomIndex = Mathf.RoundToInt(Random.Range(0, offensiveUpgrades.Length - 1));
         *  currentOffensive = Instantiate(offensiveUpgrades[randomIndex], offensivePlace.transform);
         *  
         *  randomIndex = Mathf.RoundToInt(Random.Range(0, DefenceUpgrades.Length - 1));
         *  currentHealth = Instantiate(DefenceUpgrades[randomIndex], healthPlace.transform);
         *  
         *  randomIndex = Mathf.RoundToInt(Random.Range(0, UtilityUpgrades.Length - 1));
         *  currentMovement = Instantiate(UtilityUpgrades[randomIndex], movementPlace.transform);
         * 
         * 
         * 
         */

        //HELP

        if(playerStats.offenceCounter > boundries[boundries.Length - 1])
        {
            playerStats.offenceCounter = 0;
        }
        if (playerStats.defenceCounter > boundries[boundries.Length - 1])
        {
            playerStats.defenceCounter = 0;
        }
        if (playerStats.utilityCounter > boundries[boundries.Length - 1])
        {
            playerStats.utilityCounter = 0;
        }
        if (playerStats.offenceCounter > boundries[currentBoundryOffenceIndex])
        {
            currentBoundryOffenceIndex++;
            lastUsedOffenceIndex = -1;
        }
        if (playerStats.defenceCounter > boundries[currentBoundryDefenceIndex])
        {
            currentBoundryDefenceIndex++;
            lastUsedDefenceIndex = -1;
        }
        if (playerStats.utilityCounter > boundries[currentBoundryUtilityIndex])
        {
            currentBoundryUtilityIndex++;
            lastUsedUtilityIndex = -1;
        }

        //offence
        if(currentBoundryOffenceIndex)
        int[] availibleIndexes = new int[3]; //fix the 3
        int startVal;
        if(currentBoundryOffenceIndex == 0)
        {
            startVal = 0;
        }
        else
        {
            startVal = topIndexes[currentBoundryOffenceIndex - 1] + 1;
        }
        for(int i = 0; i < availibleIndexes.Length; i++)
        {
            availibleIndexes[i] = startVal;
            startVal++;
        }
        int randomIndex = Mathf.RoundToInt(Random.Range(0,availibleIndexes.Length - 1));
        Instantiate(offensiveUpgrades[randomIndex], offensivePlace.transform);

        if (currentBoundryDefenceIndex== 0)
        {
            startVal = 0;
        }
        else
        {
            startVal = topIndexes[currentBoundryDefenceIndex- 1] + 1;
        }
        for (int i = 0; i < availibleIndexes.Length; i++)
        {
            availibleIndexes[i] = startVal;
            startVal++;
        }
        randomIndex = Mathf.RoundToInt(Random.Range(0, availibleIndexes.Length - 1));
        Instantiate(DefenceUpgrades[randomIndex], healthPlace.transform);

        if (currentBoundryUtilityIndex== 0)
        {
            startVal = 0;
        }
        else
        {
            startVal = topIndexes[currentBoundryUtilityIndex - 1] + 1;
        }
        for (int i = 0; i < availibleIndexes.Length; i++)
        {
            availibleIndexes[i] = startVal;
            startVal++;
        }
        randomIndex = Mathf.RoundToInt(Random.Range(0, availibleIndexes.Length - 1));
        Instantiate(UtilityUpgrades[randomIndex], movementPlace.transform);
    }

    public void Close()
    {
        Destroy(currentMovement);
        Destroy(currentOffensive);
        Destroy(currentHealth);

        //gameManager.ScheduleWaveStart();
    }
}
