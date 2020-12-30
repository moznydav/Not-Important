using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveIn : MonoBehaviour
{
    public void UpdateTime(float time)
    {
        if (time > 0) {
            Text myText = gameObject.GetComponent<Text>();
            myText.text = "Next wave in: " + System.Math.Round(time);
        }
    }
}
