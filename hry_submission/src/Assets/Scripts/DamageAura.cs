using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAura : MonoBehaviour
{
    [SerializeField] float damage;
    PlayerStats playerStats;
    Collider2D damageZone;


    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        damageZone = GetComponent<Collider2D>();
        damageZone.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        Stats stats = other.GetComponent<Stats>();

        if (stats)
        {
            damage = playerStats.maxHealth * playerStats.damageAuraMultiplier;
            stats.DealDamage(damage, null);
        }

    }


    public void TurnOnDamage()
    {
        damageZone.enabled = true;
    }
    public void TurnOffDamage()
    {
        damageZone.enabled = false;
    }
    //TODO: Handle it with animator

}
