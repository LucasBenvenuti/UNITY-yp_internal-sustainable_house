using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{

    public GameObject itemPrefab;
    public Transform itemPosition;
    public ItemTemplate itemSelectedTemplate;
    //public float itemPrice;
    //public float itemSustainability;
    //public float itemCostPerMonth;

    void Awake()
    {
        itemSelectedTemplate = itemPrefab.GetComponent<ItemTemplate>();
        //itemPrice = itemSelectedTemplate.itemPrice;
        //itemSustainability = itemSelectedTemplate.itemSustainability;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NewItemInstance()
    {
        GameController.instance.CheckAndDestroyItem(itemPrefab);
        if (GameController.instance.destroyOriginalItem)
        {
            GameObject prefab = Instantiate(itemPrefab);
            prefab.transform.parent = GameController.instance.itemHolder.transform;
            prefab.transform.SetAsFirstSibling();
            prefab.transform.position = itemPosition.position;
            GameController.instance.destroyOriginalItem = false;
        }
    }
    public void ClickOnItem()
    {
        GameController.instance.ChangeRequest(this);
    }
}
