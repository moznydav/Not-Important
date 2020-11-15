using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] float rollRegenTime;
    [SerializeField] int maxRolls;
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
}
