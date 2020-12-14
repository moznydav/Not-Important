using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : MonoBehaviour
{
    [SerializeField] float slowValue = 200f; //additive

    

    private void OnTriggerEnter2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.UpdateSpeed(new StatModifier(-slowValue));
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.UpdateSpeed(new StatModifier(slowValue));
        }
    }
}
