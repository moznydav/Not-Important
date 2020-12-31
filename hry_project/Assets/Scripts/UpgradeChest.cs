using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeChest : MonoBehaviour
{
    [SerializeField] GameObject popUpScreen;
    [SerializeField] GameObject arrow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            popUpScreen.SetActive(true);
            arrow.SetActive(false);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().SetCanUpgrade(true,gameObject);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            popUpScreen.SetActive(false);
            arrow.SetActive(true);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().SetCanUpgrade(false,gameObject);
        }
    }
}
