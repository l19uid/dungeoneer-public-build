using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class NPCUpgrader : NPC
    {
        private Item selectedItem;
        private string upgradeType;
        public GameObject[] upgradeableItems;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(true, gameObject, npcType);
                other.GetComponent<Inventory>().SetUpgrader(this);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(false, null, npcType);
                other.GetComponent<Inventory>().SetItemToPickUp(null);
            }
        }

        public void UpgradeItem(Item item)
        {
            Debug.Log("Upgrading item");
            switch(upgradeType)
            {
                case "anvil":
                    item.UpgradeItem("anvil");
                    break;
                case "forge":
                    item.UpgradeItem("forge");
                    break;
            }
        }
        
        public void SetSelectedItem(Item item)
        {
            selectedItem = item;
        }
        
        public void SetUpgradeType(string type)
        {
            upgradeType = type;
        }
        
        public bool CanUpgrade(Item item)
        {
            foreach (GameObject upgradeableItem in upgradeableItems)
            {
                if (item.GetName() == upgradeableItem.GetComponent<Item>().GetName())
                    return true;
            }

            return false;
        }
    }
}
