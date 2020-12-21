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
    [SerializeField] float immuneDuration = 0.2f;
    [SerializeField] GameObject[] blinkBody;
    [SerializeField] float blinkDuration = 0.1f;

    // Stats
    public CharacterStat maxHealth;
    public CharacterStat attackSpeed;
    public CharacterStat moveSpeed;
    public CharacterStat damage;
    public CharacterStat poisonDamage;
    public float explosionDamage;
    public int numOfProjectiles;
    public int pierceValue;
    public int poisonTicks;
    public CharacterStat projectileSpeed;
    private float poisonIntervals = 1.4f;

    [Header("For Debug")]
    [SerializeField] public float currentHealth;
    public bool isAlive = true;
    private bool immune = false;
    private bool blink = false;
    private bool poisoned;
    private Color baseColor = Color.white;

    [Header("Upgrades")]
    public bool hasPoison = false;
    public bool explodingProjectiles = false;

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
        maxHealth = new CharacterStat(baseMaxHealth);
        attackSpeed = new CharacterStat(baseAttackSpeed);
        moveSpeed = new CharacterStat(baseMoveSpeed);
        damage = new CharacterStat(baseDamage);
        numOfProjectiles = baseNumOfProjectiles;
        pierceValue = basePierce;
        projectileSpeed = new CharacterStat(baseProjectileSpeed);
        poisonDamage = new CharacterStat(0);
        poisonTicks = 0;
        explosionDamage = 0.15f;
}

    public void UpdateHealthbar()
    {
        if (healthbar)
        {
            healthbar.SetHealthPercentage(currentHealth, maxHealth.value);
        }
    }

    public void DealDamage(float damage)
    {
        if (!immune)
        {
            currentHealth -= damage;
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
        if((currentHealth + heal) > maxHealth.value)
        {
            currentHealth = maxHealth.value;
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
        currentHealth = maxHealth.value;
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

    public void UpdateSpeed(StatModifier modifier) {
        moveSpeed.AddModifier(modifier);
    }

    public void TurnOnPoison(bool hasPoison)
    {
        this.hasPoison = hasPoison;
    }
}
