using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] float keepDistance = 5f;
    [SerializeField] float keepDistanceError = 5f;

    AStar pathfinding;
    public GameObject player;
    public Rigidbody2D rigidBody;
    Vector3 moveDirection;
    public Stats stats;

    SpriteRenderer spriteRenderer;

    List<Vector3> path;
    int lastPathIndex;
    [SerializeField] bool distancing;
    bool canRun = true;

    Tuple<int, int> lastPlayerCell;

    void Awake()
    {
        InitializeEnemy();
    }

    public void InitializeEnemy()
    {
        stats = GetComponent<Stats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        pathfinding = (AStar)GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
        player = GameObject.FindWithTag(Constants.PLAYER_TAG);
    }

    public void HandleMovement()
    {
        if (player)
        {
            distancing = KeepDistance();
        }
        else
        {
            distancing = true;
        }

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

    private void Move()
    {
        if (canRun)
        {
            rigidBody.velocity = (moveDirection * stats.moveSpeed.value * Time.deltaTime);
        }
                   
    }

    public bool CanAttack()
    {
        return distancing;
    }


    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SetCanRun(bool canRun)
    {
        this.canRun = canRun;
    }

}
