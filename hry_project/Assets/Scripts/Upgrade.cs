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
        DUAL_DMG_HP, 
        DUAL_AS_MS, 
        PROJECTILESPEED_DMG,
        ADD_PROJECTILE, 
        ADD_PIERCE, 
        BASE_POISON,
        POISON_EXPLOSION, // TODO
        PROJ_EXPLOSION, // TODO
        RANGE_DAMAGE, // TODO
        CLOSE_DAMAGE, // TODO
        SPRAY_AND_PRAY, // TODO
        THORNS, // TODO
        BERSERK, // TODO
        CHAINS, //TODO - 50%dmg off but no rolls
        LUCKY_CHARM, // TODO - chance to heal to max
        DAMAGE_AURA, // TODO
        FIREWALL, // TODO - every 3s imunity for 1s
        HP_TO_DMG, // TODO - % of HP to dmg
        POISON_TRAPS, // TODO
        PARKOUR_BOOTS, // TODO
        GUPPY_TALISMAN, // TODO
        UNLIMITED_ROLLS, // TODO
        POISON_TRAIL // TODO


            
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
                playerStats.AddProjectileStat((int)value);
                playerStats.GetAttackSpeedStat().AddModifier(new StatModifier(secondaryValue));
                break;

            case UpgradeType.ADD_PIERCE: // Add pierce and decreases DMG
                playerStats.AddPierceStat((int)value);
                playerStats.GetDamageStat().AddModifier(new StatModifier(secondaryValue));
                break;
            case UpgradeType.BASE_POISON:
                playerStats.UpdatePoisonStat(value);
                break;
        }
    }
}
