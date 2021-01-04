using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrail : MonoBehaviour
{
    [SerializeField] float poisonDamage;
    [SerializeField] int poisonTicks;
    [SerializeField] float lifespan = 1f;
    // Start is called before the first frame update
    private int baseTicks = 3;
    private void Awake()
    {
        StartCoroutine(HandleLifeTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats stats = other.GetComponent<Stats>();

        if (stats)
        { 
            stats.ApplyPoison(baseTicks, poisonDamage);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Stats stats = other.GetComponent<Stats>();

        if (stats)
        {
            stats.ApplyPoison(poisonTicks, poisonDamage);
        }
    }

    private IEnumerator HandleLifeTime()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

    public void SetUpTrail(float damage, int ticks)
    {
        poisonDamage = damage;
       // poisonTicks = ticks;
    }


}
