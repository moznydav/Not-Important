using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float movementSpeed = 300f;
    [SerializeField] float rollSpeed = 2000f;

    // State
    bool rolling = false;

    // Cached variables

    Vector2 moveDirection;

    // Cached components
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (rolling)
        {
            Roll();
        }
        else
        {
            Move();
        }

    }

    void ProccesInputs()
    {
        if (!rolling)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(moveX, moveY).normalized;
        }

        if (Input.GetButtonDown("Jump") && anim.GetBool("Running"))
        {
            rolling = true;
            anim.SetBool("Roll", true);
        }

    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * movementSpeed * Time.fixedDeltaTime );
    }

    void HandleMovement()
    {
        if (Mathf.Abs(moveDirection.x) > 0 || Mathf.Abs(moveDirection.y) > 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if( moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    void Roll()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * rollSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * rollSpeed * Time.fixedDeltaTime);

        //TODO: invicibility while rolling ?
    }

    public void StopRoll()  // Used only in animation
    {
        rolling = false;
        anim.SetBool("Roll", false);
    }
}
