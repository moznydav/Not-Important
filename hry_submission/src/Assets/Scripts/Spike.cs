using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float damageValue = 15f; //additive

    private void OnTriggerEnter2D(Collider2D other) {
        Stats stats = other.GetComponent<Stats>();
        PlayerStats playerstats = FindObjectOfType<PlayerStats>();
        PlayerStats playercheck = other.GetComponent<PlayerStats>();

        if (stats) {
            if (playerstats.hasPoisonTraps) {
                if (playercheck) {
                    stats.DealDamage(damageValue, null);
                } else {
                    stats.DealDamage(damageValue, null);
                    stats.ApplyPoison(playerstats.poisonTicks, playerstats.poisonDamage);
                }
            } else {
                stats.DealDamage(damageValue, null);
            }
        }
    }
}
