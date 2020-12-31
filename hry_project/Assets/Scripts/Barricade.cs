using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Barricade : MonoBehaviour {
    [SerializeField] public Tilemap walls;
    [SerializeField] public GameObject lowerWalls;

    void OnEnable() {
        lowerWalls.SetActive(true);
    }
}