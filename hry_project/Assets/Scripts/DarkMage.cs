using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMage : EnemyShooter
{
    // Config
    [Header("Config Dark Mage")]
    [SerializeField] GameObject[] guns;
    [SerializeField] GameObject gunPrefab;
    [SerializeField] float timeBetweenShots = 0.6f;
    [SerializeField] bool attacking;

    private void Awake()
    {
        attacking = false;
        InitializeEnemy();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAim();
        if (!attacking)
        {
            base.HandleMovement();
        }
        
        if (base.CanAttack())
        {
            if (!base.attackOnCooldown)
            {
                attacking = true;
                attackOnCooldown = true;
                anim.SetBool("Attacking", true);
                //for(int i = 0; i < guns.Length; i++)
                //{
                //    guns[i].GetComponent<Animator>().SetBool("Attacking", true);
                //}
                StartCoroutine(ShootingCooldown());

            }
        } 
    }

    private void Shoot() // used by animation
    {
        StartCoroutine(ShootProjectiles());

        
    }

    private IEnumerator ShootProjectiles()
    {
        
        for (int i = 0; i < guns.Length; i++)
        {
            GameObject gun = Instantiate(gunPrefab, guns[i].transform.position, Quaternion.identity);
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            GameObject shot = Instantiate(projectile, guns[i].transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = aimDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();
            shot.GetComponent<Projectile>().SetDamage(stats.damage);
            shot.GetComponent<Projectile>().SetDirection(aimDirection);
            shot.GetComponent<Projectile>().SetOrigin(gameObject);
            shot.transform.eulerAngles = new Vector3(0, 0, angle);
            yield return new WaitForSeconds(timeBetweenShots);
            Destroy(gun);
        }

        anim.SetBool("Attacking", false);

        //    for (int i = 0; i < guns.Length; i++)
        //    {
        //        guns[i].GetComponent<Animator>().SetBool("Attacking", false);
        //    }
        attacking = false;
    }

}