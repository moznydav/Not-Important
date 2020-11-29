using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destroyable
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject explosionVFX;

    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    AStar pathfinding;

    private bool destroyable = true;
    private float radius = 2.0f;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pathfinding = (AStar) GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
    }

    public override void OnDestroy()
    {
        if (!destroyable)
        {
            return;
        }

        destroyable = false;

        StartCoroutine(HandleDamage());
        StartCoroutine(HandleExplosion());
    }

    private IEnumerator HandleDamage()
    {
        collider.enabled = false;
        ClearMap();

        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = new Color(.0f, .0f, .0f, .0f);

        yield return new WaitForSeconds(.2f);
        var entities = FindObjectsOfType(typeof(Stats)) as Stats[];

        foreach(Stats item in entities)
        {
            if (Vector3.Distance(item.transform.position, transform.position) <= radius)
            {
                if (pathfinding.IsPathClear(item.transform.position, transform.position))
                {
                    item.DealDamage(damage);
                }
            }
        }
    }

    private IEnumerator HandleExplosion()
    {
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.65f);

        Destroy(explosion);
        base.OnDestroy();
    }
}
