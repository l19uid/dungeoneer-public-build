using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class Item : MonoBehaviour
    {
        [Header("Item Stats")] [SerializeField]
        private string itemName;

        [SerializeField] private string itemID;
        [SerializeField] private string localID;
        [TextArea] [SerializeField] private string itemDescription;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int itemValue;
        [SerializeField] private bool stackable;
        [SerializeField] private int maxStack;
        [SerializeField] private int amount = 1;
        [SerializeField] private int requiredLevel = 0;

        SpriteRenderer spriteRenderer;

        private Vector3 pickUpPos;

        [SerializeField] private Enums.ItemType itemType;

        [SerializeField] private Enums.ItemRarity itemRarity;

        [SerializeField] private Enums.ItemSlot itemSlot;

        [Header("Only for evolvable Items")] [SerializeField]
        private bool isEvolved;

        [SerializeField] private Item evolvedItem;

        [Header("Item Stats")] [SerializeField]
        float defense = 0;

        [SerializeField] float bonusHealth = 0;
        [SerializeField] float healthRegen = 0;
        [SerializeField] float bonusMana = 0;
        [SerializeField] float manaRegen = 0;
        [SerializeField] float bonusMovementSpeed = 0;
        [SerializeField] float luck = 0;
        [SerializeField] float damage = 0;

        [SerializeField] [Tooltip("!!! HEAVINESS IS LIMITED TO WEAPONS !!!")]
        float heaviness;

        [SerializeField] [Tooltip("!!! SHARPNESS IS LIMITED TO WEAPONS !!!")]
        float sharpness;

        [Tooltip("!!! STRENGTH IS LIMITED TO EVERYTHING EXCEPT WEAPONS !!!")] [SerializeField]
        float strength = 0;

        [SerializeField] [Tooltip("!!! DEXTERITY IS LIMITED TO EVERYTHING EXCEPT WEAPONS !!!")]
        float dexterity = 0;

        [SerializeField] float bonusAttackSpeed = 0;
        [SerializeField] float bonusCritChance = 0;
        [SerializeField] float bonusCritMultiplier = 0;

        [SerializeField] private Recipe craftingRecipe;
        [SerializeField] private Recipe sharpenerRecipe;
        [SerializeField] private Recipe anvilRecipe;
        
        [SerializeField] private int sharpenerAmount;
        [SerializeField] private int anvilAmount;

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

        public float GetDamage()
        {
            return damage;
        }

        public float GetSharpness()
        {
            return (float)(sharpness + (sharpness * (.05 * sharpenerAmount)));
        }
        
        public string GetSharpnessToString()
        {
            if (Mathf.Round((float)(sharpness * (.05 * sharpenerAmount))) < 0)
                return sharpness.ToString();
            return sharpness + "+<gradient=\"StatDexterityGradient\">" + Mathf.Round((float)(sharpness * (.05 * sharpenerAmount))) +"</gradient>";
        }

        public float GetHeaviness()
        {
            return (float)(heaviness + (heaviness * (.05 * anvilAmount)));
        }
        
        public string GetHeavinessToString()
        {
            if (Mathf.Round((float)(heaviness * (.05 * anvilAmount))) < 0)
                return heaviness.ToString();
            return heaviness + "+<gradient=\"StatHeavinessGradient\">" + Mathf.Round((float)(heaviness * (.05 * anvilAmount))) +"</gradient>";
        }

        public float GetBonusAttackSpeed()
        {
            return bonusAttackSpeed;
        }

        public float GetBonusCritChance()
        {
            return bonusCritChance;
        }

        public float GetBonusCritMultiplier()
        {
            return bonusCritMultiplier;
        }

        public virtual string ItemToString()
        {
            string stats = GetName() + "\n";

            stats += "Rarity: " + GetRarityToString() + "\n";
            stats += "Description: " + GetDescription() + "\n";

            if (GetDefense() != 0)
                stats += "GetDefense(): " + GetDefense() + "\n";
            if (GetBonusHealth() != 0)
                stats += "Health: " + GetBonusHealth() + "\n";
            if (GetHealthRegen() != 0)
                stats += "Health Regen: " + GetHealthRegen() + "\n";
            if (GetBonusMana() != 0)
                stats += "Mana: " + GetBonusMana() + "\n";
            if (GetManaRegen() != 0)
                stats += "Mana Regen: " + GetManaRegen() + "\n";
            if (GetLuck() != 0)
                stats += "Luck: " + GetLuck() + "\n";
            if (GetBonusAttackSpeed() != 0)
                stats += "Attack Speed: " + GetBonusAttackSpeed() + "\n";
            if (GetBonusCritChance() != 0)
                stats += "Crit Chance: " + GetBonusCritChance() + "\n";
            if (GetBonusCritMultiplier() != 0)
                stats += "Crit multiplier: " + GetBonusCritMultiplier() + "\n";
            if (GetDexterity() != 0)
                stats += "Dexterity: " + GetDexterity() + "\n";
            if (GetSharpness() != 0)
                stats += GetSharpnessToString() + "\n";
            if (GetStrength() != 0)
                stats += "Strength: " + GetStrength() + "\n";
            if (GetHeaviness() != 0)
                stats += GetHeavinessToString() + "\n";
            if (GetBonusMovementSpeed() != 0)
                stats += "Movement Speed: " + GetBonusMovementSpeed() + "\n";

            stats += "Amount: " + GetAmount() + "\n";
            if (Input.GetKey(KeyCode.LeftShift))
                stats += "<gradient=\"GoldGradient\">Cost: " + GetValue() * GetAmount() + "</gradient>\n";
            else
                stats += "<gradient=\"GoldGradient\">Cost: " + GetValue() + "</gradient>\n";

            return stats;
        }

        public virtual string StatsToString()
        {
            string stats = "";
            if (GetDamage() != 0)
                stats += "Damage: <gradient=\"StatDamageGradient\">" + GetDamage() + "</gradient> \n";
            if (GetDexterity() != 0)
                stats += "Dexterity: " + GetDexterity() + "\n";
            if (GetSharpness() != 0)
                stats += "Sharpness: " + GetSharpnessToString() + "\n";
            if (GetStrength() != 0)
                stats += "Strength: " + GetStrength() + "\n";
            if (GetHeaviness() != 0)
                stats += "Heaviness: " + GetHeavinessToString() + "\n";
            if (GetBonusAttackSpeed() != 0)
                stats += "Attack Speed: " + GetBonusAttackSpeed() + "\n";
            if (GetBonusCritChance() != 0)
                stats += "Crit Chance: " + GetBonusCritChance() + "\n";
            if (GetBonusCritMultiplier() != 0)
                stats += "Crit multiplier: " + GetBonusCritMultiplier() + "\n";
            if (GetDefense() != 0)
                stats += "Defense: <gradient=\"StatArmorGradient\">" + GetDefense() + "</gradient> \n";
            if (GetBonusHealth() != 0)
                stats += "Health: " + GetBonusHealth() + "\n";
            if (GetHealthRegen() != 0)
                stats += "Health Regen: " + GetHealthRegen() + "\n";
            if (GetBonusMana() != 0)
                stats += "Mana: <gradient=\"StatManaGradient\">" + GetBonusMana() + "</gradient> \n";
            if (GetManaRegen() != 0)
                stats += "Mana Regen: <gradient=\"StatManaRegenGradient\">" + GetManaRegen() + "</gradient> \n";
            if (GetLuck() != 0)
                stats += "Luck: " + GetLuck() + "\n";
            if (GetBonusMovementSpeed() != 0)
                stats += "Movement Speed: " + GetBonusMovementSpeed() + "\n";

            return stats;
        }

        public string GetRarityGradient()
        {
            switch (GetItemRarity())
            {
                case Enums.ItemRarity.Common:
                    return "<gradient=\"RarityCommonGradient\">";
                case Enums.ItemRarity.Uncommon:
                    return "<gradient=\"RarityUncommonGradient\">";
                case Enums.ItemRarity.Rare:
                    return "<gradient=\"RarityRareGradient\">";
                case Enums.ItemRarity.Epic:
                    return "<gradient=\"RarityEpicGradient\">";
                case Enums.ItemRarity.Legendary:
                    return "<gradient=\"RarityLegendaryGradient\">";
                case Enums.ItemRarity.Fabled:
                    return "<gradient=\"RarityFabledGradient\">";
                case Enums.ItemRarity.Celestial:
                    return "<gradient=\"RarityCelestialGradient\">";
                case Enums.ItemRarity.Demonic:
                    return "<gradient=\"RarityDemonicGradient\">";
                default:
                    return "Unknown Rarity";
            }
        }

        public string GetName()
        {
            return itemName;
        }

        public string GetDescription()
        {
            return itemDescription;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public int GetValue()
        {
            return itemValue;
        }

        public bool GetStackable()
        {
            return stackable;
        }

        public int GetMaxStack()
        {
            return maxStack;
        }

        public Enums.ItemType GetItemType()
        {
            return itemType;
        }

        public Enums.ItemRarity GetItemRarity()
        {
            return itemRarity;
        }

        public Enums.ItemSlot GetItemSlot()
        {
            return itemSlot;
        }

        public Sprite GetItemIcon()
        {
            return itemIcon;
        }

        public bool IsEvolved()
        {
            return isEvolved;
        }

        public void Evolve()
        {
            isEvolved = true;
        }

        public Item GetEvolvedItem()
        {
            return evolvedItem;
        }

        public Recipe GetCraftingRecipe(int index)
        {
            return craftingRecipe;
        }

        public int GetAmount()
        {
            return amount;
        }

        public void AddAmount(int amountToAdd)
        {
            amount += amountToAdd;
        }

        public void SetAmount(int newAmount)
        {
            amount = newAmount;
        }

        public int GetSharpenerAmount()
        {
            return sharpenerAmount;
        }

        public int GetAnvilAmount()
        {
            return anvilAmount;
        }

        public string GetRarityToString()
        {
            switch (GetItemRarity())
            {
                case Enums.ItemRarity.Common:
                    return "<gradient=\"RarityCommonGradient\">Common</gradient>";
                case Enums.ItemRarity.Uncommon:
                    return "<gradient=\"RarityUncommonGradient\">Uncommon</gradient>";
                case Enums.ItemRarity.Rare:
                    return "<gradient=\"RarityRareGradient\">Rare</gradient>";
                case Enums.ItemRarity.Epic:
                    return "<gradient=\"RarityEpicGradient\">Epic</gradient>";
                case Enums.ItemRarity.Legendary:
                    return "<gradient=\"RarityLegendaryGradient\">Legendary</gradient>";
                case Enums.ItemRarity.Fabled:
                    return "<gradient=\"RarityFabledGradient\">Fabled</gradient>";
                case Enums.ItemRarity.Celestial:
                    return "<color=#FF0000>C</color>" +
                           "<color=#FF5400>e</color>" +
                           "<color=#FFD000>l</color>" +
                           "<color=#BFFF00>e</color>" +
                           "<color=#19FF00>s</color>" +
                           "<color=#00FF87>t</color>" +
                           "<color=#00D8FF>i</color>" +
                           "<color=#0026FF>a</color>" +
                           "<color=#BF00FF>l</color>";
                case Enums.ItemRarity.Demonic:
                    return "<gradient=\"RarityDemonicGradient\">Fabled</gradient>";
                default:
                    return "Unknown Rarity";
            }
        }

        public virtual string GetItemTypeToString()
        {
            return itemType.ToString();
        }
        
        public virtual int GetRequiredLevel()
        {
            return requiredLevel;
        }

        public virtual string GetHoverStats(bool multiplyByAmount = false)
        {
            string text = "";
            text += GetRarityGradient() + GetName() + "</gradient> \n";
            text += GetDescription() + "\n \n";
            text += StatsToString() + "\n";
            if (GetStackable())
            {
                text += "Amount: " + GetAmount() + "\n";
                if (multiplyByAmount)
                    text += "Value : <gradient=\"GoldGradient\">" + GetValue() * GetAmount() + " G</gradient> \n";
                else
                    text += "Value : <gradient=\"GoldGradient\">" + GetValue() + " G</gradient> per item.\n";
            }
            else
                text += "Value : <gradient=\"GoldGradient\">" + GetValue() + " G</gradient> \n";

            text += GetRarityGradient() + GetItemRarity() + " " +
                    GetItemTypeToString() + "</gradient>";
            return text;
        }

        public void AddSharpenerAmount(int a)
        {
            sharpenerAmount += a;
        }

        public void AddAnvilAmount(int a)
        {
            anvilAmount += a;
        }

        public string GetItemId()
        {
            return itemID;
        }

        public void SetEngravings(EngravingData[] itemDataEngravings)
        {
            throw new NotImplementedException();
        }

        public Recipe GetRecipe(string type = "crafting")
        {
            if(type == "anvil")
                return anvilRecipe;
            else if (type == "sharpener")
                return sharpenerRecipe;
            else
                return craftingRecipe;
        }

        public void UpgradeItem(string type)
        {
            if (type == "anvil")
            {
                anvilRecipe.Increase(anvilAmount/2);
                anvilAmount++;
            }
            else if (type == "sharpener")
            {
                sharpenerRecipe.Increase(anvilAmount/2);
                sharpenerAmount++;
            }
        }

        public string GetUpgradeText()
        {
            string text = GetName() + "\n";
            text += "Sharpener upgrade. \n";
            text += "Damage: " + GetDamage() + "\n";
            if(GetSharpenerAmount() <= 4)
                text += "Sharpness: " + GetSharpness() + "<gradient=\"StatDexterityGradient\"> ("+ (GetSharpenerAmount()+1)*.025*GetSharpness() + ")</gradient>\n";
            else 
                text += "Sharpness: " + GetSharpness() + "<gradient=\"StatDexterityGradient\"> ("+ GetSharpenerAmount()*.025*GetSharpness() + ")</gradient> Maxed out!\n";
            text += "\n Anvil upgrade. \n";
            text += "Damage: " + GetDamage() + "\n";
            if(GetAnvilAmount() <= 4)
                text += "Heaviness: " + GetHeaviness() + "<gradient=\"StatStrengthGradient\"> ("+ (GetAnvilAmount()+1)*.025*GetHeaviness() + ")</gradient>\n";
            else 
                text += "Heaviness: " + GetHeaviness() + "<gradient=\"StatStrengthGradient\"> ("+ GetAnvilAmount()*.025*GetHeaviness() + ")</gradient> Maxed out!\n";
            return text;
        }

        public void SetSharpenerAmount(int itemDataSharpenerAmount)
        {
            sharpenerAmount = itemDataSharpenerAmount;
        }

        public void SetAnvilAmount(int itemDataAnvilAmount)
        {
            anvilAmount = itemDataAnvilAmount;
        }
        
        public string GetLocalID()
        {
            return localID;
        }
        
        public void SetLocalID(string id)
        {
            localID = id;
        }
        
        public void GenerateLocalID()
        {
            localID = Guid.NewGuid().ToString();
        }

        public int GetStackValue()
        {
            return GetValue() * GetAmount();
        }
    }
}