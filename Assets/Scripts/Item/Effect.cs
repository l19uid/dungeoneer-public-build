using System;
using UnityEngine;

namespace Dungeoneer
{
    public class Effect : MonoBehaviour
    {
        [SerializeField]
        Enums.Effect effect;
        [SerializeField]
        float duration;
        [SerializeField]
        float timeLeft;
        
        [SerializeField]
        float defense;
        [SerializeField]
        float bonusHealth;
        [SerializeField]
        float instantHealth;
        [SerializeField]
        float healthRegen;
        [SerializeField]
        float bonusMana;
        [SerializeField]
        float instantMana;
        [SerializeField]
        float manaRegen;
        [SerializeField]
        float bonusMovementSpeed;
        [SerializeField]
        float strength;
        [SerializeField]
        float dexterity;
        [SerializeField]
        float luck;
        [SerializeField]
        float attackSpeed;
        [SerializeField]
        float critChance;
        [SerializeField]
        float critMultiplier;
        [SerializeField]
        float magicDamage;
        [SerializeField]
        GameObject effectGameObject;

        private void Start()
        {
            timeLeft = duration;
        }

        public void SetEffect(Enums.Effect effect, float duration)
        {
            this.effect = effect;
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetEffect(Enums.Effect effect, float duration, float defense, float bonusHealth, float healthRegen, float bonusMana, float manaRegen, float bonusMovementSpeed, float strength, float dexterity, float luck, float attackSpeed, float critChance, float critMultiplier)
        {
            this.effect = effect;
            this.duration = duration;
            timeLeft = duration;
            
            this.defense = defense;
            this.bonusHealth = bonusHealth;
            this.healthRegen = healthRegen;
            this.bonusMana = bonusMana;
            this.manaRegen = manaRegen;
            this.bonusMovementSpeed = bonusMovementSpeed;
            this.strength = strength;
            this.dexterity = dexterity;
            this.luck = luck;
            this.attackSpeed = attackSpeed;
            this.critChance = critChance;
            this.critMultiplier = critMultiplier;
        }
        
        public void SetBurnEffect(float duration, float damage)
        {
            effect = Enums.Effect.Burn;
            this.duration = duration;
            timeLeft = duration;
            this.magicDamage = damage;
        }
        
        public void SetFreezeEffect(float duration)
        {
            effect = Enums.Effect.Freeze;
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetStunEffect(float duration)
        {
            effect = Enums.Effect.Stun;
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetHealingEffect(float duration, float instantHealth, float healthRegen)
        {
            effect = Enums.Effect.Healing;
            this.duration = duration;
            timeLeft = duration;
            this.instantHealth = instantHealth;
            this.healthRegen = healthRegen;
        }
        
        public void SetMagicBoostEffect(float duration, float magicDamage)
        {
            effect = Enums.Effect.MagicBoost;
            this.duration = duration;
            timeLeft = duration;
            this.magicDamage = magicDamage;
        }
        
        public void SetDefenseBoostEffect(float duration, float defense)
        {
            effect = Enums.Effect.DefenseBoost;
            this.duration = duration;
            timeLeft = duration;
            this.defense = defense;
        }
        
        public void SetSpeedBoostEffect(float duration, float bonusMovementSpeed)
        {
            effect = Enums.Effect.SpeedBoost;
            this.duration = duration;
            timeLeft = duration;
            this.bonusMovementSpeed = bonusMovementSpeed;
        }
        
        public void SetLuckBoostEffect(float duration, float luck)
        {
            effect = Enums.Effect.LuckBoost;
            this.duration = duration;
            timeLeft = duration;
            this.luck = luck;
        }
        
        public void SetInvisibilityEffect(float duration)
        {
            effect = Enums.Effect.Invisibility;
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetInvulnerabilityEffect(float duration)
        {
            effect = Enums.Effect.Invulnerability;
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetAbsorbtionEffect(float duration, float bonusHealth, float bonusMana)
        {
            effect = Enums.Effect.Absorbtion;
            this.duration = duration;
            timeLeft = duration;
            this.bonusHealth = bonusHealth;
            this.bonusMana = bonusMana;
        }
        
        public void SetMuscleBoostEffect(float duration, float strength, float dexterity)
        {
            effect = Enums.Effect.MuscleBoost;
            this.duration = duration;
            timeLeft = duration;
            this.strength = strength;
            this.dexterity = dexterity;
        }
        
        public void SetPoisonEffect(float duration, float damage)
        {
            effect = Enums.Effect.Poison;
            this.duration = duration;
            timeLeft = duration;
            this.magicDamage = damage;
        }
        
        public void SetBleedEffect(float duration, float damage)
        {
            effect = Enums.Effect.Bleed;
            this.duration = duration;
            timeLeft = duration;
            this.magicDamage = damage;
        }
        
        public void SetTarEffect(float duration, float bonusMovementSpeed)
        {
            effect = Enums.Effect.Tar;
            this.duration = duration;
            timeLeft = duration;
            this.bonusMovementSpeed = bonusMovementSpeed;
        }
        
        public void SetGlueEffect(float duration, float attackSpeed)
        {
            effect = Enums.Effect.Glue;
            this.duration = duration;
            timeLeft = duration;
            this.attackSpeed = attackSpeed;
        }
        
        public void SetDuration(float duration)
        {
            this.duration = duration;
            timeLeft = duration;
        }
        
        public void SetTimeLeft(float timeLeft)
        {
            this.timeLeft = timeLeft;
        }
        
        public float GetDefense()
        {
            return defense;
        }
        
        public float GetBonusHealth()
        {
            return bonusHealth;
        }
        
        public float GetBonusMana()
        {
            return bonusMana;
        }
        
        public float GetHealthRegen()
        {
            return healthRegen;
        }
        
        public float GetManaRegen()
        {
            return manaRegen;
        }
        
        public float GetBonusMovementSpeed()
        {
            return bonusMovementSpeed;
        }
        
        public float GetStrength()
        {
            return strength;
        }
        
        public float GetDexterity()
        {
            return dexterity;
        }
        
        public float GetLuck()
        {
            return luck;
        }
        
        public float GetAttackSpeed()
        {
            return attackSpeed;
        }
        
        public float GetCritChance()
        {
            return critChance;
        }
        
        public float GetCritMultiplier()
        {
            return critMultiplier;
        }
        
        public Enums.Effect GetEffect()
        {
            return effect;
        }
        
        public float GetDuration()
        {
            return duration;
        }
        
        public float GetTimeLeft()
        {
            return timeLeft;
        }
        
        public float GetMagicDamage()
        {
            return magicDamage;
        }
        
        public float GetInstantHealth()
        {
            return instantHealth;
        }
        
        public float GetInstantMana()
        {
            return instantMana;
        }
        
        public void UpdateTimeLeft(float time)
        {
            timeLeft -= time;
        }
        
        public void SetTimeLeftToDuration()
        {
            timeLeft = duration;
        }
        
        public void SetTimeLeftToZero()
        {
            timeLeft = 0;
        }
        
        public bool IsEffectOver()
        {
            return timeLeft <= 0;
        }
        
        public void SetEffectOver()
        {
            timeLeft = 0;
        }
        
        public void SetEffectOver(float time)
        {
            timeLeft = time;
        }
        
        public void SetEffectOver(float time, float duration)
        {
            timeLeft = time;
            this.duration = duration;
        }

        public GameObject GetEffectGameObject()
        {
            return effectGameObject;
        }

        public void UpdateEffect(float deltaTime)
        {
            timeLeft -= deltaTime;
        }

        public void SetMagicDamage(float getMagicDamage)
        {
            magicDamage = getMagicDamage;
        }
    }
}