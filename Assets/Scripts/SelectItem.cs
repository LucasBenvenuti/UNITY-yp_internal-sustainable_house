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

    public string itemName;

    void Awake()
    {
        // itemSelectedTemplate = itemPrefab.GetComponent<ItemTemplate>();
        //itemPrice = itemSelectedTemplate.itemPrice;
        //itemSustainability = itemSelectedTemplate.itemSustainability;
    }

    public void NewItemInstance()
    {
        Debug.Log("STARTED NEW ITEM INSTANCE");

        // UIController.instance.CheckBaseValues();
        GameController.instance.CheckAndDestroyItem(itemPrefab, this);
        if (GameController.instance.destroyOriginalItem)
        {
            GameObject prefab = Instantiate(itemPrefab);
            prefab.transform.parent = GameController.instance.itemHolder.transform;
            prefab.transform.SetAsFirstSibling();
            prefab.transform.position = itemPosition.position;
            GameController.instance.destroyOriginalItem = false;
            // UIController.instance.updateValues = true;
            // UIController.instance.NewUpdateValues(itemPrice, itemSustainability);
        }
    }
    public void ClickOnItem()
    {
        GameController.instance.ChangeRequest(this);
    }
}
