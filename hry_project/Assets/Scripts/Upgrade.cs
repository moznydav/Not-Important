using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    enum UpgradeType
    {
        HEALTH,
        DAMAGE,
        ATTACK_SPEED,
        MOVE_SPEED
    }

    [SerializeField] float value = 0f;
    [SerializeField] UpgradeType upgradeType;

    public void ApplyUpgrade()
    {
        PlayerStats playerStats = GameObject.FindWithTag(Constants.PLAYER_TAG).GetComponent<PlayerStats>();
        switch (upgradeType)
        {
            case UpgradeType.HEALTH:
                playerStats.GetHealthStat().AddModifier(new StatModifier(value));
                break;

            case UpgradeType.DAMAGE:
                playerStats.GetDamageStat().AddModifier(new StatModifier(value));
                break;

            case UpgradeType.ATTACK_SPEED:
                playerStats.GetAttackSpeedStat().AddModifier(new StatModifier(value));
                break;

            case UpgradeType.MOVE_SPEED:
                playerStats.GetMoveSpeedStat().AddModifier(new StatModifier(value));
                break;
        }
    }
}
