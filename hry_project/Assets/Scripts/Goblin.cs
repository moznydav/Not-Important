using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    // Config
    [Header("Config")]
    [SerializeField] float loungeSpeed = 500f;
    [SerializeField] float attackCooldown = 0.7f;
    [SerializeField] float damageCooldown = 1f;


    // Cached variable
    Enemy enemy;
    GameObject player;
    Rigidbody2D rigidBody;
    Animator anim;
    Stats stats;
    
    bool attacking = false;
    bool attackDone = false;
    Vector3 attackDirection = new Vector3(0f, 0f, 0f);


    private void Awake()
    {
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        if (enemy.CanAttack())
        {
            if (!attacking)
            {
                attacking = true;
                StartCoroutine(StartCooldown());

            }
        }
    }

    void HandleMovement()
    {
        if (!attacking)
        {
            anim.SetFloat("Horizontal", enemy.GetMoveDirection().x);
            anim.SetFloat("Magnitude", enemy.GetMoveDirection().magnitude);
        }
    }

    private IEnumerator StartCooldown()
    {
        enemy.SetCanRun(false);
        if (player)
        {
            attackDirection.x = player.transform.position.x - transform.position.x;
            attackDirection.y = player.transform.position.y - transform.position.y;
            attackDirection.z = 0f;
            attackDirection.Normalize();
            anim.SetFloat("Horizontal", attackDirection.x);
            rigidBody.velocity = new Vector3(attackDirection.x * loungeSpeed * Time.fixedDeltaTime,
                                         attackDirection.y * loungeSpeed * Time.fixedDeltaTime, 0f);
            anim.SetBool("Attacking", true);
        }
        yield return new WaitForSeconds(attackCooldown);
        anim.SetBool("Attacking", false);
        attacking = false;
        enemy.SetCanRun(true);

    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        attackDone = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats otherStats = other.GetComponent<Stats>();
        if (otherStats is Player && !attackDone)
        {
            otherStats.DealDamage(stats.damage.value);
            StartCoroutine(DamageCooldown());
        }
    }
}
;