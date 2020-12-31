
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreen : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int[] bottomOffensiveIndicies;
    [SerializeField] int[] bottomDefensiveIndicies;
    [SerializeField] int[] bottomUtilityIndicies;

    [Header("Upgrades")]
    [SerializeField] List<GameObject> offensiveUpgradesTierOne;
    [SerializeField] List<GameObject> offensiveUpgradesTierTwo;
    [SerializeField] List<GameObject> offensiveUpgradesTierThree;
    [SerializeField] List<GameObject> offensiveUpgradesTierFour;

    [SerializeField] List<GameObject> defenceUpgradesTierOne;
    [SerializeField] List<GameObject> defenceUpgradesTierTwo;
    [SerializeField] List<GameObject> defenceUpgradesTierThree;
    [SerializeField] List<GameObject> defenceUpgradesTierFour;

    [SerializeField] List<GameObject> utilityUpgradesTierOne;
    [SerializeField] List<GameObject> utilityUpgradesTierTwo;
    [SerializeField] List<GameObject> utilityUpgradesTierThree;
    [SerializeField] List<GameObject> utilityUpgradesTierFour;


    [Header("Places")]
    [SerializeField] GameObject offensivePlace;
    [SerializeField] GameObject healthPlace;
    [SerializeField] GameObject movementPlace;

    private GameObject currentOffensive;
    private GameObject currentHealth;
    private GameObject currentMovement;

    private List<GameObject>[] offensiveUpgrades;
    private List<GameObject>[] defensiveUpgrades;
    private List<GameObject>[] utilityUpgrades;


    //GameManager gameManager;
    //Set up index handling;
    Player player;

    private void Awake()
    {
        player = (Player) GameObject.FindWithTag(Constants.PLAYER_TAG).GetComponent(typeof(Player));

        offensiveUpgrades = new List<GameObject>[] {
            offensiveUpgradesTierOne, offensiveUpgradesTierTwo, offensiveUpgradesTierThree, offensiveUpgradesTierFour
        };

        defensiveUpgrades = new List<GameObject>[] {
            defenceUpgradesTierOne, defenceUpgradesTierTwo, defenceUpgradesTierThree, defenceUpgradesTierFour
        };

        utilityUpgrades = new List<GameObject>[] {
            utilityUpgradesTierOne, utilityUpgradesTierTwo, utilityUpgradesTierThree, utilityUpgradesTierFour
        };
    }

    private int GetUpgradeIndex(int upgradeCount, int[] ranges)
    {
        int level = 0;

        for (int i = 0; i < ranges.Length - 1; i++)
        {
            if (upgradeCount >= ranges[i])
            {
                level++;
            }
        }

        return level;
    }

    public void Init()
    {
        int nOffensiveUpgrades = player.offsenseUpgrades % (bottomOffensiveIndicies[bottomOffensiveIndicies.Length - 1] + 1);
        int nDefensiveUpgrades = player.defenseUpgrades % (bottomDefensiveIndicies[bottomDefensiveIndicies.Length - 1] + 1);
        int nUtilityUpgrades = player.utilityUpgrades % (bottomUtilityIndicies[bottomUtilityIndicies.Length - 1] + 1);

        int currentOffensiveIndex = GetUpgradeIndex(nOffensiveUpgrades, bottomOffensiveIndicies);
        int currentDefensiveIndex = GetUpgradeIndex(nDefensiveUpgrades, bottomDefensiveIndicies);
        int currentUtilityIndex = GetUpgradeIndex(nUtilityUpgrades, bottomUtilityIndicies);

        var currentOffsensiveList = offensiveUpgrades[currentOffensiveIndex];
        var currentDefensiveList = defensiveUpgrades[currentDefensiveIndex];
        var currentUtilityList = utilityUpgrades[currentUtilityIndex];

        int randomIndex = Mathf.RoundToInt(Random.Range(0, currentOffsensiveList.Count - 1));
        currentOffensive = Instantiate(currentOffsensiveList[randomIndex], offensivePlace.transform);

        var buttonGM = currentOffensive.transform.Find("Button").gameObject;
        buttonGM.GetComponent<Button>().onClick.AddListener(SelectOffensive);

        randomIndex = Mathf.RoundToInt(Random.Range(0, currentDefensiveList.Count - 1));
        currentHealth = Instantiate(currentDefensiveList[randomIndex], healthPlace.transform);

        buttonGM = currentHealth.transform.Find("Button").gameObject;
        buttonGM.GetComponent<Button>().onClick.AddListener(SelectDefensive);

        randomIndex = Mathf.RoundToInt(Random.Range(0, currentUtilityList.Count - 1));
        currentMovement = Instantiate(currentUtilityList[randomIndex], movementPlace.transform);

        buttonGM = currentMovement.transform.Find("Button").gameObject;
        buttonGM.GetComponent<Button>().onClick.AddListener(SelectUtility);
    }

    public void Close()
    {
        Destroy(currentMovement);
        Destroy(currentOffensive);
        Destroy(currentHealth);

        //gameManager.ScheduleWaveStart();
    }

    public void SelectDefensive()
    {
        player.defenseUpgrades++;
    }

    public void SelectOffensive()
    {
        player.offsenseUpgrades++;
    }

    public void SelectUtility()
    {
        player.utilityUpgrades++;
    }
}
