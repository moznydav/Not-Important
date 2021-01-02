using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatOverview : MonoBehaviour
{
    PlayerStats playerStats;
    Player player;
    private void Awake() {
        playerStats = FindObjectOfType<PlayerStats>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable() {
        Text myText = gameObject.GetComponent<Text>();
        myText.text = "Health Points: " + playerStats.maxHealth + "\n\n" +
            "Damage: " + playerStats.damage + "\n\n" +
            "Attack Speed: " + 12/playerStats.attackSpeed + "\n\n" +
            "Movement Speed: " + playerStats.moveSpeed + "\n\n" +
            "Poison Damage: " + playerStats.poisonDamage + "\n\n\n\n\n " +
            player.offsenseUpgrades + "                 " + 
            player.defenseUpgrades + "                " + 
            player.utilityUpgrades;

    }

}
