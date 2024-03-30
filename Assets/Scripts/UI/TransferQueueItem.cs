using TMPro;
using UnityEngine;

namespace Dungeoneer
{
    public class TransferQueueItem : MonoBehaviour
    {
        private GameObject _item;
        private int _amount;
        private float _timer = 0f;
        
        public void SetUp(GameObject item, int amount)
        {
            _item = item;
            _amount = amount;
            _timer = 4f;
            
            GetComponent<TextMeshProUGUI>().text =
                "<gradient=\"ColorGreenGradient\">+ " + _item.GetComponent<Item>().GetAmount() + "x</gradient> "
                + _item.GetComponent<Item>().GetRarityGradient() + _item.GetComponent<Item>().GetName() +
                "</gradient>";
            gameObject.name = _item.GetComponent<Item>().GetItemId();
        }
        
        public GameObject GetItem()
        {
            return _item;
        }
        
        public int GetAmount()
        {
            return _amount;
        }
        
        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Destroy(gameObject);
                UIManager.Instance.RemoveTransferQueueItem(gameObject);
            }
        }
    }
}