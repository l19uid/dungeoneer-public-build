using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private List<GameObject> _storedItems = new List<GameObject>();
    private int _maxItems = 10;
    
    public void StoreItem(GameObject item)
    {
        _storedItems.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        _storedItems.Remove(item);
    }
    
    public List<GameObject> GetStoredItems()
    {
        return _storedItems;
    }
    
    public int GetMaxItems()
    {
        return _maxItems;
    }
}
