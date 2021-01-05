﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetHealthPercentage(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }
}
