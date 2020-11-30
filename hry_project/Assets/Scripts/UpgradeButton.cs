using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] Button button;

    GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();      
    }

    public void Close()
    {
        gameManager.Resume();
        gameManager.ScheduleWaveStart();
    }
}
