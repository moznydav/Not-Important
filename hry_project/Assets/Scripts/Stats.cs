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
    [SerializeField] float baseNumOfProjectiles;
    [SerializeField] float baseProjectileSpeed;
    [SerializeField] float basePierce;


    [Header("Config")]
    [SerializeField] GameObject[] VFX;
    [SerializeField] float immuneDuration = 0.2f;
    [SerializeField] GameObject[] blinkBody;
    [SerializeField] float blinkDuration = 0.1f;

    // Stats
    public CharacterStat maxHealth;
    public CharacterStat attackSpeed;
    public CharacterStat moveSpeed;
    public CharacterStat damage;
    public CharacterStat numOfProjectiles;
    public CharacterStat pierceValue;
    public CharacterStat projectileSpeed;

    [Header("For Debug")]
    [SerializeField] public float currentHealth;
    public bool isAlive = true;
    private bool immune = false;
    private bool blink = false;

    private SpriteRenderer[] spriteRenderer;

    public void SetImmune(bool isImmune) { immune = isImmune; }

    private void Start()
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
        numOfProjectiles = new CharacterStat(baseNumOfProjectiles);
        pierceValue = new CharacterStat(basePierce);
        projectileSpeed = new CharacterStat(baseProjectileSpeed);
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
            spriteRenderer[i].color = Color.white;
        }

        blink = false;
    }

    public void UpdateSpeed(StatModifier modifier) {
        moveSpeed.AddModifier(modifier);
    }
}
