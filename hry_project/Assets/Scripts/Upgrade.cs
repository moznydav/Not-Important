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
        DUAL_DMG_ATKSPEED,
        DUAL_MS_PROJETILESPEED,
        DUAL_PROJECTILESPEED_ATTACKSPEED,
        DUAL_DMG_HP, 
        DUAL_AS_MS, 
        PROJECTILESPEED_DMG,
        ADD_PROJECTILE, 
        ADD_PIERCE, 
        BASE_POISON,
        RICOCHET,
        POISON_EXPLOSION, // TODO
        PROJ_EXPLOSION, // add class bonus
        SNIPER_SCOPE, 
        BROKEN_SCOPE, 
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
        POISON_TRAIL 


            
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
                playerStats.damage += value;
                break;

            case UpgradeType.ATTACK_SPEED:
                playerStats.attackSpeed += value;
                break;

            case UpgradeType.MOVE_SPEED:
                playerStats.moveSpeed += value;
                break;

            case UpgradeType.DUAL_DMG_ATKSPEED:
                playerStats.damage += value;
                playerStats.attackSpeed += secondaryValue;
                break;

            case UpgradeType.DUAL_MS_PROJETILESPEED:
                playerStats.moveSpeed += value;
                playerStats.projectileSpeed += secondaryValue;
                break;

            case UpgradeType.DUAL_PROJECTILESPEED_ATTACKSPEED:
                playerStats.projectileSpeed += value;
                playerStats.attackSpeed += secondaryValue;
                break;

            case UpgradeType.DUAL_DMG_HP: // minor DMG and HP
                playerStats.damage += value;
                playerStats.UpdateHealthStat(secondaryValue);
                break;

            case UpgradeType.DUAL_AS_MS: // minor AttackSpeed and Movespeed
                playerStats.attackSpeed += value;
                playerStats.moveSpeed += secondaryValue;
                break;

            case UpgradeType.PROJECTILESPEED_DMG: // Add projectile speed
                playerStats.projectileSpeed += value;
                break;

            case UpgradeType.ADD_PROJECTILE: // Add projectile and decreases attackspeed
                playerStats.AddProjectileStat((int)value);
                playerStats.attackSpeed += secondaryValue;
                break;

            case UpgradeType.ADD_PIERCE: // Add pierce and decreases DMG
                playerStats.AddPierceStat((int)value);
                playerStats.damage += secondaryValue;
                break;
            case UpgradeType.BASE_POISON:
                playerStats.UpdatePoisonStat(value);
                break;
            case UpgradeType.PROJ_EXPLOSION:
                playerStats.UpdateExplodingProjectile(value);
                break;
            case UpgradeType.POISON_TRAIL:
                playerStats.hasPoisonTrail = true;
                playerStats.UpdatePoisonStat(value);
                break;
            case UpgradeType.BROKEN_SCOPE:
                playerStats.damage += value;
                playerStats.hasBrokenScope = true;
                break;
            case UpgradeType.SNIPER_SCOPE:
                playerStats.damage += value;
                playerStats.hasSniperScope = true;
                break;
            case UpgradeType.SPRAY_AND_PRAY:
                playerStats.attackSpeed += value;
                playerStats.hasSprayAndPray = true;
                break;
            case UpgradeType.THORNS:
                playerStats.UpdateThorns(value);
                break;
            case UpgradeType.BERSERK:
                playerStats.UpdateBerserk(value);
                break;
            case UpgradeType.CHAINS:
                playerStats.UpdateChains();
                break;
            case UpgradeType.FIREWALL:
                playerStats.UpdateFireWall(value);
                break;
            case UpgradeType.HP_TO_DMG:
                playerStats.UpdateHpToDmg(value);
                break;
            case UpgradeType.RICOCHET:
                playerStats.UpdateRicochet();
                break;
            case UpgradeType.LUCKY_CHARM:
                playerStats.UpdateDumbLuck();
                break;
            case UpgradeType.DAMAGE_AURA:
                playerStats.UpdateDamageAura(value);
                break;
        }
    }
}
