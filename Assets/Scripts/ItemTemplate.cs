using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : MonoBehaviour
{
    /*item type -> 0: climatização
                   1: secagem de roupa
                   2: lampada
                   3: janelas 
                   4: abastecimento de energia eletrica
                   5: abastecimento de agua
                   6: esgotamento sanitario
                   7: chuveiro
                   8: vaso sanitario    
                   9: torneira de pia  
                   10: geladeira 
                   11: lixeiras 
                   12: maquina de lavar roupa 
                   13: agua de reuso 
                   14: televisão                    
     */
    public bool isSelectable;
    public string itemName;
    public int itemType;
    public float itemPrice;
    public float itemSustainability;
    public float itemCostPerMonth;
    public int itemOption;
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
        if (itemOption < 0)
        {
            itemType = 0;
        }
        if (itemPrice < 0.1)
        {
            itemPrice = 0;
        }
        if (itemSustainability < -5.1)
        {
            itemSustainability = -5;
        }
    }


    public void onTapObject()
    {
        GameController.instance.selectItem(this.gameObject);

        Debug.Log("Object Tapped - " + this.gameObject);
    }
}
