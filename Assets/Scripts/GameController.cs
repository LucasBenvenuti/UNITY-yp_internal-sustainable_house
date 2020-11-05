﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ItemTemplate itemSelected;
    public ActionTemplate actionSelected;
    /*
      public GameObject[] dryCloths;
      public GameObject[] lamps;
      public GameObject[] windows;
      public GameObject[] energy;
      public GameObject[] water;
      public GameObject[] sanatary;
      public GameObject[] shower;
      public GameObject[] flush;
      public GameObject[] sink;
      public GameObject[] garbage;
      public GameObject[] reuseWater;
      public GameObject[] laundry;
     
     */
    //public GameObject[] acs;
    //public GameObject[] refrigerator;
    //public GameObject[] tvs;
    public GameObject[] itemType;
    public GameObject[] actionType;
    public GameObject panelItem;
    public GameObject panelAction;
    public GameObject itemHolder;
    public GameObject backBtn;
    public GameObject confirmBox;
    public Button confirmBtn;
    public int indexItemType;
    public bool destroyOriginalItem;
    public bool itemPanelActive;
    public bool canGoToObject = true;
    public float itemSelectedPrice;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(this);
        }
    }

    public void onTap()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.gameObject.tag == "Item")
            {
                selectItem(hit.transform.gameObject);
            }
            if (hit.transform.gameObject.tag == "Action")
            {
                SelectAction(hit.transform.gameObject);
            }

        }
    }

    public void selectItem(GameObject hitObject)
    {
        itemHolder = hitObject.transform.parent.gameObject;
        CheckItemValues(hitObject);

        CameraController.instance.LerpToZoomPosition(itemHolder);
        panelItem.SetActive(true);

        if (backBtn != null)
        {
            backBtn.SetActive(true);
        }
        else
        {
            Debug.Log("Need to set Back Button on Game Controller");
        }
    }

    void PrintName(GameObject go)
    {
        print(go.GetComponent<ItemTemplate>().itemName);
    }

    void CheckItemValues(GameObject go)
    {
        for (int i = 0; i < itemType.Length; i++)
        {
            itemType[i].SetActive(false);
        }
        if (itemSelected != go.GetComponent<ItemTemplate>())
        {
            itemSelected = go.GetComponent<ItemTemplate>();
        }
        indexItemType = itemSelected.itemType;
        itemType[indexItemType].SetActive(true);
    }
    public void CheckAndDestroyItem(GameObject newPrefab)
    {
        int newOption = newPrefab.GetComponent<ItemTemplate>().itemOption;
        float newItemPrice = newPrefab.GetComponent<ItemTemplate>().itemPrice;
        float newItemSus = newPrefab.GetComponent<ItemTemplate>().itemSustainability;
        if (itemHolder != null)
        {
            GameObject prefab = itemHolder.transform.GetChild(0).gameObject;
            int optionInScene = prefab.GetComponent<ItemTemplate>().itemOption;
            if (newOption != optionInScene)
            {
                bool priceControl = UIController.instance.NewUpdateValues(newItemPrice, newItemSus);
                if (priceControl)
                {
                    Destroy(prefab);
                    destroyOriginalItem = true;
                }
                else
                {
                    destroyOriginalItem = false;
                }
            }
            else
            {
                Debug.Log("item igual nao foi substituido");
                destroyOriginalItem = false;
            }
        }
    }

    public void SelectAction(GameObject hitAction)
    {
        DisplayActionTypeUI(hitAction);
        panelAction.SetActive(true);
        StopCoroutine(PanelOff());
        StartCoroutine(PanelOff());
    }

    IEnumerator PanelOff()
    {
        yield return new WaitForSeconds(3f);
        panelAction.SetActive(false);
        StopCoroutine(PanelOff()); 
    }

    void DisplayActionTypeUI(GameObject go)
    {
        for (int i = 0; i < actionType.Length; i++)
        {
            actionType[i].SetActive(false);
        }
        if (actionSelected != go.GetComponent<ActionTemplate>())
        {
            actionSelected = go.GetComponent<ActionTemplate>();
        }
        actionSelected.DoneAction();
        int indexSelected = actionSelected.actionIndex;
        actionType[indexSelected].SetActive(true);
    }

    public void ChangeRequest(SelectItem item)
    {
        if (confirmBtn != null)
        {
            //panelItem.SetActive(false);
            confirmBtn.onClick.AddListener(() => { Teste(item); });
            confirmBox.SetActive(true);
        }
        else
        {
            Debug.Log("Need to set Confirm Button on Select Item");
        }
    }
    public void Teste(SelectItem item)
    {
        item.NewItemInstance();
        confirmBtn.onClick.RemoveListener(() => { Teste(item); });
    }
}
