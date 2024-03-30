using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Dungeoneer
{
    public class Player : MonoBehaviour, IDataPersistence
    {
        private Inventory _inventory;

        [Header("Weapon family skills")] [SerializeField]
        private PlayerSkill mainSkill;

        private PlayerSkill[] weaponFamilySkills;

        [Header("Enemy family skills")] private EnemySoul[] enemySouls;

        [Header("Player Info")] [SerializeField]
        private string playerName;

        private int _playerID;

        [Header("Player Stats")] private float _luck;
        [SerializeField] private float baseLuck;
        private float _strength;
        [SerializeField] private float baseStrength;
        private float _dexterity;
        [SerializeField] private float baseDexterity;

        [Header("Weapon Stats")] [SerializeField]
        private float bonusAttackSpeed;

        [SerializeField] private float bonusCritChance;
        [SerializeField] private float bonusCritMultiplier;

        [Header("Armor Stats")] private float _maxHealth;
        [SerializeField] private float baseMaxHealth;
        private float _healthRegen;
        [SerializeField] private float baseHealthRegen;
        private float _maxMana;
        [SerializeField] private float baseMaxMana = 100;
        private float _manaRegen;
        [SerializeField] private float baseManaRegen;
        private float _defense;
        [SerializeField] private float baseDefense;
        [SerializeField] private float bonusMoveSpeed;

        private void Start()
        {
            UpdateAttackWeapon();
            UpdatePlayer();
            UpdateStats();
        }

        public void UpdateStats()
        {
            _inventory = gameObject.GetComponent<Inventory>();
            _strength = 0;
            _dexterity = 0;
            bonusAttackSpeed = 0;
            bonusCritChance = 0;
            bonusCritMultiplier = 0;
            _defense = 0;
            bonusMoveSpeed = 0;
            _luck = 0;
            _maxHealth = 0;
            _healthRegen = 0;
            _maxMana = 0;
            _manaRegen = 0;

            
            foreach (var artifact in _inventory.GetArtifacts())
            {
                if (artifact.Value != null)
                {
                    _maxHealth += artifact.Value.GetComponent<Artifact>().GetBonusHealth();
                    _healthRegen += artifact.Value.GetComponent<Artifact>().GetHealthRegen();
                    _maxMana += artifact.Value.GetComponent<Artifact>().GetBonusMana();
                    _manaRegen += artifact.Value.GetComponent<Artifact>().GetManaRegen();
                    _strength += artifact.Value.GetComponent<Artifact>().GetStrength();
                    _dexterity += artifact.Value.GetComponent<Artifact>().GetDexterity();
                    bonusAttackSpeed += artifact.Value.GetComponent<Artifact>().GetBonusAttackSpeed();
                    bonusCritChance += artifact.Value.GetComponent<Artifact>().GetBonusCritChance();
                    bonusCritMultiplier += artifact.Value.GetComponent<Artifact>().GetBonusCritMultiplier();
                    _defense += artifact.Value.GetComponent<Artifact>().GetDefense();
                    bonusMoveSpeed += artifact.Value.GetComponent<Artifact>().GetBonusMovementSpeed();
                    _luck += artifact.Value.GetComponent<Artifact>().GetLuck();
                }
            }

            if (_inventory.GetEquippedHelmet() != null)
            {
                _defense += _inventory.GetEquippedHelmetScript().GetDefense();
                _maxHealth += _inventory.GetEquippedHelmetScript().GetBonusHealth();
                _healthRegen += _inventory.GetEquippedHelmetScript().GetHealthRegen();
                _maxMana += _inventory.GetEquippedHelmetScript().GetBonusMana();
                _manaRegen += _inventory.GetEquippedHelmetScript().GetManaRegen();
                bonusMoveSpeed += _inventory.GetEquippedHelmetScript().GetBonusMovementSpeed();
                _luck += _inventory.GetEquippedHelmetScript().GetLuck();
                _strength += _inventory.GetEquippedHelmetScript().GetStrength();
                _dexterity += _inventory.GetEquippedHelmetScript().GetDexterity();
                bonusAttackSpeed += _inventory.GetEquippedHelmetScript().GetBonusAttackSpeed();
                bonusCritChance += _inventory.GetEquippedHelmetScript().GetBonusCritChance();
                bonusCritMultiplier += _inventory.GetEquippedHelmetScript().GetBonusCritMultiplier();
            }

            if (_inventory.GetEquippedChestplate() != null)
            {
                _defense += _inventory.GetEquippedChestplateScript().GetDefense();
                _maxHealth += _inventory.GetEquippedChestplateScript().GetBonusHealth();
                _healthRegen += _inventory.GetEquippedChestplateScript().GetHealthRegen();
                _maxMana += _inventory.GetEquippedChestplateScript().GetBonusMana();
                _manaRegen += _inventory.GetEquippedChestplateScript().GetManaRegen();
                bonusMoveSpeed += _inventory.GetEquippedChestplateScript().GetBonusMovementSpeed();
                _luck += _inventory.GetEquippedChestplateScript().GetLuck();
                _strength += _inventory.GetEquippedChestplateScript().GetStrength();
                _dexterity += _inventory.GetEquippedChestplateScript().GetDexterity();
                bonusAttackSpeed += _inventory.GetEquippedChestplateScript().GetBonusAttackSpeed();
                bonusCritChance += _inventory.GetEquippedChestplateScript().GetBonusCritChance();
                bonusCritMultiplier += _inventory.GetEquippedChestplateScript().GetBonusCritMultiplier();
            }

            if (_inventory.GetEquippedLeggings() != null)
            {
                _defense += _inventory.GetEquippedLeggingsScript().GetDefense();
                _maxHealth += _inventory.GetEquippedLeggingsScript().GetBonusHealth();
                _healthRegen += _inventory.GetEquippedLeggingsScript().GetHealthRegen();
                _maxMana += _inventory.GetEquippedLeggingsScript().GetBonusMana();
                _manaRegen += _inventory.GetEquippedLeggingsScript().GetManaRegen();
                bonusMoveSpeed += _inventory.GetEquippedLeggingsScript().GetBonusMovementSpeed();
                _luck += _inventory.GetEquippedLeggingsScript().GetLuck();
                _strength += _inventory.GetEquippedLeggingsScript().GetStrength();
                _dexterity += _inventory.GetEquippedLeggingsScript().GetDexterity();
                bonusAttackSpeed += _inventory.GetEquippedLeggingsScript().GetBonusAttackSpeed();
                bonusCritChance += _inventory.GetEquippedLeggingsScript().GetBonusCritChance();
                bonusCritMultiplier += _inventory.GetEquippedLeggingsScript().GetBonusCritMultiplier();
            }

            if (_inventory.GetEquippedBoots() != null)
            {
                _defense += _inventory.GetEquippedBootsScript().GetDefense();
                _maxHealth += _inventory.GetEquippedBootsScript().GetBonusHealth();
                _healthRegen += _inventory.GetEquippedBootsScript().GetHealthRegen();
                _maxMana += _inventory.GetEquippedBootsScript().GetBonusMana();
                _manaRegen += _inventory.GetEquippedBootsScript().GetManaRegen();
                bonusMoveSpeed += _inventory.GetEquippedBootsScript().GetBonusMovementSpeed();
                _luck += _inventory.GetEquippedBootsScript().GetLuck();
                _strength += _inventory.GetEquippedBootsScript().GetStrength();
                _dexterity += _inventory.GetEquippedBootsScript().GetDexterity();
                bonusAttackSpeed += _inventory.GetEquippedBootsScript().GetBonusAttackSpeed();
                bonusCritChance += _inventory.GetEquippedBootsScript().GetBonusCritChance();
                bonusCritMultiplier += _inventory.GetEquippedBootsScript().GetBonusCritMultiplier();
            }

            if (_inventory.GetEquippedWeapon() != null)
            {
                _defense += _inventory.GetEquippedWeaponScript().GetDefense();
                _maxHealth += _inventory.GetEquippedWeaponScript().GetBonusHealth();
                _healthRegen += _inventory.GetEquippedWeaponScript().GetHealthRegen();
                _maxMana += _inventory.GetEquippedWeaponScript().GetBonusMana();
                _manaRegen += _inventory.GetEquippedWeaponScript().GetManaRegen();
                bonusMoveSpeed += _inventory.GetEquippedWeaponScript().GetBonusMovementSpeed();
                _luck += _inventory.GetEquippedWeaponScript().GetLuck();
                _strength += _inventory.GetEquippedWeaponScript().GetStrength();
                _dexterity += _inventory.GetEquippedWeaponScript().GetDexterity();
                bonusAttackSpeed += _inventory.GetEquippedWeaponScript().GetBonusAttackSpeed();
                bonusCritChance += _inventory.GetEquippedWeaponScript().GetBonusCritChance();
                bonusCritMultiplier += _inventory.GetEquippedWeaponScript().GetBonusCritMultiplier();
            }
            
            SetMaxHealth(GetMaxHealth());
            SetHealthRegen(GetHealthRegen());
            gameObject.GetComponent<PlayerMovement>().UpdateArmorHashes();
            gameObject.GetComponent<PlayerMovement>().UpdateStats();
        }

        public void SetMaxHealth(float health)
        {
            gameObject.GetComponent<Health>().SetMaxHealth(health);
        }

        public void SetHealthRegen(float regen)
        {
            gameObject.GetComponent<Health>().SetHealthRegen(regen);
        }

        public float GetStrength()
        {
            return baseStrength + _strength;
        }

        public float GetDexterity()
        {
            return baseDexterity + _dexterity;
        }

        public float GetLuck()
        {
            return baseLuck + _luck;
        }

        public float GetDefense()
        {
            return baseDefense + _defense;
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

        public float GetBonusMoveSpeed()
        {
            return bonusMoveSpeed;
        }

        public float GetMaxHealth()
        {
            return baseMaxHealth + _maxHealth;
        }

        public float GetHealthRegen()
        {
            return baseHealthRegen + _healthRegen;
        }

        public float GetMaxMana()
        {
            return baseMaxMana + _maxMana;
        }

        public float GetManaRegen()
        {
            return baseManaRegen + _manaRegen;
        }

        public void UpdateAttackWeapon()
        {
            _inventory = gameObject.GetComponent<Inventory>();
            gameObject.GetComponent<PlayerAttack>().UpdateWeapon(_inventory.GetEquippedWeapon());
        }

        public void UpdateInventory()
        {
            _inventory = gameObject.GetComponent<Inventory>();
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }

        public string GetPlayerStats()
        {
            UpdateStats();
            string stats = "<b>" + GetPlayerName() + "</b> \n\n";
            stats += "Max Health: <gradient=\"StatHealthGradient\">" + GetMaxHealth() + "</gradient>\n";
            stats += "Health Regen: <gradient=\"StatHealthRegenGradient\">" + GetHealthRegen() + "/s </gradient>\n";
            stats += "Max Mana: <gradient=\"StatManaGradient\">" + GetMaxMana() + "</gradient>\n";
            stats += "Mana Regen: <gradient=\"StatManaRegenGradient\">" + GetManaRegen() + "/s </gradient>\n";
            stats += "Dexterity: <gradient=\"StatDexterityGradient\">" + GetDexterity() + "</gradient>\n";
            stats += "Strength: <gradient=\"StatStrengthGradient\">" + GetStrength() + "</gradient>\n";
            stats += "Luck: <gradient=\"StatLuckGradient\">" + GetLuck() + "</gradient>\n";
            stats += "Defense: <gradient=\"StatArmorGradient\">" + GetDefense() + "</gradient>\n";
            stats += "Attack Speed: <gradient=\"StatAttackSpeedGradient\">" + GetBonusAttackSpeed() + "</gradient>\n";
            stats += "Crit Chance: <gradient=\"StatCritChanceGradient\">" + GetBonusCritChance() + "</gradient>\n";
            stats += "Crit Multiplier: <gradient=\"StatCritMultiplierGradient\">" + GetBonusCritMultiplier() + "</gradient>\n";
            stats += "Move Speed: <gradient=\"StatMoveSpeedGradient\">" + GetBonusMoveSpeed() + "</gradient>\n";

            return stats;
        }

        private string GetPlayerName()
        {
            return playerName;
        }

        public PlayerSkill GetPlayerSkills(Enums.WeaponFamily type = Enums.WeaponFamily.None)
        {
            foreach (var skill in weaponFamilySkills)
            {
                if (skill.GetFamily() == (Enums.SkillType)type)
                    return skill;
            }
            return mainSkill;
        }
        
        public PlayerSkill GetPlayerSkillsFromType(Enums.SkillType type = Enums.SkillType.Main)
        {
            Debug.Log(weaponFamilySkills.Length);
            foreach (var skill in weaponFamilySkills)
            {
                if (skill.GetFamily() == type)
                    return skill;
            }
            return mainSkill;
        }
        
        public EnemySoul GetEnemySkillsFromFamily(Enums.EnemyFamily type)
        {
            foreach (var soul in enemySouls)
            {
                if (soul.GetEnemy() == type)
                    return soul;
            }
            return null;
        }

        public PlayerSkill[] GetPlayerWeaponSkills()
        {
            return weaponFamilySkills;
        }

        public EnemySoul GetPlayerSouls(Enums.EnemyFamily type)
        {
            foreach (var soul in enemySouls)
            {
                if (soul.GetEnemy() == type)
                    return soul;
            }

            return null;
        }

        public void AddExpAndSouls(Enemy enemy = null)
        {
            mainSkill.AddExperience(enemy.GetExpReward());

            Debug.Log(enemySouls.Length);
            foreach (var soul in enemySouls)
            {
                if (soul.GetEnemy() == enemy.GetFamily())
                    soul.AddSoul(enemy.GetSouls());
            }

            //foreach (var quest in _inventory.GetQuests())
            //{
            //    if (quest.GetComponent<Quest>().GetRequiredExp() > 0 &&
            //        quest.GetComponent<Quest>().GetRequiredEnemy() == enemy.GetFamily())
            //        quest.GetComponent<Quest>().AddExp(enemy.GetExpReward());
            //}

            foreach (var skill in weaponFamilySkills)   
            {
                if ((Enums.SkillType)_inventory.GetEquippedWeaponScript().GetWeaponFamily() == skill.GetFamily())
                    skill.AddExperience(enemy.GetExpReward());
            }   
        }

        public void AddGold(int goldReward)
        {
            _inventory.AddGold(goldReward);
        }

        public string GetName()
        {
            return playerName;
        }

        public int GetPlayerID()
        {
            return _playerID;
        }

        public void UpdatePlayer()
        {
            _inventory = gameObject.GetComponent<Inventory>();
            UpdateStats();
            UpdateInventory();
            UpdateAttackWeapon();
        }

        public void LoadData(GameData data)
        {
            baseStrength = data.playerData.baseStrength;
            baseDexterity = data.playerData.baseDexterity;
            baseLuck = data.playerData.baseLuck;
            baseDefense = data.playerData.baseDefense;
            baseHealthRegen = data.playerData.baseHealthRegen;
            baseMaxHealth = data.playerData.baseMaxHealth;
            baseManaRegen = data.playerData.baseManaRegen;
            baseMaxMana = data.playerData.baseMaxMana;
            bonusMoveSpeed = data.playerData.bonusMoveSpeed;
            playerName = data.playerData.playerName;
            _playerID = data.playerData.playerID;
            enemySouls = data.playerData.LoadEnemySouls();
            mainSkill = data.playerData.LoadMainSkill();
            weaponFamilySkills = data.playerData.LoadWeaponFamilySkills();
            gameObject.GetComponent<Health>().SetMaxHealth(GetMaxHealth());
            gameObject.GetComponent<Health>().SetHealthRegen(GetHealthRegen());
        }

        public void SaveData(GameData data) 
        {
            data.playerData.baseStrength = baseStrength;
            data.playerData.baseDexterity = baseDexterity;
            data.playerData.baseLuck = baseLuck;
            data.playerData.baseDefense = baseDefense;
            data.playerData.baseHealthRegen = baseHealthRegen;
            data.playerData.baseMaxHealth = baseMaxHealth;
            data.playerData.baseManaRegen = baseManaRegen;
            data.playerData.baseMaxMana = baseMaxMana;
            data.playerData.bonusMoveSpeed = bonusMoveSpeed;
            data.playerData.playerName = playerName;
            data.playerData.playerID = _playerID;
            data.playerData.SetEnemySouls(enemySouls);
            data.playerData.SetMainSkill(mainSkill);
            data.playerData.SetWeaponFamilySkills(weaponFamilySkills);
        }
    }
}