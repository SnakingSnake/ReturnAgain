using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<InvenItem> items = new List<InvenItem>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public bool AddItem(int itemValue)
    {
        items.Add(ItemDatabase.instance.itemDB[itemValue]);
        if (onChangeItem != null)
        {
            onChangeItem.Invoke();
            return true;
        }
        return false;

    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
    }



}