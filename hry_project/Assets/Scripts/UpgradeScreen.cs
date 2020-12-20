
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
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
