using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : Enemy
{
    // Config
    [Header("Config")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject legs;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float meleeDamageCooldown = 1f;


    // Cached variable
    Animator anim;

    [SerializeField] bool attackOnCooldown = false;
    bool attackDone = false;
    Vector3 aimDirection;


    private void Awake()
    {
        InitializeEnemy();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAim();
        HandleMovement();
        if (CanAttack())
        {
            if (!attackOnCooldown)
            {
                attackOnCooldown = true;
                anim.SetBool("Attacking", true);
                legs.GetComponent<Animator>().SetBool("Attacking", true);
                StartCoroutine(ShootingCooldown());

            }
        }
    }

    void HandleMovement()
    {
        base.HandleMovement();
        legs.GetComponent<Animator>().SetFloat("Horizontal", GetMoveDirection().x);
        legs.GetComponent<Animator>().SetFloat("Magnitude", GetMoveDirection().magnitude);

    }

    void HandleAim()
    {
        if (player)
        {
            aimDirection.x = player.transform.position.x - transform.position.x;
            aimDirection.y = player.transform.position.y - transform.position.y;
            aimDirection.z = 0f;
            aimDirection.Normalize();

            anim.SetFloat("AimHorizontal", aimDirection.x);
            legs.GetComponent<Animator>().SetFloat("AimHorizontal", aimDirection.x);
        }
    }

    private void Shoot() // used by animation
    {
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
        shot.GetComponent<Rigidbody2D>().velocity = aimDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();
        shot.GetComponent<Projectile>().SetDamage(stats.damage.value);
        shot.GetComponent<Projectile>().SetDirection(aimDirection);
        shot.transform.eulerAngles = new Vector3(0, 0, angle);
        legs.GetComponent<Animator>().SetBool("Attacking", false);
        anim.SetBool("Attacking", false);
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }

    private IEnumerator MeleeDamageCooldown()
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
