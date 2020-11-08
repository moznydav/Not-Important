using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [Header("Config")]
    [SerializeField] float rollSpeed = 2000f;

    [Header("Parts")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject crossHair;
    [SerializeField] GameObject body;
    [SerializeField] GameObject legs;

    // State
    bool isRolling = false;
    bool isShooting = false;
    // Cached variables

    Vector2 moveDirection;
    Vector2 aimDirection;

    // Cached components
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Stats stats;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
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
                stats.GetRollsRemaining() > 0) {
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
        stats.SubtractRoll();
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * stats.moveSpeed.value * Time.fixedDeltaTime,
                                         moveDirection.y * stats.moveSpeed.value * Time.fixedDeltaTime );
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
            Debug.Log("Spawning projectile");
            Debug.Log("Aim direction : " + aimDirection);
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = aimDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();
            shot.GetComponent<Projectile>().SetDamage(stats.damage.value);
            shot.GetComponent<Projectile>().SetDirection(aimDirection);
            StartCoroutine(AttackCooldown());
        }



    }

    private IEnumerator AttackCooldown()
    {

        anim.SetBool("Attack Cooldown", true);
        yield return new WaitForSeconds(stats.attackSpeed.value);
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
}
