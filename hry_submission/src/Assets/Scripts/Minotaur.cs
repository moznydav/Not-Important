using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur: EnemyShooter
{
    // Config
    [Header("Config")]
    [SerializeField] float ChargeSpeed = 500f;
    [SerializeField] float damageCooldown = 1f;
    [SerializeField] float chargingTime = 3f;
    [SerializeField] float predictionMultiplier = 0.5f;

    // Cached variable

    [SerializeField] bool charging;
    [SerializeField] bool preparing;
    // bool attackDone = false;
    [SerializeField] Vector3 chargeDirection;
    private void Awake()
    {
        InitializeEnemy();
        anim = GetComponent<Animator>();
        charging = false;
        preparing = false;
    }

    private void Update()
    {
        if (!spawning)
        {
            HandleAim();
            anim.SetFloat("AimHorizontal", aimDirection.x);
            anim.SetFloat("Horizontal", GetMoveDirection().x);
            if (charging)
            {
                HandleChargeRun();
            }
            else
            {
                HandleMovement();
            }
            if (CanAttack())
            {
                if (!preparing && !charging)
                {
                    anim.SetBool("Charging", true);
                    preparing = true;
                    rigidBody.velocity = new Vector3(0f,0f,0f);
                    SetCanRun(false);
                    //savedMoveSpeed = stats.moveSpeed;
                    //stats.moveSpeed = 0;
                }
            }
        }

    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        attackDone = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats otherStats = other.GetComponent<Stats>();
        if (otherStats is PlayerStats && !attackDone)
        {
            otherStats.DealDamage(stats.damage, stats);
            StartCoroutine(DamageCooldown());
        }
    }

    private void HandleChargeRun()
    {
        chargeDirection.Normalize();
        //transform.position += chargeDirection * stats.moveSpeed * Time.fixedDeltaTime * Time.deltaTime;
        rigidBody.velocity = (chargeDirection * stats.moveSpeed * Time.fixedDeltaTime);
    }

    private void PrepareDone()
    {
        if (preparing)
        {

            Debug.Log("Prep DONE");
            preparing = false;
            charging = true;
            stats.moveSpeed += ChargeSpeed;

            if (player.GetComponent<Player>().moveDirection.magnitude > 0)
            {
                Vector3 playerDir = player.GetComponent<Player>().moveDirection;
                chargeDirection = aimDirection + (playerDir * predictionMultiplier);
            }
            else
            {
                chargeDirection = aimDirection;
            }
            chargeDirection.Normalize();
            StartCoroutine(StartChargeTimer());
            anim.SetFloat("ChargeHorizontal", chargeDirection.x);
        }
    }

    private void chargeDone()
    {
        stats.moveSpeed -= ChargeSpeed;
        charging = false;
        SetCanRun(true);
        anim.SetBool("Charging", false);
    }

    private IEnumerator StartChargeTimer()
    {
        yield return new WaitForSeconds(chargingTime);
        chargeDone();
    }
}
;