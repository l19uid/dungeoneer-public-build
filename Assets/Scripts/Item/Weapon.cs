using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Dungeoneer
{
    public class Weapon : Item
    {
        [Header("Weapon Stats")] public Enums.WeaponType weaponType;
        public Enums.WeaponFamily weaponFamily;

        [Tooltip(
            "Time it takes to attack again, but also to stop and begin being able to move again. (1/attackDuration)")]
        public float attackDuration;

        [Tooltip("The speed of the attacking animation. (100+attackSpeed)/100")]
        public float attackSpeed = 1;

        public bool isChargeable;
        public float maxChargeTime;
        [Tooltip("Arrows, Bullets, etc.")] public GameObject projectile;
        [Header("Weapon Animations")] public AnimationClip[] animations;
        [Header("Effect")][SerializeField]private Effect weaponEffect;
        
        public float GetAttackDuration()
        {
            return attackDuration;
        }

        public AnimationClip[] GetAnimations()
        {
            return animations;
        }

        public float GetMaxChargeTime()
        {
            return maxChargeTime;
        }

        public bool GetIsChargeable()
        {
            return isChargeable;
        }

        public Enums.WeaponType GetWeaponType()
        {
            return weaponType;
        }

        public Enums.WeaponFamily GetWeaponFamily()
        {
            return weaponFamily;
        }

        public GameObject GetProjectile()
        {
            return projectile;
        }

        public float GetAttackSpeed()
        {
            return attackSpeed;
        }

        public override string GetHoverStats(bool multiplyByAmount = false)
        {
            string stats= GetRarityGradient() + GetName() + "</gradient>\n";
            stats += GetDescription() + "\n";
            stats += "\nDamage : <gradient=\"StatDamageGradient\">" + GetDamage() + "</gradient>\n";
            stats += "Sharpness : <gradient=\"StatDexterityGradient\">" + GetSharpness() + "</gradient>\n";
            stats += "Heaviness : <gradient=\"StatStrengthGradient\">" + GetHeaviness() + "</gradient>\n";
            stats += "Crit Chance : <gradient=\"StatCritChanceGradient\">" + GetBonusCritChance() + "%</gradient>\n";
            stats += "Crit Multiplier : <gradient=\"StatCritMultiplierGradient\">" + GetBonusCritMultiplier() + "%</gradient>\n";
            stats += "Attack Speed : <gradient=\"StatAttackSpeedGradient\">" + (100 + GetAttackSpeed()) + "%</gradient>\n";
            stats += "Attack Duration : <gradient=\"StatAttackDurationGradient\">" + (1 / GetAttackDuration()).ToString("F2") + "</gradient>\n";
            stats += "\nValue : <gradient=\"GoldGradient\">" + GetValue() + "</gradient>\n";

            stats += GetRarityGradient() + GetItemRarity() + " " + GetItemTypeToString() + "</gradient> Req. lvl : " + GetRequiredLevel() + "\n";
            return stats;
        }
        
        public override string GetItemTypeToString()
        {
            return weaponFamily.ToString();
        }
        
        public Effect GetWeaponEffect()
        {
            return weaponEffect;
        }
    }
}