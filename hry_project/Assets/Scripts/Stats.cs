using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //Config base stats
    [Header("Base stats")]
    [SerializeField] float baseMaxHealth;
    [SerializeField] float baseAttackSpeed;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float baseDamage;

    [Header("Config")]
    [SerializeField] GameObject VFX;
    // Stats
    public CharacterStat maxHealth;
    public CharacterStat attackSpeed;
    public CharacterStat moveSpeed;
    public CharacterStat damage;

    public bool isAlive = true;
    public bool isPlayer = false;
    [SerializeField] private float currentHealth;
    
    private void Awake()
    {
        currentHealth = baseMaxHealth; 
        maxHealth = new CharacterStat(baseMaxHealth);
        attackSpeed = new CharacterStat(baseAttackSpeed);
        moveSpeed = new CharacterStat(baseMoveSpeed);
        damage = new CharacterStat(baseDamage);
    }

    public void DealDamage(float damage)
    {
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
    }

    public void HealToMax()
    {
        currentHealth = maxHealth.value;
    }

    private IEnumerator HandleHit()
    {
        GameObject hitVFX = Instantiate(VFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(hitVFX);
    }
}
