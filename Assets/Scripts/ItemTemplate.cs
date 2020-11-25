using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

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
                   15: telhado               
     */
    public bool isSelectable;
    public int itemOption;

    //USED ON REPORT LINE
    public string itemName;
    public int itemType;
    public float itemPrice;
    public float itemSustainability;
    public float basePrice;
    public float baseSustainability;
    public Sprite itemSprite;
    public GameObject itemPrefab;

    //USE TO GET A STRING TO PUT AS TITLE OF UI
    public string categoryName;

    public float zoomSize = 5f;

    public bool alreadyChanged = false;

    public Outlinable outlineOrange;
    public Outlinable outlineBlue;

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
        // if (itemSustainability < -5.1)
        // {
        //     itemSustainability = -5;
        // }
        if (itemOption < 0)
        {
            itemOption = 0;
        }
    }
}

