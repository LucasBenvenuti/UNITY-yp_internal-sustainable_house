using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : MonoBehaviour
{
    public bool isSelectable;
    public int itemOption;
    public string itemName;
    public int itemType;
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
        if (itemType < 0)
        {
            itemType = 0;
        }
        if (itemSustainability < -5.1)
        {
            itemSustainability = -5;
        }
        if (itemOption < 0)
        {
            itemOption = 0;
        }
    }
}

