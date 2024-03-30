using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Recipe : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> _items = new List<GameObject>();
        [SerializeField]
        List<int> _amounts = new List<int>();
        [SerializeField]
        private int _price;
        
        public List<GameObject> GetRequiredItems()
        {
            return _items;
        }
        public List<int> GetRequiredAmounts()
        {
            return _amounts;
        }
        public int GetPrice()
        {
            return _price;
        }
        
        public bool CanCraft(Inventory inventory)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (!inventory.ContainsItem(_items[i], _amounts[i]))
                    return false;
            }
            return true;
        }
        
        public void Craft(Inventory inventory)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                inventory.RemoveItem(_items[i]);
            }
        }

        public void Increase(int i)
        {
            for (int j = 0; j < _items.Count; j++)
            {
                _amounts[j] *= i;
            }
            
            _price *= i;
        }
    }
}