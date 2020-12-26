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
    [SerializeField] public int explosivesLevel;
    [SerializeField] public int barricadesLevel;

    [SerializeField] Player player;

    void Awake() {
        astar = FindObjectOfType<AStar>();
        player = FindObjectOfType<Player>();
    }

    public void SetupNextLevel() {

        if (levelNumber < 7) {
            levels[levelNumber - 1].SetActive(false);
            levels[levelNumber].SetActive(true);
            levelNumber++;
            Debug.Log("Set up level " + levelNumber);

            //TODO spawners update
            GameManager.Instance.SetupNextSpawners(levels[levelNumber - 1].GetComponent<Level>().enemySpawners);
            //TODO pathfinding update
            astar.SetNewTileMap(levels[levelNumber-1].GetComponent<Level>().walls);
            //environment update
            SetUpEnvironment();

        } else {
            Debug.Log("Final level");
            //TODO level reset
            player.TeleportToMiddle();
            
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
                break;
            case 4:
                //do nothing or stronger traps
                break;
            default:
                levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel - 1].SetActive(false);
                levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel].SetActive(true);
                barricadesLevel++;
                break;
        }
    }
    public void ActivateExplosives() {
        switch (explosivesLevel) {
            case 0:
                levels[levelNumber - 1].GetComponent<Level>().explosives[explosivesLevel].SetActive(true);
                explosivesLevel++;
                break;
            case 4:
                //do nothing or stronger traps
                break;
            default:
                levels[levelNumber - 1].GetComponent<Level>().explosives[explosivesLevel - 1].SetActive(false);
                levels[levelNumber - 1].GetComponent<Level>().explosives[explosivesLevel].SetActive(true);
                explosivesLevel++;
                break;
        }
    }
    void SetUpEnvironment() {
        if (spikesLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().spikes[spikesLevel].SetActive(true);
        }
        if (tarPoolsLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().tarPools[tarPoolsLevel].SetActive(true); ;
        }
        if (barricadesLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().barricades[barricadesLevel].SetActive(true);
        }
        if (explosivesLevel > 0) {
            levels[levelNumber - 1].GetComponent<Level>().explosives[explosivesLevel].SetActive(true);
        }
    }
}
