using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    //parameters
    [SerializeField] int enemiesCount;
    [SerializeField] int waveNumber = 0;

    //spawner
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] List<GameObject> enemyTypes;
    [SerializeField] int enemyCountMultiplier = 3;
    [SerializeField] int enemyTypeInterval = 3;
    private int activeEnemyTypes = 1;

    public void CountEnemies() {
        enemiesCount++;
    }

    public void EnemyDestroyed() {
        enemiesCount--;
        if (enemiesCount <= 0) {
            Debug.Log("Wave should end.");
            FindObjectOfType<GameManager>().WaveEnded();
        }
    }

    public void WaveStart() {
        waveNumber++;
        SpawnerHandler();

    }

    private void SpawnerHandler() {
        print("Wave " + waveNumber + " started");
        if (waveNumber % enemyTypeInterval == 0) {
            if (activeEnemyTypes + 1 < enemyTypes.Capacity) {
                activeEnemyTypes++;
            }
        }
        List<GameObject> availableTypes = enemyTypes.GetRange(0, activeEnemyTypes);
        int typesToChoose = (int)Math.Ceiling((double)availableTypes.Count / 2);
        // TODO: randomly choose enemy types
        // and distribute them evenly across all spawners
        List<GameObject> chosenTypes = availableTypes.GetRange(0, typesToChoose);
        foreach (EnemySpawner spawner in enemySpawners) {
            spawner.Spawn(chosenTypes, waveNumber * enemyCountMultiplier);
        }
    }


}