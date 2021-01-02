using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    PlayerStats playerStats;
    Player player;

    [SerializeField] GameObject offenseSign;
    [SerializeField] GameObject defenseSign;
    [SerializeField] GameObject utilitySign;
    Text myText;

    private void Awake() {
        playerStats = FindObjectOfType<PlayerStats>();
        player = FindObjectOfType<Player>();
        myText = gameObject.GetComponent<Text>();
    }

    private void OnEnable() {
        offenseSign.SetActive(true);
        defenseSign.SetActive(true);
        utilitySign.SetActive(true);
        myText.text = "Maximum HP: " + playerStats.maxHealth + "\n\n" +
            "Current HP: " + playerStats.currentHealth + "\n\n" + 
        "Damage: " + playerStats.damage + "\n\n" +
        "Attack Speed: " + 12/playerStats.attackSpeed + "\n\n" +
        "Movement Speed: " + playerStats.moveSpeed + "\n\n" +
        "Poison Damage: " + playerStats.poisonDamage + "\n\n" +
        "Upgrades:\n\n\n\n     " +
        player.offsenseUpgrades + "                   " + 
        player.defenseUpgrades + "                   " + 
        player.utilityUpgrades;

    }
    public void ActivateHelp() {
        offenseSign.SetActive(false);
        defenseSign.SetActive(false);
        utilitySign.SetActive(false);

        myText.text = "Controls:\n" +
            "WSAD to move\n" +
            "Spacebar to roll\n" +
            "Mouse to aim\n" +
            "Mouseclick to shoot\n" +
            "E to pick up chests\n\n" +
            "Goal:\n" +
            "Kill enemies in waves to earn upgrades\n" +
            "Make the arena your own with environment changes\n" +
            "Survive as long as you can\n\n" +
            "Good luck, you will need it";
    }

}
