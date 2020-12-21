using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [Header("Config")]
    [SerializeField] float rollSpeed = 2000f;
    [SerializeField] float projectileSpreadModifier = 5f;
    [SerializeField] float trailInterval = 1f;

    [Header("Parts")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject crossHair;
    [SerializeField] GameObject body;
    [SerializeField] GameObject legs;

    // State
    bool isRolling = false;
    bool isShooting = false;
    bool trailCooldown = false;
    // Cached variables

    public Vector2 moveDirection;
    Vector2 aimDirection;

    // Cached components
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator anim;
    PlayerStats playerStats;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProccesInputs();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        Aim();
        if (isRolling)
        {
            Roll();
        }
        else
        {
            Move();
            HandleShoot();
        }
        if (playerStats.hasPoisonTrail)
        {
            if (moveDirection.magnitude > 0 && !trailCooldown)
            {
                trailCooldown = true;
                StartCoroutine(SpawnPoisonTrail());
            }
        }
        

    }

    void ProccesInputs()
    {
        if (!isRolling)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(moveX, moveY).normalized;

            if (Input.GetButtonDown("Jump") &&
                moveDirection.magnitude > 0 &&
                playerStats.GetRollsRemaining() > 0) {
                StartRoll();
            }
        }
    }

    void StartRoll()
    {
        isRolling = true;
        anim.SetBool("Attacking", false);
        anim.SetBool("Roll", true);
        legs.GetComponent<Animator>().SetBool("Roll", true);
        playerStats.SubtractRoll();
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * playerStats.moveSpeed.value * Time.fixedDeltaTime,
                                         moveDirection.y * playerStats.moveSpeed.value * Time.fixedDeltaTime );
    }

    void HandleMovement()
    {
        // Rework the Direction changes
        // Flip attacking body towards the aim position
        anim.SetFloat("Vertical", moveDirection.y);
        anim.SetFloat("Horizontal", moveDirection.x);
        anim.SetFloat("Magnitude", moveDirection.magnitude);

        legs.GetComponent<Animator>().SetFloat("Vertical", moveDirection.y);
        legs.GetComponent<Animator>().SetFloat("Horizontal", moveDirection.x);
        legs.GetComponent<Animator>().SetFloat("Magnitude", moveDirection.magnitude);

    }

    void Roll()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * rollSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * rollSpeed * Time.fixedDeltaTime);

        //TODO: invicibility while rolling ?
    }


    private void Aim()
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x,
                                                                          Input.mousePosition.y));
        aimDirection.x = (worldMousePosition.x - transform.position.x);
        aimDirection.y = (worldMousePosition.y - transform.position.y);
        aimDirection.Normalize();
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        crossHair.transform.eulerAngles = new Vector3(0, 0, angle);

        anim.SetFloat("Aim Horizontal", aimDirection.x);
    }

    private void HandleShoot()
    {
        if (Input.GetButton("Fire1") && !isShooting)
        {

            anim.SetBool("Attacking", true);

        }
        else if(!Input.GetButton("Fire1"))
        {
            anim.SetBool("Attacking", false);
        }
    }

    private void Shoot()
    {
        if (!isShooting)
        {
            isShooting = true;
            int numOfProjectiles = playerStats.numOfProjectiles;

            Vector2 Offset = new Vector3(aimDirection.y, -aimDirection.x);
            Offset = Offset / projectileSpreadModifier;
            Vector2 shootDirection = aimDirection;

            int switchIndex = 1;
            int projectileState = 1;
            //Debug.Log("numOfProjectiles: " + numOfProjectiles);
            //Debug.Log("Offset: " + Offset);
            for (int i = 0; i < numOfProjectiles; i++)
            {
                // Debug.Log("Spawning projectile");
                // Debug.Log("Aim direction : " + aimDirection);
                if ((i - 1) % 2 == 0 && i > 1)
                {
                    projectileState++;
                }
                if(i > 0)
                {
                    //Debug.Log("state: " + projectileState + " switchIndex: " + switchIndex);
                    shootDirection = aimDirection + (Offset * projectileState * switchIndex);
                    switchIndex *= -1;
                    shootDirection.Normalize();
                }
                //Debug.Log("Shoot direction: " + shootDirection);
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<Rigidbody2D>().velocity = shootDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();
                shot.GetComponent<Projectile>().SetDamage(playerStats.damage.value);
                shot.GetComponent<Projectile>().SetDirection(shootDirection);
                shot.GetComponent<Projectile>().SetProjectileSpeed(playerStats.projectileSpeed.value);
                shot.GetComponent<Projectile>().SetPierce(playerStats.pierceValue);
                if (playerStats.hasPoison)
                {
                    shot.GetComponent<Projectile>().SetPoison(3, (int)playerStats.poisonDamage.value);
                    //playerStats.poisonTicks
                }
                if (playerStats.explodingProjectiles)
                {
                    shot.GetComponent<Projectile>().SetExplosion(playerStats.projectileExplosion, playerStats.damage.value*playerStats.explosionDamage);
                }
            }


            
            StartCoroutine(AttackCooldown());
        }



    }

    private IEnumerator AttackCooldown()
    {
        //Debug.Log("COOLDOWN START!");
        //Debug.Log("AttackSpeed: " + playerStats.attackSpeed.value);
        anim.SetBool("Attack Cooldown", true);
        yield return new WaitForSeconds(playerStats.attackSpeed.value);
        anim.SetBool("Attack Cooldown", false);
        isShooting = false;
    }

    public void StopRollAnimation()  // Used only in animation
    {
        anim.SetBool("Roll", false);
        legs.GetComponent<Animator>().SetBool("Roll", false);

    }

    public void StopRoll()
    {
        isRolling = false;
    }

    private IEnumerator SpawnPoisonTrail()
    {
        Debug.Log("SPAWNING TRAIL");
        Vector3 V3moveDirection = moveDirection;
        GameObject Trail = Instantiate(playerStats.poisonTrail, transform.position - V3moveDirection, Quaternion.identity);
        Trail.GetComponent<PoisonTrail>().SetUpTrail(playerStats.poisonDamage.value, playerStats.poisonTicks);
        yield return new WaitForSeconds(trailInterval);
        trailCooldown = false;
    }

}
