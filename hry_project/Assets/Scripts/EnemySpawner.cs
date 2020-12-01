using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Spawn(GameObject[] types, int count)
    {
        for (int i = 0; i < types.Length; i++) {
            for (int j = 0; j < count; j++) {
                gameManager.currentEnemyCount++;
                Instantiate(types[i], transform.position, Quaternion.identity);
            }
        }
    }    
}
