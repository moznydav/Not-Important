using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentButton: MonoBehaviour
{
    GameManager gameManager;
    LevelManager levelManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void ExplosivesButton() {
        Debug.Log("Pressed explosives button");
        levelManager.ActivateExplosives();
        Close();
    }

    public void BarricadeButton() {
        levelManager.ActivateBarricades();
        Close();
    }
    public void TarPoolsButton() {
        levelManager.ActivateTarPools();
        Close();
    }

    public void SpikesButton() {
        levelManager.ActivateSpikes();
        Close();
    }

    public void Skip() {
        Close();
    }


    public void Close() {
        gameManager.Resume();
        gameManager.ScheduleWaveStart();
    }
}
