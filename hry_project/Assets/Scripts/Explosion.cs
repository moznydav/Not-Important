using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float poisonDamage;
    int poisonTicks;
    public bool poisoned;
    float damage;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats stats = other.GetComponent<Stats>();

        if (stats)
        {
            if (poisoned)
            {
                stats.ApplyPoison(poisonTicks, poisonDamage);
            }
            stats.DealDamage(damage,null);
        }

    }

    public void SetUpExplosion(float damage, bool isPoisoned, float poisonDamage, int ticks)
    {
        this.damage = damage;
        poisoned = isPoisoned;
        this.poisonDamage = poisonDamage;
        this.poisonTicks = ticks;
    }

    private void StopExplosion()
    {
        Destroy(gameObject);
    }

}
