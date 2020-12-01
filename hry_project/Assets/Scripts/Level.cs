using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public EnemySpawner[] enemySpawners;
}
