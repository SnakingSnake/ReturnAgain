using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Money, Heart, Weapon, Shild, PlusHeart
}

[System.Serializable]
public class InvenItem
{
    public ItemType itemType;
    public string itemName;
    public int itemValue;
    public Sprite itemImage;
    public Player_knights player;
    

    public bool Use()
    {
        bool isUsed = false;
        if(player.hasWeaponValue[itemValue%2] == -1)
        {
            if(itemValue%2==0)
            {
                player.hasWeapons[0] = true;
                player.hasWeaponValue[0] = itemValue;
                
            }
            else
            {
                player.hasWeapons[1] = true;
                player.hasWeaponValue[1] = itemValue;
            }
            isUsed = true;
        }
        else
        {            
            if(itemValue%2==0)
            {
                Inventory.instance.AddItem(player.hasWeaponValue[0]);
                player.hasWeapons[0] = true;
                player.hasWeaponValue[0] = itemValue;
            }
            else
            {
                Inventory.instance.AddItem(player.hasWeaponValue[1]);
                player.hasWeapons[1] = true;
                player.hasWeaponValue[1] = itemValue;
            }
            isUsed = true;
        }
        return isUsed;
    }
}
