 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movementSpeed = 100f;

    AStar pathfinding;
    GameObject player;
    Rigidbody2D rigidBody;
    Vector2 moveDirection;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        pathfinding = (AStar) GameObject.FindWithTag("PathFinding").GetComponent(typeof(AStar));
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void FollowPlayer()
    {
        var path = pathfinding.GetPath(transform.position, player.transform.position);

        if (path.Count != 0)
        {
            Vector3 nextPos = path[path.Count - 1];
            moveDirection = (nextPos - transform.position).normalized;
        }
        else
        {
            moveDirection = new Vector3(0, 0, 0);
        }
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed * Time.fixedDeltaTime,
                                         moveDirection.y * movementSpeed * Time.fixedDeltaTime );
    }
}
