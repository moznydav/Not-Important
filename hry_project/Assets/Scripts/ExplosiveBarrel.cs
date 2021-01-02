using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destroyable {
    [SerializeField] float damageValue = 50f;
    [SerializeField] GameObject explosionVFX;

    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    AStar pathfinding;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] float sFXVolume;

    private bool destroyable = true;
    private float radius = 2.0f;

    void Awake() {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pathfinding = (AStar)GameObject.FindWithTag(Constants.ASTAR_TAG).GetComponent(typeof(AStar));
    }

    public override void OnDestroy() {
        if (!destroyable) {
            return;
        }

        destroyable = false;
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, sFXVolume);
        StartCoroutine(HandleDamage());
        StartCoroutine(HandleExplosion());
    }

    private bool CanHit(Vector3 pos) {
        Vector3 position = transform.position;
        return Vector3.Distance(pos, position) <= radius && pathfinding.IsPathClear(pos, position, true);
    }

    private IEnumerator HandleDamage() {
        PlayerStats playerstats = FindObjectOfType<PlayerStats>();
        
        collider.enabled = false;
        ClearMap();

        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = new Color(.0f, .0f, .0f, .0f);

        yield return new WaitForSeconds(.2f);
        var entities = FindObjectsOfType(typeof(Stats)) as Stats[];

        foreach (Stats item in entities) {
            if (CanHit(item.transform.position)) {
                PlayerStats playercheck = item.GetComponent<PlayerStats>();
                if (playerstats.hasPoisonTraps) {
                    if (playercheck) {
                        item.DealDamage(damageValue, null);
                    } else {
                        item.DealDamage(damageValue, null);
                        item.ApplyPoison(playerstats.poisonTicks, playerstats.poisonDamage);
                    }
                } else {
                    item.DealDamage(damageValue, null);
                }
            }
        }

        var destroyables = FindObjectsOfType(typeof(Destroyable)) as Destroyable[];

        foreach (Destroyable item in destroyables) {
            if (CanHit(item.transform.position)) {
                item.Destroy();
            }
        }
    }

    private IEnumerator HandleExplosion() {
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.65f);

        Destroy(explosion);
        base.OnDestroy();
    }
}
