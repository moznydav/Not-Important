using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollSupply : MonoBehaviour
{
    public GameObject rollbar;
    private int rollbarCount;
    private int activeRollbarCount;
    private List<GameObject> rollbarList;

    public void InitializeRollSupply(int count)
    {
        rollbarCount = count;
        activeRollbarCount = count;
        rollbarList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject currRollbar = Instantiate(rollbar, transform);
            currRollbar.transform.Translate(new Vector3(i - 1, 0, 0));
            rollbarList.Add(currRollbar);
        }
    }

    public void SubtractRoll()
    {
        if (activeRollbarCount > 0)
        {
            Slider currentSlider = (Slider) rollbarList[activeRollbarCount - 1].GetComponent("Slider");
            if (activeRollbarCount < rollbarCount)
            {
                Slider lastSlider = (Slider)rollbarList[activeRollbarCount].GetComponent("Slider");
                lastSlider.value = 0;
            }
            currentSlider.value = 0;
            activeRollbarCount -= 1;
        }
    }

    public void RegenerateRoll()
    {
        if (activeRollbarCount < rollbarCount)
        {
            activeRollbarCount += 1;
            Slider currentSlider = (Slider)rollbarList[activeRollbarCount - 1].GetComponent("Slider");
            currentSlider.value = 1;
        }
    }

    public void UpdateRollRegen(float timePassed, float timeNeeded)
    {
        if (activeRollbarCount < rollbarCount)
        {
            Slider currentSlider = (Slider)rollbarList[activeRollbarCount].GetComponent("Slider");
            currentSlider.value = timePassed / timeNeeded;
        }
    }
}
