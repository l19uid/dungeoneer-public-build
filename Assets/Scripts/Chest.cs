using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Dungeoneer
{
    public class Chest : MonoBehaviour
    {
        [Header("Chest Properties")]
        public Enums.ItemRarity rarity;
        public Sprite unopenedChest;
        public Sprite openedChest;
        [Header("Fancy Effects")]
        public Material topChest;
        public Material lockChest;
        public Gradient gradientOne;
        public Gradient gradientTwo;
        public GameObject openEffect;
        
        
        SpriteRenderer spriteRenderer;
        private bool _opened;
        public GameObject droppedItemPrefab;
        public bool keyChest = false;
        public GameObject key;
        DropPool _dropPool;
        UIManager _manager;
        private bool isClose;

        private void Start()
        {
            _opened = false;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = unopenedChest;
            if(gameObject.GetComponent<DropPool>() != null)
                _dropPool = gameObject.GetComponent<DropPool>();
            else
                _dropPool = GameObject.Find("DropPool").GetComponent<DropPool>();
            
            _manager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
        }

        public void OpenChest()
        {
            if (_opened)
                return;
            spriteRenderer.sprite = openedChest;
            
            _opened = true;
            _manager.SetChestClose(false);
            DropItem();

            //if (keyChest)
            //{
            //    GameObject droppedKey = Instantiate(key, transform.position, Quaternion.identity);
            //}
            //else
        }

        private void DropItem()
        {
            // FANCY EFFECTS
            GameObject effect = Instantiate(openEffect, transform.position, Quaternion.identity);
            effect.transform.GetChild(0).GetComponent<ParticleSystem>().
                GetComponent<Renderer>().material = topChest;
            effect.transform.GetChild(1).GetComponent<ParticleSystem>().
                            GetComponent<Renderer>().material = lockChest;
            var colorOverLifetimeModule = effect.transform.GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime;
            colorOverLifetimeModule.color = 
                new ParticleSystem.MinMaxGradient(gradientOne, gradientTwo);
            Destroy(effect, 2f);
            // ---------
            
            // DROP ITEM
            GameObject item = _dropPool.GetRandomItem(rarity);
            GameObject droppeditem = Instantiate(droppedItemPrefab, transform.position+Vector3.up, Quaternion.identity);
            droppeditem.GetComponent<DroppedItem>().InitItem(item);
            UIManager.Instance.DisplayMessage(item.GetComponent<Item>());
            // ---------
        }

        public bool IsOpened()
        {
            return _opened;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_opened && other.TryGetComponent(out Player player))
            {
                isClose = true;
                _manager.SetChestClose(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                isClose = false;
                _manager.SetChestClose(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && isClose && !_opened)
            {
                OpenChest();
                _manager.SetChestClose(false);
            }
        }
    }
}