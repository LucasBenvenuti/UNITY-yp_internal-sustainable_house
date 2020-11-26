using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;

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
        GameController.instance.CheckAndDestroyItem(itemPrefab, itemName);
        if (GameController.instance.destroyOriginalItem)
        {
            if (TimerController.instance.tutorialMode)
            {
                if (Tutorial.instance.currentTutorial == "choose")
                {
                    Tutorial.instance.canContinue = false;
                }
            }

            if (!itemSelectedTemplate.alreadyChanged)
            {
                itemSelectedTemplate.outlineOrange.enabled = false;
                itemSelectedTemplate.outlineBlue.enabled = true;

                itemSelectedTemplate.alreadyChanged = true;
            }

            GameObject prefab = Instantiate(itemPrefab);
            prefab.transform.parent = GameController.instance.itemHolder.transform;
            prefab.transform.SetAsFirstSibling();
            prefab.transform.position = itemPosition.position;
            GameController.instance.destroyOriginalItem = false;

            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = true;
            GameController.instance.currentOption = itemSelectedTemplate.itemOption;
            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = false;

            itemSelectedTemplate.particleSystem.Play();

            GameController.instance.goToObjectShop = true;
            CameraController.instance.LerpToZoomPosition(prefab, itemSelectedTemplate.zoomSize);
            // UIController.instance.updateValues = true;
            // UIController.instance.NewUpdateValues(itemPrice, itemSustainability);
        }
    }
    public void ClickOnItem()
    {
        GameController.instance.ChangeRequest(this);
    }
}
