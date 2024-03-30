using UnityEngine;

namespace Dungeoneer
{
    public class Armor : Item
    {
        [SerializeField]
        private string _animationHash;
        public override string ItemToString()
        {
            string stats = GetName() + "\n";
            stats += "Rarity : " + GetRarityToString() + "\n";

            stats += "Defense : " + GetDefense() + "\n";
            if(GetBonusHealth() != 0)
                stats += "Health : " + GetBonusHealth() + "\n";
            if(GetHealthRegen() != 0)
                stats += "Health Regen : " + GetHealthRegen() + "\n";
            if(GetLuck() != 0)
                stats += "Luck : " + GetLuck() + "\n";
            if(GetBonusAttackSpeed() != 0)
                stats += "Attack Speed : " + GetBonusAttackSpeed() + "\n";
            if(GetBonusCritChance() != 0)
                stats += "Crit Chance : " + GetBonusCritChance() + "\n";
            if(GetBonusCritMultiplier() != 0)
                stats += "Crit multiplier : " + GetBonusCritMultiplier() + "\n";
            if(GetDexterity() != 0)
                stats += "Dexterity : " + GetDexterity() + "\n";
            if(GetStrength() != 0)
                stats += "Strength : " + GetStrength() + "\n";
            if(GetBonusMana() != 0)
                stats += "Mana : " + GetBonusMana() + "\n";
            if(GetManaRegen() != 0)
                stats += "Mana Regen : " + GetManaRegen() + "\n";
            if(GetBonusMovementSpeed() != 0)
                stats += "Movement Speed : " + GetBonusMovementSpeed() + "\n";
            if(GetValue() != 0)
                stats += "<gradient=\"GoldGradient\">Cost : " + GetValue() + "</gradient>\n";
            
            return stats;
        }  
        
        public override string GetItemTypeToString()
        {
            return GetItemSlot().ToString();
        }

        public override string GetHoverStats(bool multiplyByAmount = false)
        {
            string stats = GetRarityGradient() + GetName() + "</gradient>\n";
            stats += GetDescription() + "\n";
            if (GetDefense() != 0)
                stats += "\nArmor : <gradient=\"StatArmorGradient\">" + GetDefense() + "</gradient>\n";
            if (GetBonusHealth() != 0)
                stats += "Health : <gradient=\"StatHealthGradient\">" + GetBonusHealth() + "</gradient>\n";
            if (GetHealthRegen() != 0)
                stats += "Health Regen : <gradient=\"StatHealthRegenGradient\">" + GetHealthRegen() + "</gradient>\n";
            if (GetLuck() != 0)
                stats += "Luck : <gradient=\"StatLuckGradient\">" + GetLuck() + "</gradient>\n";
            if (GetBonusAttackSpeed() != 0)
                stats += "Attack Speed : <gradient=\"StatAttackSpeedGradient\">" + GetBonusAttackSpeed() + "</gradient>\n";
            if (GetBonusCritChance() != 0)
                stats += "Crit Chance : <gradient=\"StatCritChanceGradient\">" + GetBonusCritChance() + "</gradient>\n";
            if (GetBonusCritMultiplier() != 0)
                stats += "Crit multiplier : <gradient=\"StatCritMultiplierGradient\">" + GetBonusCritMultiplier() + "</gradient>\n";
            if (GetDexterity() != 0)
                stats += "Dexterity : <gradient=\"StatDexterityGradient\">" + GetDexterity() + "</gradient>\n";
            if (GetStrength() != 0)
                stats += "Strength : <gradient=\"StatStrengthGradient\">" + GetStrength() + "</gradient>\n";
            if (GetBonusMana() != 0)
                stats += "Mana : <gradient=\"StatManaGradient\">" + GetBonusMana() + "</gradient>\n";
            if (GetManaRegen() != 0)
                stats += "Mana Regen : <gradient=\"StatManaRegenGradient\">" + GetManaRegen() + "</gradient>\n";
            if (GetBonusMovementSpeed() != 0)
                stats += "Movement Speed : <gradient=\"StatMoveSpeedGradient\">" + GetBonusMovementSpeed() + "</gradient>\n";
            
            stats += "\nValue : <gradient=\"GoldGradient\">" + GetValue() + "</gradient>\n";
            
            stats += GetRarityGradient() + GetItemRarity() + " " + GetItemTypeToString() + "</gradient> Req. lvl : " + GetRequiredLevel() + "\n";
            return stats;
        }
        
        public string GetAnimationHash()
        {
            return _animationHash;
        }
        
    }
}