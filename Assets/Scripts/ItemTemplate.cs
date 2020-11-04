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
    public float itemCostPerMonth;
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
        if (itemSustainability < -5.1)
        {
            itemSustainability = -5;
        }
    }
}

