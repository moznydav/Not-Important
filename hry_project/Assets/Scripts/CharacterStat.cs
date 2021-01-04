using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat
{
    public float baseValue;

    public float value { 
        get 
        {
            if (valueChanged)
            {
                latestValue = CalculateValue();
                valueChanged = false;
            }
            return latestValue;
        } 
                        }

    private bool valueChanged = true;
    private float latestValue;

    private readonly List<StatModifier> statModifiers;

    public CharacterStat(float baseValue)
    {
        this.baseValue = baseValue;
        statModifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier mod)
    {
        valueChanged = true;
        statModifiers.Add(mod);
    }

    public bool RemoveModifier(StatModifier mod)
    {
        valueChanged = true;
        return statModifiers.Remove(mod);
    }

    private float CalculateValue()
    {
        float finalValue = baseValue;

        for(int i = 0; i < statModifiers.Count; i++)
        {
            finalValue += statModifiers[i].value;
        }

        return finalValue;
    }
}
