using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashbars : MonoBehaviour
{
    public GameObject dashbar;
    private int dasbarCount;

    public void InitializeDashbars(int count)
    {
        Instantiate(dashbar, transform);
    }
}
