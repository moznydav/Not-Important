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

    [Header("Config")]
    [SerializeField] GameObject VFX;
    [SerializeField] float immuneDuration = 0.2f;
    // Stats
    public CharacterStat maxHealth;
    public CharacterStat attackSpeed;
    public CharacterStat moveSpeed;
    public CharacterStat damage;

    
    [Header("For Debug")]
    [SerializeField] private float currentHealth;
    public bool isAlive = true;
    private bool immune = false;

    public void SetImmune(bool isImmune) { immune = isImmune; }

    private void Awake()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        currentHealth = baseMaxHealth; 
        maxHealth = new CharacterStat(baseMaxHealth);
        attackSpeed = new CharacterStat(baseAttackSpeed);
        moveSpeed = new CharacterStat(baseMoveSpeed);
        damage = new CharacterStat(baseDamage);
    }

    private void UpdateHealthbar()
    {
        if (healthbar)
        {
            healthbar.SetHealthPercentage(currentHealth, baseMaxHealth);
        }
    }

    public void DealDamage(float damage)
    {
        if (!immune)
        {
            print("took damage");
            currentHealth -= damage;
            StartCoroutine(HandleHit());
            if (currentHealth <= 0)
            {
                isAlive = false;
                Destroy(gameObject);
                //Change this
                //Add animations
            }
            Debug.Log(gameObject.name + " health reduced to " + currentHealth);
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
        GameObject hitVFX = Instantiate(VFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(hitVFX);
    }

    public IEnumerator StartImmuneFrames()
    {
        yield return new WaitForSeconds(immuneDuration);
        immune = false;
    }
}
