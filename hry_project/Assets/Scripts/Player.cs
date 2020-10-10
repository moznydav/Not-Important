using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float movementSpeed = 5f;

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
        Move();
    }

    void ProccesInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
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

}
