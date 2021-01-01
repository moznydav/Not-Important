using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    // Config
    [Header("Config")]
    [SerializeField] float loungeSpeed = 500f;
    [SerializeField] float attackCooldown = 0.7f;
    [SerializeField] float damageCooldown = 1f;

    // Cached variable
    Animator anim;

    bool attacking = false;
    bool attackDone = false;
    Vector3 attackDirection = new Vector3(0f, 0f, 0f);


    private void Awake()
    {
        InitializeEnemy();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!spawning)
        {
            HandleMovement();
            if (!attacking)
            {
                anim.SetFloat("Horizontal", GetMoveDirection().x);
                anim.SetFloat("Magnitude", GetMoveDirection().magnitude);
                if (CanAttack())
                {
                    attacking = true;
                    StartCoroutine(StartCooldown());
                }
            }
            else
            {
                transform.position += attackDirection * loungeSpeed * Time.fixedDeltaTime * Time.deltaTime;
            }
        }

    }

    private IEnumerator StartCooldown()
    {
        SetCanRun(false);
        if (player)
        {
            attackDirection.x = player.transform.position.x - transform.position.x;
            attackDirection.y = player.transform.position.y - transform.position.y;
            attackDirection.z = 0f;
            attackDirection.Normalize();
            anim.SetFloat("Horizontal", attackDirection.x);

            transform.position += attackDirection * loungeSpeed * Time.fixedDeltaTime * Time.deltaTime;
            anim.SetBool("Attacking", true);
        }
        yield return new WaitForSeconds(attackCooldown);
        anim.SetBool("Attacking", false);
        attacking = false;
        SetCanRun(true);

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
            otherStats.DealDamage(stats.damage,stats);
            attackDone = true;
            StartCoroutine(DamageCooldown());
        }
    }
}
;