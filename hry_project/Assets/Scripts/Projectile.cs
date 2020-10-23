using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 20f;

    float damage;

    private void Awake()
    {
        StartCoroutine(HandleLifeTime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats stats = other.GetComponent<Stats>();
        if (stats)
        {
            stats.DealDamage(damage);
        }
        Debug.Log("HIT " + other.name);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT WALL");
        Destroy(gameObject);
    }


    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetProjectileSPeed()
    {
        return projectileSpeed;
    }


    private IEnumerator HandleLifeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
