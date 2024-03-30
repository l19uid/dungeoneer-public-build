using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Dungeoneer
{
    public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        private GameObject _item;
        private int _id;

        public Transform parentAfterDrag;
        public GameObject stats;
        public bool isEquipped;
        private Enums.ItemSlot _itemSlot;
        private UIManager _manager;
        public GameObject itemHoverPanel;
        private GameObject itemHoverPanelGO;
        public GameObject amount;
        private bool isBeingSold;
        Image image;
        private bool isHovered;

        // Start is called before the first frame update
        public void Init(GameObject item, int id, bool isEquipped = false, bool isBeingSold = false)
        {
            _item = item;
            _id = id;
            image = GetComponentInChildren<Image>();
            image.sprite = item.GetComponent<Item>().GetItemIcon();
            this.isEquipped = isEquipped;
            this.isBeingSold = isBeingSold;

            if (gameObject.GetComponentInParent<UIInventorySlot>() != null)
                _itemSlot = gameObject.GetComponentInParent<UIInventorySlot>().GetItemSlot();

            _manager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
            amount.GetComponent<TextMeshProUGUI>().text = _item.GetComponent<Item>().GetAmount().ToString();
            amount.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Destroy(itemHoverPanelGO);
            if (!isBeingSold)
                _manager.ShowSellOverlay(true, (_item.GetComponent<Item>().GetStackValue() / 4).ToString());
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
            if(isEquipped)
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>()
                    .RenderItems(_item.GetComponent<Item>().GetItemType());
            Debug.Log("Begin drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1))
                return;
            Debug.Log("Dragging");
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _manager.ShowSellOverlay(false, "");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Destroy(itemHoverPanelGO);
            itemHoverPanelGO = Instantiate(itemHoverPanel);
            itemHoverPanelGO.GetComponent<TextBoxHover>().InitHoverPanel(_item);
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(itemHoverPanelGO);
            isHovered = false;
        }
        
        public Enums.ItemSlot GetItemSlot()
        {
            return _item.GetComponent<Item>().GetItemSlot();
        }

        public Enums.ItemType GetItemType()
        {
            return _item.GetComponent<Item>().GetItemType();
        }

        public GameObject GetItem()
        {
            return _item;
        }

        public Enums.ItemSlot GetSlotSlot()
        {
            return _itemSlot;
        }

        public bool GetEquipped()
        {
            return isEquipped;
        }

        public void DestroyItem()
        {
            Destroy(itemHoverPanelGO);
            Destroy(this);
        }

        private void Update()
        {
            if (_item != null && Input.GetKeyDown(KeyCode.LeftShift) && _item.GetComponent<Item>().GetStackable())
                amount.SetActive(true);
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                amount.SetActive(false);
            if (isHovered && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(isEquipped)
                    GameObject.FindWithTag("Player").GetComponent<Inventory>().UnequipItem(_item);
                else
                    GameObject.FindWithTag("Player").GetComponent<Inventory>().EquipItem(_item);
                
                _manager.UpdateUI();
            }
        }

        public int GetSlotNumber()
        {
            Debug.Log("Slot : " + _id);
            return _id;
        }
    }
}
