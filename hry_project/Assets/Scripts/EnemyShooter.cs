using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    // Config
    [Header("Config")]
    [SerializeField] public GameObject projectile;
    [SerializeField] public float attackCooldown = 2f;
    [SerializeField] public float meleeDamageCooldown = 1f;


    // Cached variable
    public Animator anim;

    [SerializeField] public bool attackOnCooldown = false;
    public bool attackDone = false;
    public Vector3 aimDirection;

    public void HandleAim()
    {
        if (player)
        {
            aimDirection.x = player.transform.position.x - transform.position.x;
            aimDirection.y = player.transform.position.y - transform.position.y;
            aimDirection.z = 0f;
            aimDirection.Normalize();

            anim.SetFloat("AimHorizontal", aimDirection.x);
        }
    }

    public IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    public IEnumerator MeleeDamageCooldown()
    {
        yield return new WaitForSeconds(meleeDamageCooldown);
        attackDone = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats otherStats = other.GetComponent<Stats>();
        if (otherStats is PlayerStats && !attackDone)
        {
            otherStats.DealDamage(stats.damage.value);
            StartCoroutine(MeleeDamageCooldown());
        }
    }
}

