using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class DroppedItem : MonoBehaviour
    {
        private GameObject item;
        public SpriteRenderer spriteRenderer;
        public ParticleSystem rarityParticle;
        public ParticleSystem rarityParticleTwo;
        private UIManager _manager;
        public Material outlineMaterial;
        public Material defaultMaterial;
        private Material _material;
        private string _id;
        private bool _isSelected;
        private GameObject player;
        private UIManager _uiManager;
        public void InitItem(GameObject item)
        {
            _manager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
            this.item = item;
            spriteRenderer.sprite = item.GetComponent<Item>().GetItemIcon();
            spriteRenderer.material = defaultMaterial;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-250, 250), Random.Range(100, 250)));
            ChangeRarityColor(item.GetComponent<Item>());
            _id = Guid.NewGuid().ToString();
            _uiManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
        }
        
        public void ChangeRarityColor(Item item)
        {
            var colorOverLifetimeModule = rarityParticle.colorOverLifetime;
            colorOverLifetimeModule.color = 
                new ParticleSystem.MinMaxGradient(GradientUtility.GetGradient(item.GetItemRarity()), GradientUtility.GetGradient(item.GetItemRarity()));  
            var colorOverLifetimeModuleTwo = rarityParticleTwo.colorOverLifetime;
            colorOverLifetimeModuleTwo.color = 
                new ParticleSystem.MinMaxGradient(GradientUtility.GetGradient(item.GetItemRarity()), GradientUtility.GetGradient(item.GetItemRarity()));  
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.gameObject;
                other.GetComponent<Inventory>().SetItemToPickUp(item);
                if (other.GetComponent<Inventory>().GetItemToPickUp() == item)
                {
                    spriteRenderer.material = outlineMaterial;
                    _isSelected = true;
                    _uiManager.SetPickUpClose(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.SetPickUpClose(false);
                spriteRenderer.material = defaultMaterial;
                other.GetComponent<Inventory>().SetItemToPickUp(null);
                _isSelected = false;
                _uiManager.SetPickUpClose(false);
            }
        }

        private void Update()
        {
            if (_isSelected && Input.GetKeyDown(KeyCode.F))
            {
                player.GetComponent<Inventory>().PickUpItem();
                Destroy(gameObject);
                _manager.SetPickUpClose(false);
            }
        }

        public string GetId()
        {
            return _id;
        }
    }
}