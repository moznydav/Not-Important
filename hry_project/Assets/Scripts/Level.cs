using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public EnemySpawner[] enemySpawners;
    [SerializeField] public GameObject explosives;
    [SerializeField] public GameObject tarPools;
    [SerializeField] public GameObject barricades;
    [SerializeField] public GameObject spikes;

    [SerializeField] public GameObject[] explosivesArray;
    [SerializeField] public GameObject[] barricadesArray;

    [SerializeField] public bool explosivesActive = false;
    [SerializeField] public bool barricadesActive = false;

    Vector2[] explosivesPositions;
    Vector2[] barricadesPositions;



    private void Awake() {
        SavePositions();
    }

    private void SavePositions() {
        for (int i = 0; i < explosivesArray.Length; i++) {
            explosivesPositions[i] = explosivesArray[i].transform.position;
        }

        for (int i = 0; i < barricadesArray.Length; i++) {
            explosivesPositions[i] = barricadesArray[i].transform.position;
        }
    }

    public void EnvironmentalReset() {
        if (barricadesActive) {
            BarricadeReset();
        }
        if (explosivesActive) {
            ExplosivesReset();
        }
    }

    private void ExplosivesReset() {
        Debug.Log("Explosive Reset.");
        for (int i = 0; i < explosivesArray.Length; i++) {
            Instantiate(explosivesArray[i], explosivesPositions[i], Quaternion.identity);
        }
    }
    private void BarricadeReset() {
        Debug.Log("Barricade Reset.");
        for (int i = 0; i < barricadesArray.Length; i++) {
            Instantiate(barricadesArray[i], barricadesPositions[i], Quaternion.identity);
        }
    }
}


