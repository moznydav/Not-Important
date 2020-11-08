using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    public RollSupply rollSupply;
    private int rollsRemaining;

    private void Awake() {
        
        base.InitializeStats();
        rollsRemaining = 3;
        rollSupply.InitializeRollSupply(rollsRemaining);
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
}
