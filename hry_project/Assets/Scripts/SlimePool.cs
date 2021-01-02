using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : MonoBehaviour
{
    float slowValue = 100f; //additive

    

    private void OnTriggerEnter2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.moveSpeed -= slowValue;
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.moveSpeed += slowValue;
        }
    }
}
