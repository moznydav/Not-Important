using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashbars : MonoBehaviour
{
    public GameObject dashbar;
    private int dasbarCount;
    private List<GameObject> dashbarList;

    public void InitializeDashbars(int count)
    {
        dashbarList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject currDashbar = Instantiate(dashbar, transform);
            currDashbar.transform.Translate(new Vector3(i - 1, 0, 0));
            dashbarList.Add(currDashbar);
        }
    }
}
