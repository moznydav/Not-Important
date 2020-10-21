using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 20f;



    private void Update()
    {
        StartCoroutine(HandleLifeTime());
    }



    public float GetProjectileSPeed()
    {
        return projectileSpeed;
    }


    private IEnumerator HandleLifeTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
