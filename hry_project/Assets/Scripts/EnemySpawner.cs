using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void Spawn(List<GameObject> types, int count)
    {
        for (int i = 0; i < types.Count; i++) {
            for (int j = 0; j < count; j++) {
                Instantiate(types[i], transform.position, Quaternion.identity);
            }
        }
    }    
}
