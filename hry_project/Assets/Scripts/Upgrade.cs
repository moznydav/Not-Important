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
        MOVE_SPEED,
        DUAL_DMG_HP, //TODO
        DUAL_AS_MS, //TODO
        PROJECTILESPEED_DMG, //TODO
        ADD_PROJECTILE, //TODO
        ADD_PIERCE //TODO
            //TODO: Drawback upgrades
    }

    [SerializeField] float value = 0f;
    [SerializeField] float secondaryValue = 0f;
    [SerializeField] UpgradeType upgradeType;

    public void ApplyUpgrade()
    {
        PlayerStats playerStats = GameObject.FindWithTag(Constants.PLAYER_TAG).GetComponent<PlayerStats>();
        switch (upgradeType)
        {
            case UpgradeType.HEALTH:
                playerStats.UpdateHealthStat(value);
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

            case UpgradeType.DUAL_DMG_HP: // minor DMG and HP
                playerStats.GetDamageStat().AddModifier(new StatModifier(value));
                playerStats.UpdateHealthStat(secondaryValue);
                break;

            case UpgradeType.DUAL_AS_MS: // minor AttackSpeed and Movespeed
                playerStats.GetAttackSpeedStat().AddModifier(new StatModifier(value));
                playerStats.GetMoveSpeedStat().AddModifier(new StatModifier(secondaryValue));
                break;

            case UpgradeType.PROJECTILESPEED_DMG: // Add projectile speed and minor DMG
                playerStats.UpdateProjectileSpeed(value);
                playerStats.GetDamageStat().AddModifier(new StatModifier(secondaryValue));
                break;

            case UpgradeType.ADD_PROJECTILE: // Add projectile and decreases attackspeed
                playerStats.UpdateNumOfProjectilesStat(value);
                playerStats.GetAttackSpeedStat().AddModifier(new StatModifier(secondaryValue));
                break;

            case UpgradeType.ADD_PIERCE: // Add pierce and decreases DMG
                playerStats.UpdatePierceValueStat(value);
                playerStats.GetDamageStat().AddModifier(new StatModifier(secondaryValue));
                break;
        }
    }
}
