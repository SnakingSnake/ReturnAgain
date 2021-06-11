using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerUpHandler
{
    public int slotnum;
    public InvenItem invenItem;
    public Image itemIcon;
    
    public void UpdateSlotUI()
    {

        itemIcon.sprite = invenItem.itemImage;
        itemIcon.gameObject.SetActive(true);

    }

    public void RemoveSlot()
    {
        invenItem = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        bool isUse = invenItem.Use();
        if(isUse)
        {
            Inventory.instance.RemoveItem(slotnum);
        }
    }

}
