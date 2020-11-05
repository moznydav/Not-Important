using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Destroyable : MonoBehaviour
{
    AStar pathfinding;

    void Start()
    {
        pathfinding = (AStar) GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
        var cell = pathfinding.WorldToCell(transform.position);

        pathfinding.SetWall(cell.Item1, cell.Item2, true);
    }

    public void Destroy()
    {
        var cell = pathfinding.WorldToCell(transform.position);
        pathfinding.SetWall(cell.Item1, cell.Item2, false);

        Destroy(gameObject);
    }
}
