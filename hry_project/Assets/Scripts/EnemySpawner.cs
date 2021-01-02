using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public IEnumerator Spawn(GameObject[] types, int runsDone)
    {

        gameManager = FindObjectOfType<GameManager>();
        Vector2 spawnPosition;
        for (int i = 0; i < types.Length; i++) {
            spawnPosition = transform.position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0);
            gameManager.currentEnemyCount++;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));
            GameObject enemy = Instantiate(types[i], spawnPosition, Quaternion.identity);
            enemy.GetComponent<Stats>().damage += enemy.GetComponent<Stats>().damage * Mathf.Pow(2,runsDone) ;
            enemy.GetComponent<Stats>().maxHealth += enemy.GetComponent<Stats>().maxHealth * Mathf.Pow(2, runsDone);
            enemy.GetComponent<Stats>().HealToMax();
        }
    }
}
