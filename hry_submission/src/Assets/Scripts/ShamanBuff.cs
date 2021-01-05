using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanBuff : MonoBehaviour
{
    [SerializeField] float movementBuff = 100f;
    [SerializeField] float buffDuration = 3f;
    GameObject shaman;
    Collider2D buffZone;

    private void Awake()
    {
        buffZone = GetComponent<Collider2D>();
        buffZone.enabled = false;
    }

    private void EnableBuffZone()
    {
        buffZone.enabled = true;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

    public void SetShaman(GameObject shaman)
    {
        this.shaman = shaman;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats stats = other.GetComponent<Stats>();
        if (stats)
        {
            if (!stats.isBuffed)
            {
                stats.isBuffed = true;
                stats.SetBuffed(movementBuff, buffDuration);
            }
        }
    }

    
}
