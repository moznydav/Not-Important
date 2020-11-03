using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonoBehaviour
{
    // Config
    [Header("Config")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject legs;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float meleeDamageCooldown = 1f;


    // Cached variable
    Enemy enemy;
    GameObject player;
    Rigidbody2D rigidBody;
    Animator anim;
    Stats stats;

    [SerializeField] bool attackOnCooldown = false;
    bool attackDone = false;
    Vector3 aimDirection;


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
        HandleAim();
        HandleMovement();
        if (enemy.CanAttack())
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
        anim.SetFloat("Horizontal", enemy.GetMoveDirection().x);
        anim.SetFloat("Magnitude", enemy.GetMoveDirection().magnitude);
        legs.GetComponent<Animator>().SetFloat("Horizontal", enemy.GetMoveDirection().x);
        legs.GetComponent<Animator>().SetFloat("Magnitude", enemy.GetMoveDirection().magnitude);

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
        if (otherStats.isPlayer && !attackDone)
        {
            otherStats.DealDamage(stats.damage.value);
            StartCoroutine(MeleeDamageCooldown());
        }
    }
}
