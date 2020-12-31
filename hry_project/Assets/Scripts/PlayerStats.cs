using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [Header("PlayerStats part")]
    [SerializeField]public float rollRegenTime;
    [SerializeField] int maxRolls;
    
    public RollSupply rollSupply;
    private int rollsRemaining;
    private float lastRegenTime;

    //Upgrade counters
    public int offenceCounter;
    public int defenceCounter;
    public int utilityCounter;

    private void Awake() {
        lastRegenTime = Time.time;
        base.InitializeStats();
        rollsRemaining = maxRolls;
        rollSupply.InitializeRollSupply(maxRolls);
        offenceCounter = 0;
        DefenceCounter = 0;
        UtilityCounter = 0;
    }

    private void HandleRollRegen()
    {
        float now = Time.time;
        if (rollsRemaining < maxRolls)
        {
            if (now - lastRegenTime > rollRegenTime)
            {
                rollsRemaining += 1;
                rollSupply.RegenerateRoll();
                lastRegenTime = now;
            }
            else
            {
                rollSupply.UpdateRollRegen(now - lastRegenTime, rollRegenTime);
            }
        }
    }

    private void Update()
    {
        HandleRollRegen();
        
    }

    public int GetRollsRemaining() { return rollsRemaining; }

    public void SubtractRoll()
    {
        if (rollsRemaining > 0)
        {
            if (rollSupply)
            {
                rollSupply.SubtractRoll();
            }
            rollsRemaining -= 1;
        }
    }

    public void DealDamage(float damage, Stats origin)
    {
        base.DealDamage(damage, origin);
        SetImmune(true);
        StartCoroutine(StartImmuneFrames());
        
    }

    public void UpdateHealthStat(float value)
    {
        maxHealth += value;
        base.currentHealth += value;
        base.UpdateHealthbar();
    }

    public void AddPierceStat(int value)
    {
        pierceValue += value;
    }
    public void AddProjectileStat(int value)
    {
        numOfProjectiles += value;
    }

    public void UpdatePoisonStat(float value)
    {
        if (!base.hasPoison)
        {
            base.hasPoison = true;
            base.poisonTicks = poisonTicks;
        }
        base.poisonDamage += value;
            //AddModifier(new StatModifier(value));
    }

    public void updateCorpseExplosion(float value)
    {
        hasCorpseExplosion = true;
        poisonDamage += value;
        poisonTicks = 3;
    }

    public void UpdateExplodingProjectile(float damage)
    {
        base.explodingProjectiles = true;
        base.explosionDamage += damage;

    }

    public void UpdateChains()
    {
        chainsMultiplier /= 2;
        hasChains = true;
        rollSupply.gameObject.SetActive(false);      
    }

    public void UpdateUnlimitedRolls()
    {
        if (!hasUnlimitedRolls)
        {
            hasUnlimitedRolls = true;
            rollRegenTime = 0.1f;
            unlimitedRollsMultiplier = 2f;
        }
        else
        {
            unlimitedRollsMultiplier *= 0.7f;
        }
    }

}
