using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int levelNumber = 1;
    [SerializeField] int waveNumber;
    [SerializeField] List<GameObject> levels;
    [SerializeField] AStar astar;

    [SerializeField] public int spikesLevel;
    [SerializeField] public int tarPoolsLevel;
    [SerializeField] GameObject[] explosives;
    [SerializeField] public int barricadesLevel;

    [SerializeField] Player player;

    private bool explosivesFirst = true;

    void Awake() {
        astar = FindObjectOfType<AStar>();
        player = FindObjectOfType<Player>();
    }

    public void SetupNextLevel() {

        if (levelNumber < 8) {
            levels[levelNumber - 1].SetActive(false);
            levels[levelNumber].SetActive(true);
            levelNumber++;
            Debug.Log("Set up level " + levelNumber);
            LevelUpdate(levelNumber);

        } else {
            Debug.Log("Reseting level");
            //TODO level reset
            player.TeleportToMiddle();
            levelNumber = 0;
            LevelUpdate(levelNumber);
            
        }
    }

    void LevelUpdate(int levelNumber) {
        //TODO spawners update
        GameManager.Instance.SetupNextSpawners(levels[levelNumber - 1].GetComponent<Level>().enemySpawners);
        //TODO pathfinding update
        UpdatePathfinding(levelNumber);
        
        //environment update
        SetUpEnvironment(levelNumber);
    }

    void UpdatePathfinding(int levelNumber) {
        if (barricadesLevel == 0) {
            astar.SetNewTileMap(levels[levelNumber - 1].GetComponent<Level>().walls);
        } else {
            astar.SetNewTileMap(levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel-1].walls);
        }
    }

    public void ActivateSpikes() {
        switch (spikesLevel) {
            case 0:
                levels[levelNumber - 1].GetComponent<Level>().spikes[spikesLevel].SetActive(true);
                spikesLevel++;
                break;
            case 4:
                //do nothing or stronger traps
                break;
            default:
                levels[levelNumber - 1].GetComponent<Level>().spikes[spikesLevel - 1].SetActive(false);
                levels[levelNumber - 1].GetComponent<Level>().spikes[spikesLevel].SetActive(true);
                spikesLevel++;
                break;
        }
    }
    public void ActivateTarPools() {
        switch (tarPoolsLevel) {
            case 0:
                levels[levelNumber - 1].GetComponent<Level>().tarPools[tarPoolsLevel].SetActive(true);
                tarPoolsLevel++;
                break;
            case 4:
                //do nothing or stronger traps
                break;
            default:
                levels[levelNumber - 1].GetComponent<Level>().tarPools[tarPoolsLevel - 1].SetActive(false);
                levels[levelNumber - 1].GetComponent<Level>().tarPools[tarPoolsLevel].SetActive(true);
                tarPoolsLevel++;
                break;
        }
    }
    public void ActivateBarricades() {
        switch (barricadesLevel) {
            case 0:
                levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel].SetActive(true);
                barricadesLevel++;
                UpdatePathfinding(levelNumber);
                break;
            case 4:
                //do nothing or stronger traps
                break;
            default:
                levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel - 1].SetActive(false);
                levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel].SetActive(true);
                barricadesLevel++;
                UpdatePathfinding(levelNumber);
                break;
        
        }
    }
    public void ActivateExplosives() {
        if (explosivesFirst) {
            explosives[0].SetActive(true);
            explosivesFirst = false;
        } else {
            for (int i = 0; i < explosives.Length; i++) {
                Debug.Log("Explosives change");
                if (explosives[i].activeSelf) {
                    explosives[i].SetActive(false);
                    explosives[levelNumber-1].SetActive(true);
                    break;
                }
            }
        }

        
    }
    void SetUpEnvironment(int levelNumber) {
        if (spikesLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().spikes[spikesLevel].SetActive(true);
        }
        if (tarPoolsLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().tarPools[tarPoolsLevel].SetActive(true); ;
        }
        if (barricadesLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel].SetActive(true);
            UpdatePathfinding(levelNumber);
        }
    }
}
