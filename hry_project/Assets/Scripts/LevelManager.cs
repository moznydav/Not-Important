using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int levelNumber = 1;
    [SerializeField] List<GameObject> levels;
    [SerializeField] AStar astar;

    void Awake() {
        astar = FindObjectOfType<AStar>();
    } 

    public void SetupNextLevel() {
        levels[levelNumber - 1].SetActive(false);
        levels[levelNumber].SetActive(true);

        Debug.Log("Set up level " + levelNumber);

        //TODO spawners update
        //TODO pathfinding update
        astar.SetNewTileMap(levels[levelNumber].GetComponent<Level>().tilemap);


        if(levelNumber <= 7) {
            levelNumber++;
        } else {
            Debug.Log("Final level");
            //TODO level reset
        }
    }
}
