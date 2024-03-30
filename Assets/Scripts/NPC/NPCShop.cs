using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class NPCShop : NPC
    {
        [SerializeField] private GameObject[] itemsForSale;

        public GameObject[] GetItemsForSale()
        {
            return itemsForSale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(true, gameObject, npcType);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(false, null, npcType);
            }
        }
    }
}