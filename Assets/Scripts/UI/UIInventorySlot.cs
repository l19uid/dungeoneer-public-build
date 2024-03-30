using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeoneer
{
    public class UIInventorySlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private UIManager _manager;
        [SerializeField] private Enums.ItemSlot slot;
        [SerializeField] private int slotNum;
        [SerializeField] private Enums.ItemType type;

        private void Awake()
        {
            _inventory = GameObject.Find("Player").GetComponent<Inventory>();
            _manager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
        }

        /// WTF IS THIS FUNCTION I DONT UNDERSTAND ITS SO LONG I CANT REMEMBER
        public void OnDrop(PointerEventData eventData)
        {
            _manager.ShowSellOverlay(false, "");
            GameObject droppedItem = eventData.pointerDrag;
            UIItem droppedItemUI = droppedItem.GetComponent<UIItem>();
            
            if(droppedItemUI.GetEquipped() && slot != Enums.ItemSlot.None)
                return;

            if (slot == Enums.ItemSlot.Weapon && droppedItemUI.GetItemSlot() == Enums.ItemSlot.Weapon)
            {
                SetEquippedWeapon(droppedItem.GetComponent<UIItem>().GetItem());
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }
            else if (slot == Enums.ItemSlot.None && droppedItemUI.GetItemSlot() == Enums.ItemSlot.Weapon &&
                     droppedItemUI.GetEquipped())
            {
                UnequipWeapon();
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }

            else if (slot == Enums.ItemSlot.Ability && droppedItemUI.GetItemSlot() == Enums.ItemSlot.Ability)
            {
                SetEquippedAbility(droppedItem.GetComponent<UIItem>().GetItem());
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }

            else if (type == Enums.ItemType.Armor && droppedItemUI.GetItemType() == Enums.ItemType.Armor)
            {
                SetEquippedArmor(droppedItem.GetComponent<UIItem>().GetItem(),droppedItem.GetComponent<UIItem>().GetSlotNumber());
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }

            else if (droppedItemUI.GetItemType() == Enums.ItemType.Armor && slot == Enums.ItemSlot.None && droppedItemUI.GetEquipped())
            {
                UnequipArmor(droppedItemUI.GetSlotSlot(),slotNum);
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }

            else if (slot == Enums.ItemSlot.Sell)
            {
                _inventory.SellItem(droppedItem.GetComponent<UIItem>().GetItem());
            }

            else if (slot != Enums.ItemSlot.Store && droppedItemUI.GetSlotSlot() == Enums.ItemSlot.Store)
            {
                Debug.Log("Buy item");
                _inventory.BuyItem(droppedItem.GetComponent<UIItem>().GetItem(), slotNum);
                string itemType = droppedItem.GetComponent<UIItem>().GetItemType().ToString();
                _manager.SetCurrentInventoryType(itemType);
            }

            else if (slot == Enums.ItemSlot.Consume && droppedItemUI.GetItemType() != Enums.ItemType.Consumable)
                _inventory.ConsumeItem(droppedItem.GetComponent<UIItem>().GetItem());

            // TODO : ARTIFACT
            //if (slot == Enum.ItemSlot.ArtifactPouch && droppedItemUI.GetItemType() == Enum.ItemType.Artifact)
            //{
            //    _inventory.EquipArtifact(droppedItem.GetComponent<UIItem>().GetItem());
            //}
            //else if (slot == Enum.ItemSlot.ArtifactPouch && droppedItemUI.GetItemType() == Enum.ItemType.None)
            //{
            //    _inventory.UnequipArtifact(droppedItem.GetComponent<UIItem>().GetItem());
            //}

            //upgrade item
            else if (slot == Enums.ItemSlot.Upgrade && droppedItemUI.GetItemSlot() != Enums.ItemSlot.Upgrade)
            {
                _inventory.SetUpgradeItem(droppedItemUI.GetItem());
                _manager.SetUpgradeInfo();
            }
            
            //move item (change slots of the item)
            else if (droppedItemUI.GetSlotSlot() == Enums.ItemSlot.None &&
                     droppedItemUI.GetEquipped() == false && slotNum < 30 && slotNum >= 0)
            {
                _inventory.MoveItemToSlot(droppedItemUI.GetItem(),slotNum);
            }
            
            _manager.UpdateUI();
        }

        private void SetEquippedWeapon(GameObject weapon)
        {
            _inventory.EquipWeapon(weapon);
        }

        private void UnequipWeapon()
        {
            _inventory.UnequipWeapon();
        }

        private void SetEquippedArmor(GameObject armor, int slot)
        {
            if (armor.GetComponent<Item>().GetItemSlot() == this.slot)
            {
                Debug.Log(armor.GetComponent<Item>().GetItemSlot() + " " + this.slot);
                _inventory.EquipArmor(armor, slot);
            }
        }

        private void UnequipArmor(Enums.ItemSlot slotType, int slot)
        {
            Debug.Log("UNEQUIPPING ARMOR " + this.slot);
            _inventory.UnequipArmor(slotType, slot);
        }

        private void SetEquippedAbility(GameObject ability)
        {
            _inventory.EquipAbility(ability);
        }

        private void UnequipAbility()
        {
            _inventory.UnequipAbility();
        }

        public void UpdateSlot(Enums.ItemType itemType)
        {
            type = itemType;
        }

        public Enums.ItemSlot GetItemSlot()
        {
            return slot;
        }

        public Enums.ItemType GetItemType()
        {
            return type;
        }
    }
}
