using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [Header("Config")]
    [SerializeField] float movementSpeed = 300f;
    [SerializeField] float rollSpeed = 2000f;
    [SerializeField] float attackSpeed = 5f;

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
    Vector3 aimDirection;

    // Cached components
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProccesInputs();
        HandleMovement();
    }

    private void FixedUpdate()
    {
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
        }

        if (Input.GetButtonDown("Jump") && anim.GetBool("Running"))
        {
            isRolling = true;
            anim.SetBool("Roll", true);
            ShowLegs(false);
        }

        
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * movementSpeed * Time.fixedDeltaTime );
    }

    void HandleMovement()
    {
        // Rework the Direction changes
        // Flip attacking body towards the aim position
        if (Mathf.Abs(moveDirection.x) > 0 || Mathf.Abs(moveDirection.y) > 0)
        {
            anim.SetBool("Running", true);
            legs.GetComponent<Animator>().SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
            legs.GetComponent<Animator>().SetBool("Running", false);
        }

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            legs.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if( moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
            legs.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    void Roll()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * rollSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * rollSpeed * Time.fixedDeltaTime);

        //TODO: invicibility while rolling ?
    }

   

    private void HandleShoot()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                          Input.mousePosition.y,
                                                                          0f));
        aimDirection = (worldMousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        crossHair.transform.eulerAngles = new Vector3(0, 0, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isShooting)
            {
                isShooting = true;
                anim.SetBool("Attacking", true);
                StartCoroutine(AttackCooldown());
                Debug.Log("Spawning projectile");
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<Rigidbody2D>().velocity = aimDirection * shot.GetComponent<Projectile>().GetProjectileSPeed();

            }


        }
     
    }

    public void StopShootingAnimation()
    {
        anim.SetBool("Attacking", false);
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(2f / attackSpeed);
        isShooting = false;
    }

    public void ShowLegs(bool show)
    {
        legs.SetActive(show);
    }

    public void StopRoll()  // Used only in animation
    {
        isRolling = false;
        anim.SetBool("Roll", false);
        ShowLegs(true);
    }
}
