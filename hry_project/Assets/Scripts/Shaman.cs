using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : Enemy
{
    // Config
    [Header("Config")]
    [SerializeField] float buffCooldown = 8f;
    [SerializeField] GameObject buff;
    [SerializeField] GameObject buffPlace;
    [SerializeField] float attackCooldown = 0.7f;
    [SerializeField] float damageCooldown = 1f;

    // Cached variable
    Animator anim;

    bool attacking = false;
    bool attackDone = false;
    bool buffEnabled;
    Vector3 attackDirection = new Vector3(0f, 0f, 0f);


    private void Awake()
    {
        InitializeEnemy();
        buffEnabled = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!spawning)
        {
            if (!buffEnabled)
            {
                buffEnabled = true;
                StartCoroutine(StartBuffing());
            }
            HandleMovement();
            if (!attacking)
            {
                anim.SetFloat("Horizontal", GetMoveDirection().x);
                anim.SetFloat("Magnitude", GetMoveDirection().magnitude);
                
            }
        }

    }

    private IEnumerator StartBuffing()
    {
        Debug.Log("Buffing init");
        
        while (buffEnabled)
        {
            SetCanRun(false);
            anim.SetBool("Buffing", true);
            GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
            GameObject currentBuff = Instantiate(buff, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(buffCooldown);
            //Handle animator
        }
        //BUFF
        

    }

    private void DoneBuffing()
    {
        anim.SetBool("Buffing", false);
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
            otherStats.DealDamage(stats.damage);
            StartCoroutine(DamageCooldown());
        }
    }
}
