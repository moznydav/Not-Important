using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] float rollRegenTime;
    [SerializeField] int maxRolls;
    public RollSupply rollSupply;
    private int rollsRemaining;
    private float lastRollTime;

    private void Awake() {
        lastRollTime = Time.time;
        base.InitializeStats();
        rollsRemaining = maxRolls;
        rollSupply.InitializeRollSupply(maxRolls);
    }

    private void Update()
    {
        float now = Time.time;
        if (now - lastRollTime > rollRegenTime)
        {
            if (rollsRemaining < maxRolls)
            {
                rollsRemaining += 1;
                rollSupply.RegenerateRoll();
            }
            lastRollTime = now;
        }
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
