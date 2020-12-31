using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Barricade : MonoBehaviour {
    [SerializeField] public Tilemap walls;
    [SerializeField] public GameObject lowerWalls;
    [SerializeField] public GameObject activator;

    private void OnEnable() {
        lowerWalls.SetActive(true);
    }
    
    public void SetActive(bool boolean) {
        activator.SetActive(boolean);
    }

}