using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Dungeoneer
{
    public class Inventory : MonoBehaviour, IDataPersistence
    {
        public int maxSlots = 30;
        [Header("Inventory")] Dictionary<int,GameObject> items = new Dictionary<int,GameObject>();
        [SerializeField] Dictionary<int,GameObject> armor = new Dictionary<int,GameObject>();
        [SerializeField] Dictionary<int,GameObject> weapons = new Dictionary<int,GameObject>();
        [SerializeField] Dictionary<int,GameObject> artifacts = new Dictionary<int,GameObject>();
        // TODO : ARTIFACTS
        [SerializeField] List<GameObject> equippedArtifacts;
        [SerializeField] int equippedArtifactSlots = 9;
        [SerializeField] new Dictionary<int,GameObject> abilities = new Dictionary<int,GameObject>();
        [SerializeField] new Dictionary<int,GameObject> consumables = new Dictionary<int,GameObject>();
        [SerializeField] List<GameObject> quests = new List<GameObject>();
        [SerializeField] private int gold;
        [SerializeField] private int stars;
        [Header("Equipped")] [SerializeField] private GameObject equippedWeapon;
        private Weapon equippedWeaponScript;
        [SerializeField] private GameObject equippedAbility;
        private Ability equippedAbilityScript;

        [SerializeField] private GameObject equippedHelmet;
        private Armor equippedHelmetScript;
        [SerializeField] private GameObject equippedChestplate;
        private Armor equippedChestplateScript;
        [SerializeField] private GameObject equippedLeggings;
        private Armor equippedLeggingsScript;
        [SerializeField] private GameObject equippedBoots;
        private Armor equippedBootsScript;
        [SerializeField] private GameObject equippedNecklace;
        private Armor equippedNecklaceScript;
        [SerializeField] private GameObject equippedGloves;
        private Armor equippedGlovesScript;

        
        private GameObject pickupItem;
        private GameObject upgradeItem;
        private NPCUpgrader _upgrader;
        private Player _player;
        private Transform itemParent;
        private void Start()
        {
            itemParent = GameObject.Find("ItemParent").transform;

            //goldText.text = gold.ToString();
        }

        public void AddItem(GameObject item, int slot = -1)
        {
            UIManager.Instance.PickUpItem(item);
            GameObject itemGO = Instantiate(item,itemParent);
            itemGO.transform.position = new Vector3(-9999, -9999, -99);
            itemGO.GetComponent<Item>().GenerateLocalID();
            Debug.Log(itemGO.GetComponent<Item>().GetItemType());

            switch (item.GetComponent<Item>().GetItemType())
            {
                case Enums.ItemType.Ability:
                    if (CanFitItem(abilities))
                    {
                        slot = IsSlotEmpty(abilities, slot);
                        AddAbilityToInventory(itemGO,slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
                case Enums.ItemType.Artifact:
                    if (CanFitItem(artifacts))
                    {
                        slot = IsSlotEmpty(artifacts, slot);
                        AddArtifactToInventory(itemGO,slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
                case Enums.ItemType.Armor:
                    if (CanFitItem(armor))
                    {
                        slot = IsSlotEmpty(armor, slot);
                        AddArmorToInventory(itemGO, slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
                case Enums.ItemType.Weapon:
                    if (CanFitItem(weapons))
                    {
                        slot = IsSlotEmpty(weapons, slot);
                        AddWeaponToInventory(itemGO, slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
                case Enums.ItemType.Consumable:
                    if (CanFitItem(consumables))
                    {
                        slot = IsSlotEmpty(consumables, slot);
                        AddConsumableToInventory(itemGO, slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
                default:
                    if (CanFitItem(items))
                    {
                        slot = IsSlotEmpty(items, slot);
                        AddItemToInventory(itemGO, slot);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
                    break;
            }
        }

        private int IsSlotEmpty(Dictionary<int, GameObject> gameObjects, int slot)
        {
            if (gameObjects.ContainsKey(slot))
            {
                for (int i = 0; i < maxSlots; i++)
                {
                    if (!gameObjects.ContainsKey(i))
                    {
                        slot = i;
                        break;
                    }
                }
            }

            return slot;
        }


        public void AddItems(Dictionary<int, GameObject> items)
        {
            itemParent = GameObject.Find("ItemParent").transform;
            foreach (var item in items)
            {
                    AddItem(item.Value, item.Key);
            }
        }
        
        public bool CanFitItem(Dictionary<int,GameObject> inventory)
        {
            return inventory.Count < maxSlots;
        }

        public void RemoveItem(GameObject item, int slot)
        {
            switch (item.GetComponent<Item>().GetItemType())
            {
                case Enums.ItemType.Ability:
                    RemoveAbilityFromInventory(slot);
                    break;
                case Enums.ItemType.Artifact:
                    RemoveArtifactFromInventory(slot);
                    break;
                case Enums.ItemType.Armor:
                    RemoveArmorFromInventory(slot);
                    break;
                case Enums.ItemType.Weapon:
                    RemoveWeaponFromInventory(slot);
                    break;
                default:
                    RemoveItemFromInventory(slot);
                    break;
            }
        }
        public void RemoveItem(GameObject item)
        {
            switch (item.GetComponent<Item>().GetItemType())
            {
                case Enums.ItemType.Ability:
                    RemoveAbilityFromInventory(item);
                    break;
                case Enums.ItemType.Artifact:
                    RemoveArtifactFromInventory(item);
                    break;
                case Enums.ItemType.Armor:
                    RemoveArmorFromInventory(item);
                    break;
                case Enums.ItemType.Weapon:
                    RemoveWeaponFromInventory(item);
                    break;
                default:
                    RemoveItemFromInventory(item);
                    break;
            }
        }

        public int GetGold()
        {
            return gold;
        }

        public void AddGold(int amount)
        {
            gold += amount;
        }

        public void RemoveGold(int amount)
        {
            gold -= amount;
        }

        public int GetStars()
        {
            return stars;
        }

        public void AddStars(int amount)
        {
            stars += amount;
        }

        public void RemoveStars(int amount)
        {
            stars -= amount;
        }

        public Weapon GetEquippedWeaponScript()
        {
            if (equippedWeapon != null)
            {
                equippedWeaponScript = equippedWeapon.GetComponent<Weapon>();
                return equippedWeaponScript;
            }
            
            return gameObject.GetComponent<PlayerAttack>().GetDefaultWeapon();
        }

        public GameObject GetEquippedWeapon()
        {
            return equippedWeapon;
        }

        public Ability GetEquippedAbilityScript()
        {
            equippedAbilityScript = equippedAbility.GetComponent<Ability>();
            return equippedAbilityScript;
        }

        public GameObject GetEquippedAbility()
        {
            return equippedAbility;
        }

        public Armor GetEquippedHelmetScript()
        {
            if(equippedHelmet == null)
                return null;
            equippedHelmetScript = equippedHelmet.GetComponent<Armor>();
            return equippedHelmetScript;
        }

        public GameObject GetEquippedHelmet()
        {
            return equippedHelmet;
        }

        public Armor GetEquippedChestplateScript()
        {
            if(equippedChestplate == null)
                return null;
            equippedChestplateScript = equippedChestplate.GetComponent<Armor>();
            return equippedChestplateScript;
        }

        public GameObject GetEquippedChestplate()
        {
            return equippedChestplate;
        }

        public Armor GetEquippedLeggingsScript()
        {
            if(equippedLeggings == null)
                return null;
            equippedLeggingsScript = equippedLeggings.GetComponent<Armor>();
            return equippedLeggingsScript;
        }

        public GameObject GetEquippedLeggings()
        {
            return equippedLeggings;
        }

        public Armor GetEquippedBootsScript()
        {
            if (equippedBootsScript == null)
                return null;
            equippedBootsScript = equippedBoots.GetComponent<Armor>();
            return equippedBootsScript;
        }

        public GameObject GetEquippedBoots()
        {
            return equippedBoots;
        }

        public Armor GetEquippedNecklaceScript()
        {
            return equippedNecklaceScript;
        }

        public GameObject GetEquippedNecklace()
        {
            return equippedNecklace;
        }

        public Armor GetEquippedGlovesScript()
        {
            return equippedGlovesScript;
        }

        public GameObject GetEquippedGloves()
        {
            return equippedGloves;
        }

        public Dictionary<int,GameObject> GetArtifacts()
        {
            return artifacts;
        }

        // TODO : ARTIFACTS
        //public List<GameObject> GetEquippedArtifacts()
        //{
        //    return equippedArtifacts;
        //}

        public Dictionary<int,GameObject> GetAbilities()
        {
            return abilities;
        }

        public Dictionary<int,GameObject> GetArmor()
        {
            return armor;
        }

        public Dictionary<int,GameObject> GetWeapons()
        {
            return weapons;
        }

        public Dictionary<int,GameObject> GetItems()
        {
            return items;
        }

        public Dictionary<int,GameObject> GetConsumables()
        {
            return consumables;
        }

        public List<GameObject> GetQuests()
        {
            return quests;
        }

        public void EquipWeapon(GameObject weapon, bool inInventory = true)
        {
            if (weapon == null)
            {
                UIManager.Instance.DisplayMessage(
                    "Tried equipping null weapon.", UIManager.PopUpType.Error);
                return;
            }
            if (_player.GetPlayerSkills(weapon.GetComponent<Weapon>().GetWeaponFamily()).GetLevel() <=
                     weapon.GetComponent<Weapon>().GetRequiredLevel())
            {
                UIManager.Instance.DisplayMessage("You are not high enough level to equip this weapon!", UIManager.PopUpType.Error);
                return;
            }
            if (equippedWeapon != null)
            {
                UIManager.Instance.DisplayMessage(
                    "Unequipped : " + equippedWeapon.GetComponent<Item>().GetName() + 
                    "\nEquipped : " + weapon.GetComponent<Item>().GetName());
            }
            else
            {
                UIManager.Instance.DisplayMessage(
                    "Equipped " + weapon.GetComponent<Item>().GetName());
            }

            
            
            GameObject itemGO = Instantiate(weapon, itemParent);
            itemGO.transform.position = new Vector3(-9999, -9999, -99);
            itemGO.GetComponent<Item>().GenerateLocalID();

            if (inInventory)
            {
                RemoveWeaponFromInventory(weapon);
            }
            if (equippedWeapon != null)
            {
                AddWeaponToInventory(equippedWeapon);
            }
            
            equippedWeapon = itemGO;
            equippedWeaponScript = equippedWeapon.GetComponent<Weapon>();
            
            gameObject.GetComponent<PlayerAttack>().UpdateWeapon(equippedWeapon);
            _player = gameObject.GetComponent<Player>();
            _player.UpdateStats();
        }

        public void UnequipWeapon()
        {
            UIManager.Instance.DisplayMessage("Unequipped " + equippedWeapon.GetComponent<Item>().GetName(),
                UIManager.PopUpType.Success);
            AddWeaponToInventory(equippedWeapon);
            equippedWeapon = null;
            equippedWeaponScript = null;

            gameObject.GetComponent<PlayerAttack>().UpdateWeapon(null);
            _player.UpdateStats();
        }

        public void EquipArmor(GameObject armorPiece,int slot = 0, bool inInventory = true)
        {
            if(armorPiece == null)
            {
                UIManager.Instance.DisplayMessage("Tried equipping null armor!", UIManager.PopUpType.Error);
                return;
            }
            if (armorPiece.GetComponent<Item>().GetRequiredLevel() > _player.GetPlayerSkills().GetLevel())
            {
                UIManager.Instance.DisplayMessage("You are not high enough level to equip this armor!", UIManager.PopUpType.Error);
                return;
            }
            
            GameObject itemGO = Instantiate(armorPiece, itemParent);
            itemGO.transform.position = new Vector3(-9999, -9999, -99);
            itemGO.GetComponent<Item>().GenerateLocalID();
            
            if(inInventory)
                RemoveArmorFromInventory(slot);
            
            switch (armorPiece.GetComponent<Armor>().GetItemSlot())
            {
                case Enums.ItemSlot.Helmet:
                    if (equippedHelmet != null)
                    {
                        AddItem(equippedHelmet,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedHelmet.GetComponent<Item>().GetName() + "\n" +
                            "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedHelmet = itemGO;
                    equippedHelmetScript = equippedHelmet.GetComponent<Armor>();
                    break;
                case Enums.ItemSlot.Chestplate:
                    if (equippedChestplate != null)
                    {
                        AddItem(equippedChestplate,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedChestplate.GetComponent<Item>().GetName() + "\n" +
                                                          "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedChestplate = itemGO;
                    equippedChestplateScript = equippedChestplate.GetComponent<Armor>();
                    break;
                case Enums.ItemSlot.Leggings:
                    if (equippedLeggings != null)
                    {
                        AddItem(equippedLeggings,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedLeggings.GetComponent<Item>().GetName() + "\n" +
                                                          "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedLeggings = itemGO;
                    equippedLeggingsScript = equippedLeggings.GetComponent<Armor>();
                    break;
                case Enums.ItemSlot.Boots:
                    if (equippedBoots != null)
                    {
                        AddItem(equippedBoots,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedBoots.GetComponent<Item>().GetName() + "\n" +
                                                          "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedBoots = itemGO;
                    equippedBootsScript = equippedBoots.GetComponent<Armor>();
                    break;
                case Enums.ItemSlot.Necklace:
                    if (equippedNecklace != null)
                    {
                        AddItem(equippedNecklace,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedNecklace.GetComponent<Item>().GetName() + "\n" +
                                                          "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedNecklace = itemGO;
                    equippedNecklaceScript = equippedNecklace.GetComponent<Armor>();
                    break;
                case Enums.ItemSlot.Gloves:
                    if (equippedGloves != null)
                    {
                        AddItem(equippedGloves,slot);
                        UIManager.Instance.DisplayMessage("Unequipped : " + equippedGloves.GetComponent<Item>().GetName() + "\n" +
                                                          "Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    }
                    else
                        UIManager.Instance.DisplayMessage("Equipped : " + armorPiece.GetComponent<Item>().GetName(),
                            UIManager.PopUpType.Success);
                    equippedGloves = itemGO;
                    equippedGlovesScript = equippedGloves.GetComponent<Armor>();
                    break;
            }
            
            _player.UpdateStats();
        }
        
        public void EquipArtifact(GameObject artifact, bool inInventory = true)
        {
            if(artifact == null)
                return;
            
            if (equippedArtifacts.Count < equippedArtifactSlots)
            {
                equippedArtifacts.Add(artifact);
                if(inInventory)
                    RemoveArtifactFromInventory(artifact);
            }
            else
            {
                UIManager.Instance.DisplayMessage("Artifact pouch is full!", UIManager.PopUpType.Error);
            }
        }
        
        public void UnequipArtifact(GameObject artifact)
        {
            if (equippedArtifacts.Count > maxSlots)
            {
                equippedArtifacts.Remove(artifact);
                AddArtifactToInventory(artifact);
            }
            else
            {
                UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
            }
        }

        public void EquipAbility(GameObject ability, bool inInventory = true)
        {
            if(ability == null)
                return;
            
            equippedAbility = ability;
            equippedAbilityScript = equippedAbility.GetComponent<Ability>();
            if (inInventory)
            {
                AddAbilityToInventory(ability);
                RemoveAbilityFromInventory(ability);
            }
            _player.UpdateStats();
        }

        // TODO : ARTIFACTS
        //public void EquipArtifact(GameObject artifact)
        //{
        //    if (equippedArtifacts.Count < equippedArtifactSlots)
        //    {
        //        equippedArtifacts.Add(artifact);
        //        RemoveArtifactFromInventory(artifact);
        //    }
        //    else
        //    {
        //        UIManager.Instance.DisplayMessage("Artifact pouch is full!", UIManager.PopUpType.Error);
        //    }
        //}
//
        //public void UnequipArtifact(GameObject artifact)
        //{
        //    if (equippedArtifacts.Count > maxSlots)
        //    {
        //        equippedArtifacts.Remove(artifact);
        //        AddArtifactToInventory(artifact);
        //    }
        //    else
        //    {
        //        UIManager.Instance.DisplayMessage("Inventory is full!", UIManager.PopUpType.Error);
        //    }
        //}

        public IEnumerable<Item> GetEquippedArmorScripts()
        {
            return new List<Item>
            {
                equippedHelmetScript,
                equippedChestplateScript,
                equippedLeggingsScript,
                equippedBootsScript,
                equippedNecklaceScript,
                equippedGlovesScript
            };
        }

        public IEnumerable<GameObject> GetEquippedArmor()
        {
            return new List<GameObject>
            {
                equippedHelmet,
                equippedChestplate,
                equippedLeggings,
                equippedBoots,
                equippedNecklace,
                equippedGloves
            };
        }

        public void UnequipArmor(Enums.ItemSlot slotType, int slot = -1)
        {
            switch (slotType)
            {
                case Enums.ItemSlot.Helmet:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedHelmet.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedHelmet,slot);
                    equippedHelmet = null;
                    equippedHelmetScript = null;
                    break;
                case Enums.ItemSlot.Chestplate:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedChestplate.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedChestplate,slot);
                    equippedChestplate = null;
                    equippedChestplateScript = null;
                    break;
                case Enums.ItemSlot.Leggings:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedLeggings.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedLeggings,slot);
                    equippedLeggings = null;
                    equippedLeggingsScript = null;
                    break;
                case Enums.ItemSlot.Boots:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedBoots.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedBoots,slot);
                    equippedBoots = null;
                    equippedBootsScript = null;
                    break;
                case Enums.ItemSlot.Necklace:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedNecklace.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedNecklace,slot);
                    equippedNecklace = null;
                    equippedNecklaceScript = null;
                    break;
                case Enums.ItemSlot.Gloves:
                    UIManager.Instance.DisplayMessage("Unequipped " + equippedGloves.GetComponent<Item>().GetName(),
                        UIManager.PopUpType.Success);
                    AddItem(equippedGloves,slot);
                    equippedGloves = null;
                    equippedGlovesScript = null;
                    break;
            }

            _player.UpdateStats();
        }

        public void UnequipAbility()
        {
            equippedAbility = null;
            equippedAbilityScript = null;
            _player.UpdateStats();
        }
        
        public void UnequipItem(GameObject item)
        {
            if(item.TryGetComponent<Weapon>(out var weapon))
                UnequipWeapon();
            else if (item.TryGetComponent<Ability>(out var ability))
                UnequipAbility();
            //else if(item.TryGetComponent<Artifact>(out var artifact))
            //    UnequipArtifact(item);
            else
                UIManager.Instance.DisplayMessage("Cannot unequip this item!", UIManager.PopUpType.Error);
        }
        
        public void EquipItem(GameObject item)
        {
            if(item.TryGetComponent<Armor>(out var armor))
                EquipArmor(item);
            else if(item.TryGetComponent<Weapon>(out var weapon))
                EquipWeapon(item);
            else if (item.TryGetComponent<Ability>(out var ability))
                EquipAbility(item);
            //else if(item.TryGetComponent<Artifact>(out var artifact))
            //    EquipArtifact(item);
            else
                UIManager.Instance.DisplayMessage("Cannot equip this item!", UIManager.PopUpType.Error);
        }

        public void RemoveWeaponFromInventory(int slot)
        {
            weapons.Remove(slot);
        }
        public void RemoveWeaponFromInventory(GameObject item)
        {
            weapons.Remove(weapons.FirstOrDefault(
                x => x.Value.GetComponent<Item>().GetLocalID()
                     == item.GetComponent<Item>().GetLocalID()).Key);
        }

        public void RemoveArmorFromInventory(int slot)
        {
            armor.Remove(slot);
        }
        public void RemoveArmorFromInventory(GameObject item)
        {
            armor.Remove(armor.FirstOrDefault(
                x => x.Value.GetComponent<Item>().GetLocalID()
                     == item.GetComponent<Item>().GetLocalID()).Key);
        }

        public void RemoveAbilityFromInventory(int slot)
        {
            abilities.Remove(slot);
        }
        public void RemoveAbilityFromInventory(GameObject item)
        {
            abilities.Remove(abilities.FirstOrDefault(
                x => x.Value.GetComponent<Item>().GetLocalID()
                     == item.GetComponent<Item>().GetLocalID()).Key);
        }

        public void RemoveArtifactFromInventory(int slot)
        {
            artifacts.Remove(slot);
        }
        public void RemoveArtifactFromInventory(GameObject item)
        {
            artifacts.Remove(artifacts.FirstOrDefault(
                x => x.Value.GetComponent<Item>().GetLocalID()
                     == item.GetComponent<Item>().GetLocalID()).Key);
        }

        public void RemoveItemFromInventory(int slot)
        {
            items.Remove(slot);
        }
        public void RemoveItemFromInventory(GameObject item)
        {
            items.Remove(items.FirstOrDefault(
                x => x.Value.GetComponent<Item>().GetLocalID()
                     == item.GetComponent<Item>().GetLocalID()).Key);
        }

        public void AddWeaponToInventory(GameObject weapon, int slot = -1)
        {
            AddToInventory(weapon, slot, weapons);
        }

        public void AddArmorToInventory(GameObject armorPiece, int slot = -1)
        {
            AddToInventory(armorPiece, slot, armor);
        }

        public void AddAbilityToInventory(GameObject ability, int slot = -1)
        {
            AddToInventory(ability, slot, abilities);
        }

        public void AddArtifactToInventory(GameObject artifact, int slot = -1)
        {
            AddToInventory(artifact, slot, artifacts);
        }
        
        public void AddConsumableToInventory(GameObject consumable, int slot = -1)
        {
            AddToInventory(consumable, slot, consumables);
        }

        public void AddItemToInventory(GameObject item, int slot = -1)
        {
            AddToInventory(item, slot, items);
        }

        private void AddToInventory(GameObject item, int slot, Dictionary<int, GameObject> inventory)
        {
            // If the item is stackable
            if (item.GetComponent<Item>().GetStackable())
            {
                bool exists = false;
                int amount = item.GetComponent<Item>().GetAmount();
                if (slot == -1)
                {
                    foreach (var var in inventory)
                    {
                        if (var.Value.GetComponent<Item>().GetName() == item.GetComponent<Item>().GetName())
                        {
                            if (var.Value.GetComponent<Item>().GetAmount() + amount <= item.GetComponent<Item>().GetMaxStack())
                            {
                                exists = true;
                                var.Value.GetComponent<Item>().AddAmount(amount);
                                break;
                            }
                            var.Value.GetComponent<Item>().AddAmount(item.GetComponent<Item>().GetMaxStack() - var.Value.GetComponent<Item>().GetAmount());
                            amount -= item.GetComponent<Item>().GetMaxStack() - var.Value.GetComponent<Item>().GetAmount();
                        }
                    }
                }
                if (!exists)
                {
                    if(slot > maxSlots)
                        UIManager.Instance.DisplayMessage(
                            item.GetComponent<Item>().GetItemType() +"inventory is full!",
                            UIManager.PopUpType.Error);
                    //if slot isnt set then find the lowest available slot
                    if (slot == -1)
                    {
                        //Find the lowest available slot
                        for (int i = maxSlots-1; i >= 0; i--)
                        {
                            if(!inventory.ContainsKey(i))
                                slot = i;
                        }
                    }
                    item.GetComponent<Item>().SetAmount(amount);
                    inventory.Add(slot,item);
                }
            }
            // If the item is not stackable
            else if (item != null && item.TryGetComponent<Item>(out var itemScript))
            {
                if(slot > maxSlots)
                    UIManager.Instance.DisplayMessage(
                        item.GetComponent<Item>().GetItemType() +"inventory is full!",
                        UIManager.PopUpType.Error);
                //if slot isnt set then find the lowest available slot
                if (slot == -1)
                {
                    //Find the lowest available slot
                    for (int i = maxSlots-1; i >= 0; i--)
                    {
                        if(!inventory.ContainsKey(i))
                            slot = i;
                    }
                }
                inventory.Add(slot,item);
            }
            else
            {
                UIManager.Instance.DisplayMessage(item.GetComponent<Item>().GetItemType() + 
                                                  " is null or doesnt have a weapon script",
                    UIManager.PopUpType.Error);
            }
        }

        public void AddQuest(GameObject quest)
        {
            bool exists = false;
            foreach (GameObject q in quests)
            {
                if (q.GetComponent<Quest>().GetName() == quest.GetComponent<Quest>().GetName())
                {
                    exists = true;
                }
            }

            if (!exists)
                quests.Add(quest);
        }

        public void BuyItem(GameObject getItem, int slot)
        {
            if (gold >= getItem.GetComponent<Item>().GetValue())
            {
                gold -= getItem.GetComponent<Item>().GetValue();

                AddItem(getItem,slot);
                UIManager.Instance.DisplayMessage("Purchased!", UIManager.PopUpType.Success);
            }
            else
            {
                UIManager.Instance.DisplayMessage("Not enough gold!", UIManager.PopUpType.Error);
            }
        }

        public void SellItem(GameObject item)
        {
            if(item.GetComponent<Item>().GetStackValue() / 4 < 1)
                gold += 1;
            else
                gold += item.GetComponent<Item>().GetStackValue() / 4;
            RemoveItem(item);
            UIManager.Instance.DisplayMessage("Sold for " + item.GetComponent<Item>().GetStackValue() / 4 + " G!",
                UIManager.PopUpType.Success);
        }

        public void SellItem(int index)
        {
            gold += items[index].GetComponent<Item>().GetStackValue() / 4;
            RemoveItem(items[index]);
            UIManager.Instance.DisplayMessage("Sold for " + items[index].GetComponent<Item>().GetStackValue() / 4 + " G!",
                UIManager.PopUpType.Success);
        }

        public void SellItem(Ability ability)
        {
            gold += ability.GetValue() / 4;
            RemoveAbilityFromInventory(ability.gameObject);
            UIManager.Instance.DisplayMessage("Sold for " + ability.GetValue() / 4 + " G!",
                UIManager.PopUpType.Success);
        }

        public void SellItem(Armor armor)
        {
            gold += armor.GetValue() / 4;
            RemoveArmorFromInventory(armor.gameObject);
            UIManager.Instance.DisplayMessage("Sold for " + armor.GetValue() / 4 + " G!", UIManager.PopUpType.Success);
        }

        public void SellItem(Weapon weapon)
        {
            gold += weapon.GetValue() / 4;
            RemoveWeaponFromInventory(weapon.gameObject);
            UIManager.Instance.DisplayMessage("Sold for " + weapon.GetValue() / 4 + " G!", UIManager.PopUpType.Success);
        }

        public void SellItem(Artifact artifact)
        {
            gold += artifact.GetValue() / 4;
            RemoveArtifactFromInventory(artifact.gameObject);
            UIManager.Instance.DisplayMessage("Sold for " + artifact.GetValue() / 4 + " G!",
                UIManager.PopUpType.Success);
        }

        public void ConsumeItem(GameObject getItem)
        {
            if (getItem.GetComponent<Item>().GetItemType() == Enums.ItemType.Consumable)
            {
                switch (getItem.GetComponent<Item>().GetItemSlot())
                {
                    case Enums.ItemSlot.Consume:
                        RemoveItem(getItem);
                        UIManager.Instance.DisplayMessage("Consumed!", UIManager.PopUpType.Success);
                        break;
                    default:
                        UIManager.Instance.DisplayMessage("Cannot consume this item!", UIManager.PopUpType.Error);
                        break;
                }
            }
            else
            {
                UIManager.Instance.DisplayMessage("Cannot consume this item!", UIManager.PopUpType.Error);
            }
        }

        public bool ContainsItem(GameObject item, int amount)
        {
            switch(item.GetComponent<Item>().GetItemType())
            {
                case Enums.ItemType.Ability:
                    foreach (var var in abilities)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
                case Enums.ItemType.Artifact:
                    foreach (var var in artifacts)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
                case Enums.ItemType.Armor:
                    foreach (var var in armor)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
                case Enums.ItemType.Weapon:
                    foreach (var var in weapons)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
                case Enums.ItemType.Consumable:
                    foreach (var var in consumables)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
                default:
                    foreach (var var in items)
                    {
                        if(var.Value == item && var.Value.GetComponent<Item>().GetAmount() >= amount)
                            return true;
                    }
                    return false;
            }
        }

        public void SetUpgradeItem(GameObject getItem)
        {
            if (!_upgrader.CanUpgrade(getItem.GetComponent<Item>()))
            {
                UIManager.Instance.DisplayMessage("Cannot upgrade this item!", UIManager.PopUpType.Error);
                return;
            }
            upgradeItem = getItem;
        }

        public void UpgradeItem(string type)
        {
            if(upgradeItem == null)
                return;

            Item curItem = upgradeItem.GetComponent<Item>();

            if (curItem.GetSharpenerAmount() >= 5 && type == "sharpener")
            {
                UIManager.Instance.DisplayMessage("Item is already fully sharpened!", UIManager.PopUpType.Error);
                return;
            }
            if (curItem.GetAnvilAmount() >= 5 && type == "anvil")
            {
                UIManager.Instance.DisplayMessage("Item is already fully anviled!", UIManager.PopUpType.Error);
                return;
            }
            
            if (upgradeItem.GetComponent<Item>().GetRecipe(type) == null)
            {
                UIManager.Instance.DisplayMessage("Cannot upgrade this item!", UIManager.PopUpType.Error);
                return;
            }
            if (gold < upgradeItem.GetComponent<Item>().GetRecipe(type).GetPrice())
            {
                UIManager.Instance.DisplayMessage("Not enough gold!", UIManager.PopUpType.Error);
                return;
            }
            
            for (int i = 0; i <upgradeItem.GetComponent<Item>().GetRecipe(type).GetRequiredItems().Count ; i++)
            {
                if (!ContainsItem(upgradeItem.GetComponent<Item>().GetRecipe(type).GetRequiredItems()[i],
                        upgradeItem.GetComponent<Item>().GetRecipe(type).GetRequiredAmounts()[i]))
                {
                    UIManager.Instance.DisplayMessage("Not enough items!", UIManager.PopUpType.Error);
                    return;
                }
            }

            if (type == "anvil")
            {
                curItem.AddAnvilAmount(1);
                UIManager.Instance.DisplayMessage(curItem.GetName() + " has been anviled.", UIManager.PopUpType.Success);
            }
            else if (type == "sharpener")
            {
                curItem.AddSharpenerAmount(1);
                UIManager.Instance.DisplayMessage(curItem.GetName() + " has been sharpened.", UIManager.PopUpType.Success);
            }
        }

        public Item GetUpgradeItem()
        {
            return upgradeItem.GetComponent<Item>();
        }

        // Moves item to a different slot. If the slot is occupied, it swaps the items.
        public void MoveItemToSlot(GameObject getItem, int newSlot)
        {
            int oldSlot = 0;
            switch (getItem.GetComponent<Item>().GetItemType())
            {
                case Enums.ItemType.Ability:
                    SwapItems(newSlot,getItem,abilities);
                    break;
                case Enums.ItemType.Artifact:
                    SwapItems(newSlot,getItem,artifacts);
                    break;
                case Enums.ItemType.Armor:
                    SwapItems(newSlot,getItem,armor);
                    break;
                case Enums.ItemType.Weapon:
                    SwapItems(newSlot,getItem,weapons);
                    break;
                case Enums.ItemType.Consumable:
                    SwapItems(newSlot,getItem,consumables);
                    break;
                default:
                    SwapItems(newSlot,getItem,items);
                    break;
            }
        }
        
        // funny swap function
        public void SwapItems(int newSlot, GameObject oldItem, Dictionary<int,GameObject> items)
        {
            int oldSlot = items.FirstOrDefault(
                 x => x.Value.GetComponent<Item>().GetLocalID()
                      == oldItem.GetComponent<Item>().GetLocalID()).Key;
            
            // If we are trying to swap the item with a stackable item of the same item, merge the stacks to the new slot
            if (items.ContainsKey(newSlot) &&
                items[newSlot].GetComponent<Item>().GetName() == oldItem.GetComponent<Item>().GetName() && oldItem.GetComponent<Item>().GetStackable())
            {
                int amount = items[newSlot].GetComponent<Item>().GetAmount();
                int maxStack = items[newSlot].GetComponent<Item>().GetMaxStack();
                if (amount + oldItem.GetComponent<Item>().GetAmount() <= maxStack)
                {
                    items[newSlot].GetComponent<Item>().AddAmount(oldItem.GetComponent<Item>().GetAmount());
                    items.Remove(oldSlot);
                }
                else
                {
                    items[newSlot].GetComponent<Item>().SetAmount(maxStack);
                    oldItem.GetComponent<Item>()
                        .SetAmount(amount + oldItem.GetComponent<Item>().GetAmount() - maxStack);
                    items.Remove(oldSlot);
                }
            }
            // if the new slot is occupied, swap the items
             else if (items.ContainsKey(newSlot))
             {
                    GameObject newSlotItem = items[newSlot];
                    items.Remove(newSlot);
                    items.Add(newSlot, oldItem);
                    items.Remove(oldSlot);
                    items.Add(oldSlot, newSlotItem);
             }
             // swap the items
             else
             {
                 items.Remove(oldSlot);
                 items.Add(newSlot, oldItem);
             }
        }
        
        public void LoadData(GameData data)
        {
            _player = GetComponent<Player>();
            _player.LoadData(data);
            armor = new Dictionary<int,GameObject>();
            weapons = new Dictionary<int,GameObject>();
            artifacts = new Dictionary<int,GameObject>();
            abilities = new Dictionary<int,GameObject>();
            consumables = new Dictionary<int,GameObject>();
            items = new Dictionary<int,GameObject>();
            equippedAbility = null;
            equippedWeapon = null;
            equippedHelmet = null;
            equippedChestplate = null;
            equippedLeggings = null;
            equippedBoots = null;
            equippedNecklace = null;
            equippedGloves = null;
            
            // Adds items from loaded data through functions VERY IMPORTANT !!
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Armor));
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Weapon));
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Artifact));
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Ability));
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Consumable));
            AddItems(data.playerData.inventory.GetItems(Enums.ItemType.Item));
            // Quests arent very flushed out yet
            quests = data.playerData.inventory.GetQuests();
            gold = data.playerData.inventory.gold;
            stars = data.playerData.inventory.stars;
            EquipWeapon(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Weapon),false);
            EquipAbility(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Ability),false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Helmet),0,false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Chestplate),0,false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Leggings),0,false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Boots),0,false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Necklace),0,false);
            EquipArmor(data.playerData.inventory.GetEquippedItem(Enums.ItemSlot.Gloves),0,false);
            
            _player.UpdatePlayer();
        }

        public void SaveData(GameData data)
        {
            data.playerData.CreateInventory();
            data.playerData.inventory.AddEquippedItem(GetEquippedAbility());
            data.playerData.inventory.AddEquippedItem(GetEquippedWeapon());
            data.playerData.inventory.AddEquippedItem(GetEquippedHelmet());
            data.playerData.inventory.AddEquippedItem(GetEquippedChestplate());
            data.playerData.inventory.AddEquippedItem(GetEquippedLeggings());
            data.playerData.inventory.AddEquippedItem(GetEquippedBoots());
            data.playerData.inventory.AddEquippedItem(GetEquippedNecklace());
            data.playerData.inventory.AddEquippedItem(GetEquippedGloves());

            data.playerData.inventory.AddItems(armor);
            data.playerData.inventory.AddItems(weapons);
            data.playerData.inventory.AddItems(artifacts);
            data.playerData.inventory.AddItems(abilities);
            data.playerData.inventory.AddItems(consumables);
            data.playerData.inventory.AddQuests(quests);
            data.playerData.inventory.AddItems(items);
            data.playerData.inventory.gold = gold;
            data.playerData.inventory.stars = stars;
        }

        public void SetUpgrader(NPCUpgrader upgrader)
        {
            _upgrader = upgrader;
        }

        public void SetItemToPickUp(GameObject o)
        {
            if(pickupItem == null || o == null)
                pickupItem = o;
        }
        
        public void PickUpItem()
        {
            if (pickupItem != null)
            {
                AddItem(pickupItem);
                pickupItem = null;
            }
        }
        
        public GameObject GetItemToPickUp()
        {
            return pickupItem;
        }
    }
}