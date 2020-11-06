using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashbars : MonoBehaviour
{
    public GameObject dashbar;
    private int dashbarCount;
    private int activeDashbarCount;
    private List<GameObject> dashbarList;

    public void InitializeDashbars(int count)
    {
        dashbarCount = count;
        activeDashbarCount = count;
        dashbarList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject currDashbar = Instantiate(dashbar, transform);
            currDashbar.transform.Translate(new Vector3(i - 1, 0, 0));
            dashbarList.Add(currDashbar);
        }
    }

    public void SubtractRoll()
    {
        if (activeDashbarCount > 0)
        {
            Slider currentSlider = (Slider) dashbarList[activeDashbarCount - 1].GetComponent("Slider");
            currentSlider.value = 0;
            activeDashbarCount -= 1;
        }
    }
}
