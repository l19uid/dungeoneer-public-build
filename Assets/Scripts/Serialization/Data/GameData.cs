using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dungeoneer
{
    [Serializable]
    public class GameData
    {
        public string version = "0.4";
        public PlayerData playerData;

        public GameData(string playerName = "Player")
        {
            playerData = new PlayerData();
            playerData.playerName = playerName;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [Serializable]
    public class InventoryData
    {
        public List<ItemData> items = new List<ItemData>();
        public List<ItemData> armor = new List<ItemData>();
        public List<ItemData> weapons = new List<ItemData>();
        public List<ItemData> artifacts = new List<ItemData>();
        public List<ItemData> abilities = new List<ItemData>();
        public List<ItemData> consumables = new List<ItemData>();
        public List<ItemData> quests = new List<ItemData>();
        public int gold = 100;
        public int stars = 0;
        public int maxSlots = 30;
        public ItemData equippedWeapon;
        public ItemData equippedAbility;
        public ItemData equippedHelmet;
        public ItemData equippedChestplate;
        public ItemData equippedLeggings;
        public ItemData equippedBoots;
        public ItemData equippedNecklace;
        public ItemData equippedGloves;
        public List<ItemData> equippedArtifacts;

        public void AddItem(Item item, int slot = 0)
        {
            ItemData itemData = new ItemData(item, slot);

            if (item.GetItemType() == Enums.ItemType.Artifact)
            {
                artifacts.Add(itemData);
            }
            else if (item.GetItemType() == Enums.ItemType.Ability)
            {
                abilities.Add(itemData);
            }
            else if (item.GetItemType() == Enums.ItemType.Consumable)
            {
                consumables.Add(itemData);
            }
            else if (item.GetItemType() == Enums.ItemType.Quest)
            {
                quests.Add(itemData);
            }
            else if (item.GetItemType() == Enums.ItemType.Weapon)
            {
                weapons.Add(itemData);
            }
            else if (item.GetItemType() == Enums.ItemType.Armor)
            {
                armor.Add(itemData);
            }
            else
            {
                items.Add(itemData);
            }
        }

        public void AddItems(Dictionary<int,GameObject> items)
        {
            Dictionary<int,Item> itemScripts = new Dictionary<int,Item>();
            foreach (var i in items)
            {
                if(items.Count < 30)
                    itemScripts.Add(i.Key,i.Value.GetComponent<Item>());
            }

            foreach (var item in itemScripts)
            {
                AddItem(item.Value,item.Key);
            }
        }

        public void AddEquippedItem(GameObject itemGO)
        {
            if (itemGO == null)
                return;
            Item item = itemGO.GetComponent<Item>();
            ItemData itemData = new ItemData(item);

            if (item.GetItemType() == Enums.ItemType.Weapon)
            {
                equippedWeapon = itemData;
            }
            else if (item.GetItemType() == Enums.ItemType.Ability)
            {
                equippedAbility = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Helmet)
            {
                equippedHelmet = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Chestplate)
            {
                equippedChestplate = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Leggings)
            {
                equippedLeggings = itemData;
                Debug.Log("Equipped leggings." + equippedLeggings.path + " - " + equippedLeggings.slot);
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Boots)
            {
                equippedBoots = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Necklace)
            {
                equippedNecklace = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.Gloves)
            {
                equippedGloves = itemData;
            }
            else if (item.GetItemSlot() == Enums.ItemSlot.ArtifactPouch)
            {
                equippedArtifacts.Add(itemData);
            }
        }

        public List<GameObject> GetQuests()
        {
            List<GameObject> inventoryItems = new List<GameObject>();
            foreach (ItemData itemData in quests)
            {
                inventoryItems.Add(DataItemToGameObject(itemData).item);
            }

            return inventoryItems;
        }

        public Dictionary<int,GameObject> GetItems(Enums.ItemType type)
        {
            Dictionary<int,GameObject> inventoryItems = new Dictionary<int,GameObject>();
            switch (type)
            {
                case Enums.ItemType.Ability:
                    foreach (ItemData itemData in abilities)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Artifact:
                    foreach (ItemData itemData in artifacts)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Armor:
                    foreach (ItemData itemData in armor)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Consumable:
                    foreach (ItemData itemData in consumables)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Weapon:
                    foreach (ItemData itemData in weapons)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Quest:
                    foreach (ItemData itemData in quests)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                case Enums.ItemType.Item:
                    foreach (ItemData itemData in items)
                    {
                        inventoryItems.Add(DataItemToGameObject(itemData).slot,DataItemToGameObject(itemData).item);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return inventoryItems;
        }

        public GameObject GetEquippedItem(Enums.ItemSlot slot)
        {
            switch (slot)
            {
                case Enums.ItemSlot.Ability:
                    return DataItemToGameObjectEquipped(equippedAbility);
                case Enums.ItemSlot.Chestplate:
                    return DataItemToGameObjectEquipped(equippedChestplate);
                case Enums.ItemSlot.Gloves:
                    return DataItemToGameObjectEquipped(equippedGloves);
                case Enums.ItemSlot.Helmet:
                    return DataItemToGameObjectEquipped(equippedHelmet);
                case Enums.ItemSlot.Leggings:
                    return DataItemToGameObjectEquipped(equippedLeggings);
                case Enums.ItemSlot.Necklace:
                    return DataItemToGameObjectEquipped(equippedNecklace);
                case Enums.ItemSlot.Boots:
                    return DataItemToGameObjectEquipped(equippedBoots);
                case Enums.ItemSlot.Weapon:
                    return DataItemToGameObjectEquipped(equippedWeapon);
            }

            return null;
        }

        public struct ItemAndSlot
        {
            public GameObject item;
            public int slot;
            
            public ItemAndSlot(GameObject item, int slot)
            {
                this.item = item;
                this.slot = slot;
            }
        }

        public ItemAndSlot DataItemToGameObject(ItemData itemData)
        {
            if (itemData == null)
            {
                Debug.Log("Item data is null");
                // Returns null, why is it like this idk.
                return default(ItemAndSlot);
            }

            if (itemData.path == null)
            {
                Debug.Log("Item data path is null.");
                // Returns null, why is it like this idk.
                return default(ItemAndSlot);;
            }

            if (itemData.path == "")
            {
                Debug.Log("Item data path is empty.");
                // Returns null, why is it like this idk.
                return default(ItemAndSlot);;
            }

            GameObject item = Resources.Load<GameObject>(itemData.path);
            
            if (item == null)
            {
                Debug.LogError("Path : \"" + itemData.path + "\" produces a null item GameObject.");
                // Returns null, why is it like this idk.
                return default(ItemAndSlot);
            }
            
            Debug.Log(item.GetComponent<Item>().GetName() + " - Is loaded.");
            item.GetComponent<Item>().SetSharpenerAmount(itemData.sharpenerAmount);
            item.GetComponent<Item>().SetAnvilAmount(itemData.anvilAmount);
            item.GetComponent<Item>().SetAmount(itemData.amount);
            
            //item.GetComponent<Item>().SetEngravings(itemData.engravings);
            return new ItemAndSlot(item, itemData.slot);
        }
        
        public GameObject DataItemToGameObjectEquipped(ItemData itemData)
        {
            if (itemData == null)
            {
                Debug.Log("Item data is null");
                // Returns null, why is it like this idk.
                return null;
            }

            if (itemData.path == null)
            {
                Debug.Log("Item data path is null.");
                // Returns null, why is it like this idk.
                return null;
            }

            if (itemData.path == "")
            {
                Debug.Log("Item data path is empty.");
                // Returns null, why is it like this idk.
                return null;
            }

            GameObject item = Resources.Load<GameObject>(itemData.path);
            
            if (item == null)
            {
                Debug.LogError("Path : \"" + itemData.path + "\" produces a null item GameObject.");
                // Returns null, why is it like this idk.
                return null;
            }
            
            Debug.Log(item.GetComponent<Item>().GetName() + " - Is loaded.");
            item.GetComponent<Item>().SetSharpenerAmount(itemData.sharpenerAmount);
            item.GetComponent<Item>().SetAnvilAmount(itemData.anvilAmount);
            item.GetComponent<Item>().SetAmount(itemData.amount);
            
            //item.GetComponent<Item>().SetEngravings(itemData.engravings);
            return item;
        }

        public void AddQuests(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                AddItem(gameObject.GetComponent<Item>());
            }
        }
    }
    
    

    [Serializable]
    public class PlayerSkillData
    {
        public string name;
        public Enums.SkillType family;
        public string description;
        public float level;
        public float experience;
        public float wholeExperience;
        public float gap;
        public float toNextLevel;

        public PlayerSkillData(PlayerSkill playerSkill)
        {
            name = playerSkill.GetName();
            family = playerSkill.GetFamily();
            description = playerSkill.GetDescription();
            level = playerSkill.GetLevel();
            experience = playerSkill.GetExperience();
            wholeExperience = playerSkill.GetWholeExperience();
            gap = playerSkill.GetGap();
            toNextLevel = playerSkill.GetExperienceToNextLevel();
        }

        public PlayerSkillData(string name, Enums.SkillType family, string description, float level, float experience,
            float wholeExperience, float gap, float toNextLevel)
        {
            this.name = name;
            this.family = family;
            this.description = description;
            this.level = level;
            this.experience = experience;
            this.wholeExperience = wholeExperience;
            this.gap = gap;
            this.toNextLevel = gap;
        }
    }

    [Serializable]
    public class EnemySoulData
    {
        public string name;
        public Enums.EnemyFamily enemy;
        public string description;
        public int level;
        public int count;
        public int toNextLevel;
        public int wholeCount;
        public int gap;

        public EnemySoulData(EnemySoul enemySoul)
        {
            name = enemySoul.GetName();
            enemy = enemySoul.GetEnemy();
            description = enemySoul.GetDescription();
            level = enemySoul.GetLevel();
            count = enemySoul.GetCount();
            toNextLevel = enemySoul.GetCountToNextLevel();
            wholeCount = enemySoul.GetWholeCount();
            gap = enemySoul.GetGap();
        }

        public EnemySoulData(string name, Enums.EnemyFamily enemy, string description, int level, int count,
            int wholeCount, int gap, int toNextLevel)
        {
            this.name = name;
            this.enemy = enemy;
            this.description = description;
            this.level = level;
            this.count = count;
            this.toNextLevel = gap;
            this.wholeCount = wholeCount;
            this.gap = gap;
        }
    }

    [Serializable]
    public class ItemData
    {
        public string path;
        public int slot;
        public int sharpenerAmount;
        public int anvilAmount;
        public int amount;
        public EngravingData[] engravings;

        public ItemData(Item item, int slot = 0)
        {
            path = "Items/" + item.GetItemType() + "/" + item.GetItemId();
            sharpenerAmount = item.GetSharpenerAmount();
            anvilAmount = item.GetAnvilAmount();
            amount = item.GetAmount();
            this.slot = slot;

            //engravings = new EngravingData[item.GetEngravings().Length];
            //for (int i = 0; i < item.GetEngravings().Length; i++)
            //{
            //    engravings[i] = new EngravingData(item.GetEngravings()[i]);
            //}
        }
    }

    [Serializable]
    public class PlayerData
    {
        public InventoryData inventory;
        public PlayerSkillData mainSkill;
        public PlayerSkillData[] weaponFamilySkills;
        public EnemySoulData[] enemySouls;
        public string playerName;
        public int playerID;
        public float baseLuck;
        public float baseStrength;
        public float baseDexterity;
        public float baseMaxHealth;
        public float baseHealthRegen;
        public float baseMaxMana;
        public float baseManaRegen;
        public float baseDefense;
        public float bonusMoveSpeed;
        
                
        public int bankGold = 0;
        public int bankGoldCap = 0;
        public int bankGoldCapUpgradeCost = 0;
        public int bankGoldCapUpgradeCostMultiplier = 2;

        public PlayerData()
        {
            inventory = new InventoryData();
            playerName = "Player";
            playerID = 0;
            baseLuck = 0;
            baseStrength = 10;
            baseDexterity = 10;
            baseMaxHealth = 100;
            baseHealthRegen = .5f;
            baseMaxMana = 100;
            baseManaRegen = 2;
            baseDefense = 0;
            bonusMoveSpeed = 0;

            // Create new weapon family skills.
            weaponFamilySkills = new PlayerSkillData[12];
            weaponFamilySkills[0] = new PlayerSkillData("Sword", Enums.SkillType.Sword, "Sword Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[1] = new PlayerSkillData("Axe", Enums.SkillType.Axe, "Axe Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[2] = new PlayerSkillData("Spear", Enums.SkillType.Spear, "Spear Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[3] =
                new PlayerSkillData("Daggers", Enums.SkillType.Daggers, "Daggers Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[4] = new PlayerSkillData("Javelin", Enums.SkillType.Javelin, "Javelin Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[5] =
                new PlayerSkillData("Hammer", Enums.SkillType.Hammer, "Hammer Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[6] = new PlayerSkillData("Guantlets", Enums.SkillType.Guantlets, "Guantlets Skill", 1, 0,
                0, 1000, 0);
            weaponFamilySkills[7] =
                new PlayerSkillData("Glaive", Enums.SkillType.Glaive, "Glaive Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[8] = new PlayerSkillData("Staff", Enums.SkillType.Staff, "Staff Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[9] =
                new PlayerSkillData("Crossbow", Enums.SkillType.Crossbow, "Crossbow Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[10] = new PlayerSkillData("Bow", Enums.SkillType.Bow, "Bow Skill", 1, 0, 0, 100, 0);
            weaponFamilySkills[11] = new PlayerSkillData("Scythe", Enums.SkillType.Scythe, "Scythe Skill", 1, 0, 0, 100, 0);

            // Create new main skill.
            mainSkill = new PlayerSkillData("Main", Enums.SkillType.Main, "Main Skill", 1, 0, 0, 200, 0);

            // Create new enemy souls.
            enemySouls = new EnemySoulData[3];
            enemySouls[0] = new EnemySoulData("Undead", Enums.EnemyFamily.Zombie, "Undead.", 
                1, 0, 0, 25, 0);
            enemySouls[1] = new EnemySoulData("Skeletons", Enums.EnemyFamily.Skeleton, "Skeletons.", 
                1, 0, 0, 25, 0);
            enemySouls[2] = new EnemySoulData("Rats", Enums.EnemyFamily.Rat, "Rats.", 
                1, 0, 0, 25, 0);
            
            bankGold = 100;
            bankGoldCap = 100000;
            bankGoldCapUpgradeCost = 2500;
            bankGoldCapUpgradeCostMultiplier = 2;
        }

        public void AddPlayerSkill(PlayerSkill playerSkill)
        {
            PlayerSkillData playerSkillData = new PlayerSkillData(playerSkill);
            if (playerSkill.GetFamily() == Enums.SkillType.Main)
                mainSkill = playerSkillData;
            else
                weaponFamilySkills = new[] { playerSkillData };
        }

        public PlayerSkillData GetPlayerSkillData(string name)
        {
            foreach (PlayerSkillData playerSkillData in weaponFamilySkills)
            {
                if (playerSkillData.name == name)
                {
                    return playerSkillData;
                }
            }

            return mainSkill;
        }

        public void SetEnemySouls(EnemySoul[] enemySouls)
        {
            this.enemySouls = new EnemySoulData[enemySouls.Length];
            for (int i = 0; i < enemySouls.Length; i++)
            {
                this.enemySouls[i] = new EnemySoulData(enemySouls[i]);
            }
        }

        public EnemySoulData GetEnemySoul(string name)
        {
            foreach (EnemySoulData enemySoulData in enemySouls)
            {
                if (enemySoulData.name == name)
                {
                    return enemySoulData;
                }
            }

            return null;
        }

        public void CreateInventory()
        {
            inventory = new InventoryData();
        }

        public EnemySoul[] LoadEnemySouls()
        {
            EnemySoul[] enemySoulScripts = new EnemySoul[enemySouls.Length];
            for (int i = 0; i < enemySouls.Length; i++)
            {
                enemySoulScripts[i] = new EnemySoul(enemySouls[i].name, enemySouls[i].enemy, enemySouls[i].description,
                    enemySouls[i].level, enemySouls[i].count, enemySouls[i].wholeCount, enemySouls[i].gap,
                    enemySouls[i].toNextLevel);
                Debug.Log(enemySoulScripts[i].GetEnemy());
            }

            return enemySoulScripts;
        }

        public PlayerSkill[] LoadWeaponFamilySkills()
        {
            PlayerSkill[] playerSkillScripts = new PlayerSkill[weaponFamilySkills.Length];
            for (int i = 0; i < weaponFamilySkills.Length; i++)
            {
                playerSkillScripts[i] = new PlayerSkill(weaponFamilySkills[i].name, weaponFamilySkills[i].family,
                    weaponFamilySkills[i].description, weaponFamilySkills[i].level, weaponFamilySkills[i].experience,
                    weaponFamilySkills[i].wholeExperience, weaponFamilySkills[i].gap, weaponFamilySkills[i].toNextLevel);
            }

            return playerSkillScripts;
        }

        public PlayerSkill LoadMainSkill()
        {
            return new PlayerSkill(mainSkill.name, mainSkill.family, mainSkill.description, mainSkill.level,
                mainSkill.experience, mainSkill.wholeExperience, mainSkill.gap, mainSkill.toNextLevel);
        }

        public void SetMainSkill(PlayerSkill playerSkill)
        {
            mainSkill = new PlayerSkillData(playerSkill);
        }

        public void SetWeaponFamilySkills(PlayerSkill[] playerSkills)
        {
            weaponFamilySkills = new PlayerSkillData[playerSkills.Length];
            for (int i = 0; i < playerSkills.Length; i++)
            {
                weaponFamilySkills[i] = new PlayerSkillData(playerSkills[i]);
            }
        }
              
        public void SetBank(int gold, int goldCap, int goldCapUpgradeCost, int goldCapUpgradeCostMultiplier)
        {
            bankGold = gold;
            bankGoldCap = goldCap;
            bankGoldCapUpgradeCost = goldCapUpgradeCost;
            bankGoldCapUpgradeCostMultiplier = goldCapUpgradeCostMultiplier;
        }
    }

    [Serializable]
    public class EngravingData
    {

    }
}
