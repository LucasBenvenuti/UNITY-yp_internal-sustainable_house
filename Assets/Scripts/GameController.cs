using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ItemTemplate itemSelected;
    public ActionTemplate actionSelected;
    public GameObject[] refrigerator;
    public GameObject[] shower;
    public GameObject[] itemType;
    public GameObject[] actionType;
    public GameObject panelItem;
    public GameObject panelAction;
    public GameObject itemHolder;
    public GameObject backBtn;
    public int indexItemType;
    public bool destroyOriginalItem;
    public bool itemPanelActive;
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
<<<<<<< Updated upstream
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.tag == "Item")
                {
                    itemHolder = hit.transform.parent.gameObject;
                    CheckItemValues(hit.transform.gameObject);
=======
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
        DisplayItemTypeUI(hitObject);
>>>>>>> Stashed changes

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
            }
        }
    }

    void DisplayItemTypeUI(GameObject go)
    {
        for (int i = 0; i < itemType.Length; i++)
        {
            itemType[i].SetActive(false);
        }
        if (itemSelected != go.GetComponent<ItemTemplate>())
        {
            itemSelected = go.GetComponent<ItemTemplate>();
        }
        if (itemSelected.itemType == "Geladeira")
        {
            itemSelectedPrice = itemSelected.itemPrice;
            itemType[0].SetActive(true);
        }
        if (itemSelected.itemType == "Chuveiro")
        {
            itemType[1].SetActive(true);
        }
    }

    public void DestroyInGameItem()
    {
        if (itemHolder != null)
        {
            GameObject prefab = itemHolder.transform.GetChild(0).gameObject;
            Destroy(prefab);
            destroyOriginalItem = true;
        }
    }

    public void SelectAction(GameObject hitAction)
    {
        DisplayActionTypeUI(hitAction);
        panelAction.SetActive(true);
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
        int indexSelected = actionSelected.actionIndex;
        actionType[indexSelected].SetActive(true);
        //if(actionSelected.actionIndex == 0)
        //{
        //    actionType[0].SetActive(true);
        //}
        //if(actionSelected.actionIndex == 1)
        //{
        //    actionType[1].SetActive(true);
        //}
    }


    void PrintName(GameObject go)
    {
        print(go.GetComponent<ItemTemplate>().itemName);
    }


}
