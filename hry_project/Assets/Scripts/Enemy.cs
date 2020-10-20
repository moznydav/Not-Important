using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed = 200f;

    AStar pathfinding;
    GameObject player;
    Vector2 moveDirection;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator anim;

    List<Vector3> path;
    int lastPathIndex;

    Tuple<int, int> lastPlayerCell;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        pathfinding = (AStar) GameObject.FindWithTag("PathFinding").GetComponent(typeof(AStar));
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        FollowPlayer();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void FollowPlayer()
    {
        var playerCell = pathfinding.WorldToCell(player.transform.position);

        /* Player stands still, no need to recalc */
        if (lastPlayerCell != null && Utils.AreTuplesSame(lastPlayerCell, playerCell))
        {
            var enemyCell = pathfinding.WorldToCell(transform.position);

            for (int i = lastPathIndex; i >= 0; i--)
            {
                var nextCell = pathfinding.WorldToCell(path[i]);

                if (!Utils.AreTuplesSame(enemyCell, nextCell))
                {
                    moveDirection = (path[i] - transform.position).normalized;
                    lastPathIndex = i;
                    return;
                }
            }

            lastPathIndex = 0;
            moveDirection = new Vector3(0, 0, 0);
            return;
        }

        /* recalc path */
        lastPlayerCell = playerCell;
        path = pathfinding.GetPath(transform.position, player.transform.position);

        if (path.Count != 0)
        {
            lastPathIndex = path.Count - 1;
            moveDirection = (path[path.Count - 1] - transform.position).normalized;
        }
        else
        {
            lastPathIndex = 0;
            moveDirection = new Vector3(0, 0, 0);
        }
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

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * movementSpeed * Time.fixedDeltaTime);
    }
}
