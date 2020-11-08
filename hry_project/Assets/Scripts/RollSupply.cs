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
            currentSlider.value = 0;
            activeRollbarCount -= 1;
        }
    }
}
