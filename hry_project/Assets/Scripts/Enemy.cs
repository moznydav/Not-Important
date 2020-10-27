using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float keepDistance = 5f;
    [SerializeField] float keepDistanceError = 5f;

    AStar pathfinding;
    GameObject player;
    Vector3 moveDirection;

    SpriteRenderer spriteRenderer;

    List<Vector3> path;
    int lastPathIndex;
    bool distancing;

    Tuple<int, int> lastPlayerCell;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();

        pathfinding = (AStar) GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
    }

    void Update()
    {
        HandleMovement();
        distancing = KeepDistance();

        if (!distancing)
        {
            FollowPlayer();
        }
        else
        {
            moveDirection = new Vector3(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private bool KeepDistance()
    {
        if (keepDistance == 0.0)
        {
            return false;
        }

        var position = transform.position;
        var playerPos = player.transform.position;

        bool isPathClear = pathfinding.IsPathClear(position, playerPos);

        if (!isPathClear)
        {
            return false;
        }

        var dst = Vector3.Distance(position, playerPos);

        if (distancing)
        {
            return dst <= keepDistance + keepDistanceError;
        }

        return dst <= keepDistance;
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
            // anim.SetBool("Running", true);
        }
        else
        {
            // anim.SetBool("Running", false);
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
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
    }
}
