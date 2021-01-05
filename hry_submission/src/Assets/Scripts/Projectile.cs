﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float lifespan = 1f;
    [SerializeField] int ricochet = 0;
    [SerializeField] int pierce = 0;
    [SerializeField] float pierceDamageModifier = 0.4f;
    [SerializeField] float knockBackValue = 0.6f;
    [SerializeField] public float poisonDamage;
    [SerializeField] public int poisonTicks = 6;
    [SerializeField] float scopeIntervals = 0.1f;
    [SerializeField] float brokenScopeModifier = 6f;
    [SerializeField] float SniperScopeModifier = 5f;

    public bool poisoned;
    [SerializeField] float damage;

    public bool exploding;
    GameObject explosion;
    Stats origin;
    float explosionDamage;
    bool brokenScope = false;
    bool sniperScope = false;

    [Header("SFX stuff")]
    [SerializeField] AudioSource myAudioSource;
    [SerializeField] bool isPlayers;
    [SerializeField] float explosionVolume;
    [SerializeField] float othersVolume;
    [SerializeField] AudioClip bounceSFX;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip swingSFX;

    bool attackDone = false;
    Rigidbody2D rigidBody;
    Vector2 direction;

   

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(HandleLifeTime());
        if (isPlayers) {
            AudioSource.PlayClipAtPoint(swingSFX, Camera.main.transform.position, othersVolume);
        }
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
                if (isPlayers) {
                    AudioSource.PlayClipAtPoint(bounceSFX, Camera.main.transform.position, othersVolume);
                }
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
        if (!attackDone)
        {
            attackDone = true;
            Stats stats = other.GetComponent<Stats>();
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats)
            {
                playerStats.DealDamage(damage, origin);
                if (pierce > 0)
                {
                    damage *= pierceDamageModifier;
                    pierce--;
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            else if (stats)
            {
                if (poisoned)
                {
                    stats.ApplyPoison(poisonTicks, poisonDamage);
                }
                stats.DealDamage(damage,origin);
                if(isPlayers && !exploding) {
                    Debug.Log("HIT!");
                    AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, othersVolume);
                }

                if (exploding)
                {
                    GameObject BoomBoom = Instantiate(explosion, transform.position, Quaternion.identity);
                    BoomBoom.GetComponent<Explosion>().SetUpExplosion(explosionDamage, poisoned, poisonDamage, poisonTicks);
                    AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position);
                }
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
                
            }


            // Debug.Log("HIT " + other.name);


            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attackDone = false;
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

    public void SetPoison(int ticks, int damage)
    {
        poisoned = true;
        poisonTicks = ticks;
        poisonDamage = damage;
    }

    public void SetExplosion(GameObject exlosionPrefab,float damage)
    {
        exploding = true;
        explosion = exlosionPrefab;
        explosionDamage = damage;
    }

    public void SetScopes(bool broken, bool sniper)
    {
        brokenScope = broken;
        sniperScope = sniper;
        StartCoroutine(HandleScopes());
    }

    private IEnumerator HandleScopes()
    {
        while (true)
        {
            yield return new WaitForSeconds(scopeIntervals);
            if (brokenScope)
            {
                damage *= brokenScopeModifier;
            }
            if (sniperScope)
            {
                damage *= SniperScopeModifier;
            }
        }
        
    }
    public void SetRicochet(int value)
    {
        ricochet = value;
    }
    public void SetOrigin(Stats creator)
    {
        origin = creator;
    }

}