using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeChest : MonoBehaviour
{
    [SerializeField] GameObject popUpScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            popUpScreen.SetActive(true);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().SetCanUpgrade(true);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            popUpScreen.SetActive(false);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().SetCanUpgrade(false);
        }
    }
}
