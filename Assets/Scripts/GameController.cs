using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ItemTemplate itemSelected;
    public GameObject[] refrigerator;
    public GameObject[] shower;
    public GameObject[] itemType;
    public GameObject panelItem;
    public GameObject itemHolder;
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
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.tag == "Item")
                {
                    itemHolder = hit.transform.parent.gameObject;
                    CheckItemValues(hit.transform.gameObject);
                       
                    CameraController.instance.ChangeZoomControl();
                    panelItem.SetActive(true);
                }
            }
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
        if (itemSelected.itemType == "Geladeira")
        {
            //for (int i = 0; i < refrigerator.Length; i++)
            //{
            //    print("nome da geladeira" + i + ":" + refrigerator[i].GetComponent<ItemTemplate>().itemName);
            //}
            itemSelectedPrice = itemSelected.itemPrice;
            itemType[0].SetActive(true);
        }
        if (itemSelected.itemType == "Chuveiro")
        {
            //for (int i = 0; i < shower.Length; i++)
            //{
            //    print("nome da chuveiro" + i + ":" + shower[i].GetComponent<ItemTemplate>().itemName);
            //}

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

    //public IEnumerator ZoomItemAnimation()
    //{
    //    if (!CameraController.instance.isOnZoomPosition)
    //    {
    //        CameraController.instance.LerpToZoomPosition(itemHolder);
    //        yield return null;
    //    }
    //}
}
