using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject goblin;
    void Spawn()
    {
        print("Spawning");
        GameObject new_goblin = Instantiate(goblin, transform.position, Quaternion.identity);
    }

    void Start()
    {
        Spawn();
    }

    
}
