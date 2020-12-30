using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destroyable
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject explosionVFX;

    BoxCollider2D barrelCollider;
    SpriteRenderer spriteRenderer;
    AStar pathfinding;

    private bool destroyable = true;
    private float radius = 2.0f;

    void Awake()
    {
        barrelCollider = GetComponent<BoxCollider2D>();
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

    private bool CanHit(Vector3 pos)
    {
        Vector3 position = transform.position;
        return Vector3.Distance(pos, position) <= radius && pathfinding.IsPathClear(pos, position, true);
    }

    private IEnumerator HandleDamage()
    {
        barrelCollider.enabled = false;
        ClearMap();

        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = new Color(.0f, .0f, .0f, .0f);

        yield return new WaitForSeconds(.2f);
        var entities = FindObjectsOfType(typeof(Stats)) as Stats[];

        foreach(Stats item in entities)
        {
            if (CanHit(item.transform.position))
            {
                item.DealDamage(damage);
            }
        }

        var destroyables = FindObjectsOfType(typeof(Destroyable)) as Destroyable[];

        foreach(Destroyable item in destroyables)
        {
            if (CanHit(item.transform.position))
            {
                item.Destroy(); //destroying barrel
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
