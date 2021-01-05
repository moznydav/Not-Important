using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyShooter
{
    // Config
    //[Header("Config Golem")]
    
    //[SerializeField] bool attackOnCooldown = false;

    private void Awake()
    {
        InitializeEnemy();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAim();
        HandleGolemMovement();
        if (CanAttack())
        {
            Debug.Log("Golem Can Attack");
            if (!attackOnCooldown)
            {
                Debug.Log("Golem attack is off cooldown");
                anim.SetBool("Idle", false);
                attackOnCooldown = true;
                anim.SetBool("Attacking", true);
                StartCoroutine(ShootingCooldown());

            }
            else
            {
                anim.SetBool("Idle", true);
            }
        }
        //Debug.Log("Golem Can't Attack");
    }


    void HandleGolemMovement()
    {
        base.HandleMovement();
        anim.SetFloat("Horizontal", GetMoveDirection().x);
    }

    private void Shoot() // used by animation
    {
        
        anim.SetBool("Attacking", false);
        GameObject shot = Instantiate(projectile, player.transform.position, Quaternion.identity);
        shot.GetComponent<GolemProjectile>().SetDamage(stats.damage);
        shot.GetComponent<GolemProjectile>().SetPlayerAndOrigin(player,stats);

    }

}
