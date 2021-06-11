using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Money, Heart, Weapon, Shild, PlusHeart };
    public Type type;
    public int value;
    public InvenItem invenItem;
    public SpriteRenderer image;

    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }

    public void SetItem(InvenItem _invenItem)
    {
        invenItem.itemName = _invenItem.itemName;
        invenItem.itemImage = _invenItem.itemImage;
        invenItem.itemType = _invenItem.itemType;
        invenItem.itemValue = _invenItem.itemValue;

        image.sprite = _invenItem.itemImage;
    }

    public InvenItem GetItem()
    {
        return invenItem;
    }
}
