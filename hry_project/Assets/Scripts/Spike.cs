using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float damageValue = 15f; //additive

    private void OnTriggerEnter2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.DealDamage(damageValue);
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();

        if (stats) {
            stats.DealDamage(damageValue);
        }
    }
}
