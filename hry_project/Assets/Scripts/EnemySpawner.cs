using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject goblin;
    public void Spawn(List<GameObject> types, int count)
    {
        print("Spawning");
        GameObject new_goblin = Instantiate(goblin, transform.position, Quaternion.identity);
    }    
}
