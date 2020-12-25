using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public Healthbar healthbar;

    //Config base stats
    [Header("Base stats")]
    [SerializeField] float baseMaxHealth;
    [SerializeField] float baseAttackSpeed;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float baseDamage;
    [SerializeField] int baseNumOfProjectiles;
    [SerializeField] float baseProjectileSpeed;
    [SerializeField] int basePierce;


    [Header("Config")]
    [SerializeField] GameObject[] VFX;
    [SerializeField] public GameObject projectileExplosion;
    [SerializeField] public GameObject poisonTrail;
    [SerializeField] float immuneDuration = 0.2f;
    [SerializeField] GameObject[] blinkBody;
    [SerializeField] float blinkDuration = 0.1f;

    // Stats
    public float maxHealth;
    public float attackSpeed;
    public float moveSpeed;
    public float damage;
    public float poisonDamage;
    public float explosionDamage;
    public int numOfProjectiles;
    public int pierceValue;
    public int ricochetValue;
    public int poisonTicks;
    public float projectileSpeed;
    private float poisonIntervals = 1.4f;
    public float thornsDamage;
    public float berserkMultiplier = 1f;
    private float chainsMultiplier = 1f;
    private float fireWallInterval;
    private float HpToDmgMultiplier;

    [Header("For Debug")]
    [SerializeField] public float currentHealth;
    public bool isAlive = true;
    private bool immune = false;
    private bool fireWallImmune = false;
    private bool blink = false;
    private bool poisoned;
    private Color baseColor = Color.white;

    [Header("Upgrades")]
    public bool hasPoison = false;
    public bool explodingProjectiles = false;
    public bool hasPoisonTrail = false;
    public bool hasBrokenScope = false; // maybe switch to % of dmg
    public bool hasSniperScope = false; // maybe switch to % of dmg
    public bool hasSprayAndPray = false;
    public bool hasThorns = false;
    public bool hasBerserk = false;
    public bool hasChains = false; // 50% of dmg off, no rolls
    public bool hasFireWall = false; // immunity every 3s
    public bool hasHpToDmg = false;
    public bool hasRicochet = false;

    private SpriteRenderer[] spriteRenderer;

    public void SetImmune(bool isImmune) { immune = isImmune; }

    private void Awake()
    {
        spriteRenderer = new SpriteRenderer[blinkBody.Length];

        for (int i = 0; i < blinkBody.Length; i++)
        {
            spriteRenderer[i] = blinkBody[i].GetComponent<SpriteRenderer>();
        }

        InitializeStats();
    }

    public void InitializeStats()
    {
        currentHealth = baseMaxHealth;
        maxHealth = baseMaxHealth;
        attackSpeed = baseAttackSpeed;
        moveSpeed = baseMoveSpeed;
        damage = baseDamage;
        numOfProjectiles = baseNumOfProjectiles;
        pierceValue = basePierce;
        projectileSpeed = baseProjectileSpeed;
        poisonDamage = 0;
        poisonTicks = 0;
        ricochetValue = 0;
        explosionDamage = 0.15f;
}

    public void UpdateHealthbar()
    {
        if (healthbar)
        {
            healthbar.SetHealthPercentage(currentHealth, maxHealth);
        }
    }

    public void DealDamage(float damage)
    {
        if (!immune && !fireWallImmune)
        {
            if (hasChains)
            {
                currentHealth -= damage * chainsMultiplier ;
            }
            else
            {
                currentHealth -= damage;
            }
            
            StartCoroutine(HandleHit());

            if (!blink)
            {
                blink = true;
                StartCoroutine(Flash());
            }

            if (currentHealth <= 0)
            {
                isAlive = false;
                Destroy(gameObject);
                //GameManager.Instance.EnemyKilled();
                //Change this
                //Add animations
            }
            // Debug.Log(gameObject.name + " health reduced to " + currentHealth);
        }
        UpdateHealthbar();
    }

    public void Heal(float heal)
    {
        if((currentHealth + heal) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += heal;
        }
        UpdateHealthbar();
    }

    public void ApplyPoison(int numOfTicks, float damage)
    {
        if (!poisoned)
        {
            poisoned = true;
            StartCoroutine(HandlePoison(numOfTicks, damage));
        }
    }

    private IEnumerator HandlePoison(int ticks, float damage)
    {
        //Debug.Log("INIT POISON");
        updateSpriteColor(Color.green);
        for(int  i = 0; i < ticks;  i++)
        {
            yield return new WaitForSeconds(poisonIntervals);
           // Debug.Log("POISON TICK");
            DealDamage(damage);
        }
        updateSpriteColor(Color.white);
        poisoned = false;
    }
    public void HealToMax()
    {
        currentHealth = maxHealth;
        UpdateHealthbar();
    }

    private IEnumerator HandleHit()
    {
        GameObject selectedVFX = VFX[Random.Range(0,VFX.Length - 1)];
        GameObject hitVFX = Instantiate(selectedVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Animator soonToBeDestroyed = hitVFX.GetComponent<Animator>();
        Destroy(soonToBeDestroyed);
    }

    public IEnumerator StartImmuneFrames()
    {
        yield return new WaitForSeconds(immuneDuration);
        immune = false;
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = Color.red;
        }

        yield return new WaitForSeconds(blinkDuration);

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = baseColor;
        }

        blink = false;
    }

    private void updateSpriteColor(Color color)
    {
        baseColor = color;

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = Color.white;
        }

    }

    public void TurnOnPoison(bool hasPoison)
    {
        this.hasPoison = hasPoison;
    }

    public void UpdateThorns(float value)
    {
        thornsDamage += value;
        hasThorns = true;               
    }

    public void UpdateBerserk(float value)
    {
        berserkMultiplier += value;
        hasBerserk = true;
    }

    public void UpdateChains()
    {
        chainsMultiplier /= 2;
        hasChains = true;
    }

    public void UpdateFireWall(float value)
    {
        if (hasFireWall)
        {
            fireWallInterval += value;
        }
        else
        {
            hasFireWall = true;
            fireWallInterval = 1f;
            StartCoroutine(HandleFireWall());
        }
    } 

    private IEnumerator HandleFireWall()
    {
        while (hasFireWall)
        {
            yield return new WaitForSeconds(3f);
            fireWallImmune = true;
            yield return new WaitForSeconds(fireWallInterval);
        }
        

    }

    public void UpdateRicochet()
    {
        ricochetValue++;
        hasRicochet = true;
    }

    public void UpdateHpToDmg(float value)
    {
        if (hasHpToDmg)
        {
            HpToDmgMultiplier += value;
        }
        else
        {
            hasHpToDmg = true;
            HpToDmgMultiplier = 0.2f;
        }
    }

    //TODO use this function everytime someone attacks
    public float GetDamage()
    {
        float retDamage = damage;
        if (hasHpToDmg)
        {
            retDamage += maxHealth * HpToDmgMultiplier;
        }
        if (hasBerserk && currentHealth <= (0.3 * maxHealth))
        {
            retDamage *= berserkMultiplier;
        }
        return retDamage;
    }
}
