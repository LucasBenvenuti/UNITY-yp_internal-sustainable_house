using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{

    public GameObject itemPrefab;
    public Transform itemPosition;
    public ItemTemplate itemSelectedTemplate;
    public float itemPrice;
    public float itemSustainability;
    public float itemCostPerMonth;
    public GameObject confirmBtn;

    void Awake()
    {
        itemSelectedTemplate = itemPrefab.GetComponent<ItemTemplate>();
        itemPrice = itemSelectedTemplate.itemPrice;
        itemSustainability = itemSelectedTemplate.itemSustainability;
    }

    public void NewItemInstance()
    {
        // UIController.instance.CheckBaseValues();
        GameController.instance.CheckAndDestroyItem(itemPrefab);
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
        if (confirmBtn != null)
        {
            confirmBtn.SetActive(true);
        }
        else
        {
            Debug.Log("Need to set Confirm Button on Select Item");
        }
    }
}
