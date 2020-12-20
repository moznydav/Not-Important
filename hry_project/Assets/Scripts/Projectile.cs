﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float lifespan = 1f;
    [SerializeField] int ricochet = 2;
    [SerializeField] int pierce = 0;
    [SerializeField] float pierceDamageModifier = 0.4f;
    [SerializeField] float knockBackValue = 0.6f;


    float damage;

    bool attackDone = false;
    Rigidbody2D rigidBody;
    Vector2 direction;
    string lastHit = "Karel";

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(HandleLifeTime());

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Pierce value: " + pierce);
        if (!attackDone)
        {
            var other = collision.collider;
            Stats stats = other.GetComponent<Stats>();

            if (other.tag == Constants.DESTROYABLE_TAG)
            {
                var destroyable = other.GetComponent<Destroyable>();
                destroyable.Destroy();
            }
            //else if (stats && lastHit != other.name)
            //{
            //    stats.DealDamage(damage);
            //    lastHit = other.name;
            //}
            else if ( ricochet-- > 0)
            {
                var contact = collision.GetContact(0);

                direction = Vector2.Reflect(direction, contact.normal);
                direction.Normalize();
                rigidBody.velocity = direction * projectileSpeed;
                attackDone = true;
                return;
            }

            attackDone = true;
            //Debug.Log("HIT " + other.name);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       // if (!attackDone)
       // {
            Stats stats = other.GetComponent<Stats>();

            if (stats)
            {
                stats.DealDamage(damage);
                lastHit = other.name;
                attackDone = true;
                other.transform.Translate(direction * knockBackValue);
                if(pierce > 0)
                {
                    damage *= pierceDamageModifier;
                    pierce--;
                }
                else
                {
                    Destroy(gameObject);
                }
                
         //   }


            // Debug.Log("HIT " + other.name);


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

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void SetPierce(int pierceValue)
    {
        pierce = pierceValue;
    }

    private IEnumerator HandleLifeTime()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
