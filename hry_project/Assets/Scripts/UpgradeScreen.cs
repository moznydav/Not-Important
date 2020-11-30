
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    [SerializeField] GameObject[] offensiveUpgrades;
    [SerializeField] GameObject[] healthUpgrades;
    [SerializeField] GameObject[] movementUpgrades;
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
        int randomIndex = Mathf.RoundToInt(Random.Range(0, offensiveUpgrades.Length));
        currentOffensive = Instantiate(offensiveUpgrades[randomIndex], offensivePlace.transform);

        randomIndex = Mathf.RoundToInt(Random.Range(0, healthUpgrades.Length));
        currentHealth = Instantiate(healthUpgrades[randomIndex], healthPlace.transform);

        randomIndex = Mathf.RoundToInt(Random.Range(0, movementUpgrades.Length));
        currentMovement = Instantiate(movementUpgrades[randomIndex], movementPlace.transform);

    }

    public void Close()
    {
        Destroy(currentMovement);
        Destroy(currentOffensive);
        Destroy(currentHealth);

        //gameManager.ScheduleWaveStart();
    }
}
