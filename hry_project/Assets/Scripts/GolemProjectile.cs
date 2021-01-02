using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private bool followPlayer;
    [SerializeField] Collider2D dmgArea;
    [SerializeField] GameObject player;
    [SerializeField] Stats origin;

    private void Awake()
    {
        dmgArea = GetComponent<Collider2D>();
        dmgArea.enabled = false;
        followPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            Stats stats = other.GetComponent<Stats>();

            if (stats)
            {
                stats.DealDamage(damage,origin);
            GetComponent<Collider2D>().enabled = false;
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

    public void SetPlayerAndOrigin(GameObject player, Stats origin)
    {
        this.player = player;
        this.origin = origin;

    }
   
}
