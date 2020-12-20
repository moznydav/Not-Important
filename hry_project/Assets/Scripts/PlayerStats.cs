﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] float rollRegenTime;
    [SerializeField] int maxRolls;
    [SerializeField] int poisonTicks;
    public RollSupply rollSupply;
    private int rollsRemaining;
    private float lastRegenTime;

    private void Awake() {
        lastRegenTime = Time.time;
        base.InitializeStats();
        rollsRemaining = maxRolls;
        rollSupply.InitializeRollSupply(maxRolls);
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

    public void DealDamage(float damage)
    {
        base.DealDamage(damage);
        SetImmune(true);
        StartCoroutine(StartImmuneFrames());
    }

    public void UpdateHealthStat(float value)
    {
        maxHealth.AddModifier(new StatModifier(value));
        base.currentHealth += value;
        base.UpdateHealthbar();
    }
    public CharacterStat GetDamageStat()
    {
        return damage;
    }
    public CharacterStat GetAttackSpeedStat()
    {
        return attackSpeed;
    }
    public CharacterStat GetMoveSpeedStat()
    {
        return moveSpeed;
    }

    public void AddPierceStat(int value)
    {
        pierceValue += value;
    }
    public void AddProjectileStat(int value)
    {
        numOfProjectiles += value;
    }
    public CharacterStat GetProjectileSpeedStat()
    {
        return projectileSpeed;
    }

    public void UpdateProjectileSpeed(float value)
    {
        projectileSpeed.AddModifier(new StatModifier(value));
    }

    public void UpdatePoisonStat(float value)
    {
        if (!base.hasPoison)
        {
            base.hasPoison = true;
            base.poisonTicks = poisonTicks;
        }
        base.poisonDamage.AddModifier(new StatModifier(value));
    }

}
