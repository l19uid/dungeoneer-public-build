using UnityEngine;

namespace Dungeoneer
{
    public class Artifact : Item
    {
        public override string ItemToString()
        { 
            string stats = GetName() + "\n";
            stats += GetDescription() + "\n";
            stats += "Rarity: " + GetRarityToString() + "\n";
    
            if(GetDefense() > 0)
                stats += "Defense: " + GetDefense() + "\n";
            if(GetBonusHealth() > 0)
                stats += "Health: " + GetBonusHealth() + "\n";
            if(GetHealthRegen() > 0)
                stats += "Health Regen: " + GetHealthRegen() + "\n";
            if(GetBonusMana() > 0)
                stats += "Mana: " + GetBonusMana() + "\n";
            if(GetManaRegen() > 0)
                stats += "Mana Regen: " + GetManaRegen() + "\n";
            if(GetLuck() > 0)
                stats += "Luck: " + GetLuck() + "\n";
            if(GetBonusAttackSpeed() > 0)
                stats += "Attack Speed: " + GetBonusAttackSpeed() + "\n";
            if(GetBonusCritChance() > 0)
                stats += "Crit Chance: " + GetBonusCritChance() + "\n";
            if(GetBonusCritMultiplier() > 0)
                stats += "Crit multiplier: " + GetBonusCritMultiplier() + "\n";
            if(GetDexterity() > 0)
                stats += "Dexterity: " + GetDexterity() + "\n";
            if(GetSharpness() > 0)
                stats += "Sharpness: " + GetSharpness() + "\n";
            if(GetStrength() > 0)
                stats += "Strength: " + GetStrength() + "\n";
            if(GetHeaviness() > 0)
                stats += "Heaviness: " + GetHeaviness() + "\n";
            if(GetBonusMovementSpeed() > 0)
                stats += "Movement Speed: " + GetBonusMovementSpeed() + "\n";
            return stats;
        }

        public override string GetHoverStats(bool multiplyByAmount = false)
        {
            string stats = GetRarityGradient() + GetName() + "</gradient>\n";
            stats += GetDescription() + "\n";
            if (GetDefense() > 0)
                stats += "\nArmor : <gradient=\"StatArmorGradient\">" + GetDefense() + "</gradient>\n";
            if (GetBonusHealth() > 0)
                stats += "Health : <gradient=\"StatHealthGradient\">" + GetBonusHealth() + "</gradient>\n";
            if (GetHealthRegen() > 0)
                stats += "Health Regen : <gradient=\"StatHealthRegenGradient\">" + GetHealthRegen() + "</gradient>\n";
            if (GetLuck() > 0)
                stats += "Luck : <gradient=\"StatLuckGradient\">" + GetLuck() + "</gradient>\n";
            if (GetBonusAttackSpeed() > 0)
                stats += "Attack Speed : <gradient=\"StatAttackSpeedGradient\">" + GetBonusAttackSpeed() + "</gradient>\n";
            if (GetBonusCritChance() > 0)
                stats += "Crit Chance : <gradient=\"StatCritChanceGradient\">" + GetBonusCritChance() + "%</gradient>\n";
            if (GetBonusCritMultiplier() > 0)
                stats += "Crit multiplier : <gradient=\"StatCritMultiplierGradient\">" + GetBonusCritMultiplier() + "%</gradient>\n";
            if (GetDexterity() > 0)
                stats += "Dexterity : <gradient=\"StatDexterityGradient\">" + GetDexterity() + "</gradient>\n";
            if (GetSharpness() > 0)
                stats += "Sharpness : <gradient=\"StatSharpnessGradient\">" + GetSharpness() + "</gradient>\n";
            if (GetStrength() > 0)
                stats += "Strength : <gradient=\"StatStrengthGradient\">" + GetStrength() + "</gradient>\n";
            if (GetHeaviness() > 0)
                stats += "Heaviness : <gradient=\"StatHeavinessGradient\">" + GetHeaviness() + "</gradient>\n";
            if (GetBonusMovementSpeed() > 0)
                stats += "Movement Speed : <gradient=\"StatMoveSpeedGradient\">" + GetBonusMovementSpeed() + "</gradient>\n";
            
            stats += "\n<gradient=\"GoldGradient\">Cost : " + GetValue() + "</gradient>\n";
            stats += GetRarityGradient() + GetItemRarity() + " " + GetItemTypeToString() + "</gradient>\n";

            return stats;
        }
    }
}