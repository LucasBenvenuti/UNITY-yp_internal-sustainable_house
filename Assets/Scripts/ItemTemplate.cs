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
        if(itemName == null)
        {
            itemName = "Default";
        } 
        if(itemType == null)
        {
            itemType = "Default";
        } 
        if(itemPrice < 0.1)
        {
            itemPrice = 0;
<<<<<<< Updated upstream
        } 
        if(itemSustainability < 0.1)
=======
        }
        if (itemSustainability < -5.1)
>>>>>>> Stashed changes
        {
            itemSustainability = -5;
        }
<<<<<<< Updated upstream
    }


    void Update()
    {
        
=======
>>>>>>> Stashed changes
    }
  
}
