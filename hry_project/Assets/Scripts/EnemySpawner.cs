using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public IEnumerator Spawn(GameObject[] types)
    {
        Vector2 spawnPosition;
        for (int i = 0; i < types.Length; i++) {
            spawnPosition = transform.position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0);
            gameManager.currentEnemyCount++;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));
            Instantiate(types[i], spawnPosition, Quaternion.identity);
        }
    }
}
