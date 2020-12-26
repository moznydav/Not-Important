using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour {
    [SerializeField] public Tilemap walls;
    [SerializeField] public EnemySpawner[] enemySpawners;

    [SerializeField] public GameObject[] spikes;
    [SerializeField] public GameObject[] tarPools;
    [SerializeField] public GameObject[] barricades;
    [SerializeField] public GameObject[] explosives;


}
