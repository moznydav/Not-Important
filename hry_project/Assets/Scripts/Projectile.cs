using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float lifespan = 1f;
    [SerializeField] int ricochet = 1;

    float damage;

    bool attackDone = false;
    Rigidbody2D rigidBody;
    Vector2 direction;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(HandleLifeTime());

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!attackDone)
        {
            var other = collision.collider;
            Stats stats = other.GetComponent<Stats>();

            if (stats)
            {
                stats.DealDamage(damage);
            }

            if (other.name == "Walls" && ricochet-- > 0)
            {
                var contact = collision.GetContact(0);

                direction = Vector2.Reflect(direction, contact.normal);
                direction.Normalize();
                rigidBody.velocity = direction * projectileSpeed;

                return;
            }

            attackDone = true;
            Debug.Log("HIT " + other.name);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attackDone)
        {
            Stats stats = other.GetComponent<Stats>();

            if (stats)
            {
                stats.DealDamage(damage);
            }

            attackDone = true;
            Debug.Log("HIT " + other.name);

            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public float GetProjectileSPeed()
    {
        return projectileSpeed;
    }


    private IEnumerator HandleLifeTime()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
