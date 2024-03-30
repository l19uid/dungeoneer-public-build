using UnityEngine;
using UnityEngine.Search;

namespace Dungeoneer
{
    public class Ability : Item
    {
        [Header("Ability Stats")]
        [SerializeField]
        private Enums.AbilityType abilityType;
        [SerializeField]
        private Enums.AbilityElement abilityElement;

        [SerializeField] 
        private float magicDamage = 0;
        
        [SerializeField]
        private float attackDistance = 0;
        [SerializeField]
        private float healing = 0;
        [SerializeField]
        private float recastTime = 0;
        [SerializeField]
        private float manaCost = 0;
        
        public override string ItemToString()
        {
            string stats = GetName();
            stats += "Rarity: " + GetRarityToString() + "\n";

            stats += "Type : " + GetAbilityType() + "\n";
            stats += "Element : " + GetAbilityElement() + "\n";
            
            if(GetDamage() > 0)
                stats += "Damage : " + GetDamage() + "\n";
            if(GetHealing() > 0)
                stats += "Healing : " + GetHealing() + "\n";
            if(GetManaCost() > 0)
                stats += "Mana Cost : " + GetManaCost() + "\n";
            if(GetValue() != 0)
                stats += "<gradient=\"GoldGradient\">Cost: " + GetValue() + "</gradient>\n";
            
            return stats;
        }

        private Enums.AbilityElement GetAbilityElement()
        {
            return abilityElement;
        }

        private Enums.AbilityType GetAbilityType()
        {
            return abilityType;
        }
        
        public float GetAttackDistance()
        {
            return attackDistance;
        }
        
        public float GetHealing()
        {
            return healing;
        }
        
        public float GetRecastTime()
        {
            return recastTime;
        }
        
        public float GetManaCost()
        {
            return manaCost;
        }
    }
}