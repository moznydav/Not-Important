using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : EnemyShooter
{
    // Config
    [Header("Config Beholder")]
    [SerializeField] GameObject legs;

    //[SerializeField] bool attackOnCooldown = false;

    private void Awake()
    {
        InitializeEnemy();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAim();
        HandleBeholderMovement();
        if (CanAttack())
        {
            if (!attackOnCooldown)
            {
                attackOnCooldown = true;
                anim.SetBool("Attacking", true);
                StartCoroutine(ShootingCooldown());
                legs.GetComponent<Animator>().SetBool("Attacking", true);

            }
        }
    }

    
    void HandleBeholderMovement()
    {
        base.HandleMovement();
        legs.GetComponent<Animator>().SetFloat("Horizontal", GetMoveDirection().x);
        legs.GetComponent<Animator>().SetFloat("Magnitude", GetMoveDirection().magnitude);

    }

    private void Shoot() // used by animation
    {
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
        shot.GetComponent<Rigidbody2D>().velocity = aimDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();
        shot.GetComponent<Projectile>().SetDamage(stats.damage);
        shot.GetComponent<Projectile>().SetDirection(aimDirection);
        shot.transform.eulerAngles = new Vector3(0, 0, angle);
        legs.GetComponent<Animator>().SetBool("Attacking", false);
        anim.SetBool("Attacking", false);
    }

}
