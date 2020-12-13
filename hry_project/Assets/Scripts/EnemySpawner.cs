using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Spawn(GameObject[] types)
    {
        for (int i = 0; i < types.Length; i++) {
            gameManager.currentEnemyCount++;
            Instantiate(types[i], transform.position, Quaternion.identity);
        }
    }    
}
