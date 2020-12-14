using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    private float damage = 20f;
    private bool followPlayer;
    Collider2D dmgArea;
    GameObject player;

    private void Awake()
    {
        dmgArea = GetComponent<Collider2D>();
        dmgArea.enabled = false;
        followPlayer = true;
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            Stats stats = other.GetComponent<Stats>();

            if (stats)
            {
                stats.DealDamage(damage);
            }
            // Debug.Log("HIT " + other.name);
    }



    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            Follow();
        }
    }

    private void Follow()
    {
        transform.position = player.transform.position;
    }

    private void StopFollow()
    {
        followPlayer = false;
    }

    private void ActivateDamageArea()
    {
        dmgArea.enabled = true;
    }

    private void KillProjectile()
    {
        Destroy(gameObject);
    }


    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

   
}
