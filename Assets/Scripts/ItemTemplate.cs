using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : MonoBehaviour
{
    public bool isSelectable;
    public string itemName;
    public string itemType;
    public float itemPrice;
    public float itemSustainability;
    public Sprite itemSprite;

    void Start()
    {
        if (!isSelectable)
        {
            isSelectable = true;
        }
        if (itemName == null)
        {
            itemName = "Default";
        }
        if (itemType == null)
        {
            itemType = "Default";
        }
        if (itemPrice < 0.1)
        {
            itemPrice = 0;
        }
        if (itemSustainability < 0.1)
        {
            itemSustainability = 0;
        }
    }


    public void onTapObject()
    {
        GameController.instance.selectItem(this.gameObject);

        Debug.Log("Object Tapped - " + this.gameObject);
    }
}
