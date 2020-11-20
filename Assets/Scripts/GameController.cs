using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ListItems
{
    public List<ItemTemplate> prefabsList;
}

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

    public CanvasGroup panelItem;
    public TMP_Text titlePanel;
    public List<ItemOption> uiItemList;

    public GameObject panelAction;
    public GameObject itemHolder;
    public GameObject confirmBox;
    public Button confirmBtn;
    public int indexItemType;
    public bool destroyOriginalItem;
    public bool itemPanelActive;
    public bool canGoToObject = true;
    public float itemSelectedPrice;

    public List<ListItems> prefabsList = new List<ListItems>();

    public ActionsAnimations[] actionsAnimations;

    public List<string> reportList = new List<string>();

    public float tweenDuration = 0.3f;
    public LeanTweenType easeInOut;

    [HideInInspector]
    public bool tvOn = true;
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

        panelItem.alpha = 0;
        panelItem.interactable = false;
        panelItem.blocksRaycasts = false;

        for (int i = 0; i < uiItemList.Count; i++)
        {
            uiItemList[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (SceneController.instance)
        {
            SceneController.instance.StartScene();
        }
        else
        {
            Debug.Log("SceneController doesnt exist!");
        }
    }

    public void onTap()
    {
        if (canGoToObject)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500f))
            {
                if (hit.transform.gameObject.tag == "Item")
                {
                    selectItem(hit.transform.gameObject);
                }
                if (hit.transform.gameObject.tag == "Action")
                {
                    hit.transform.gameObject.SetActive(false);
                    SelectAction(hit.transform.gameObject);
                }

            }
        }
    }

    public void selectItem(GameObject hitObject)
    {
        itemHolder = hitObject.transform.parent.gameObject;

        ItemTemplate hitItem = hitObject.GetComponent<ItemTemplate>();
        float zoomValue = hitItem.zoomSize;

        //DO HERE PANEL APPEAR
        showPanel(hitItem);

        CameraController.instance.LerpToZoomPosition(itemHolder, zoomValue);

    }

    public void showPanel(ItemTemplate item)
    {
        Debug.Log(item);

        for (int i = 0; i < prefabsList[item.itemType].prefabsList.Count; i++)
        {
            uiItemList[i].gameObject.SetActive(true);

            uiItemList[i].icon.sprite = prefabsList[item.itemType].prefabsList[i].itemSprite;
            uiItemList[i].itemName.text = prefabsList[item.itemType].prefabsList[i].itemName;
            uiItemList[i].itemCost.text = prefabsList[item.itemType].prefabsList[i].itemPrice.ToString();
            uiItemList[i].itemSus.text = prefabsList[item.itemType].prefabsList[i].itemSustainability.ToString();

            uiItemList[i].selectItem.itemPrefab = prefabsList[item.itemType].prefabsList[i].gameObject;
            uiItemList[i].selectItem.itemName = prefabsList[item.itemType].prefabsList[i].itemName;
            uiItemList[i].selectItem.itemPosition = item.transform.parent;
        }

        LeanTween.alphaCanvas(panelItem, 1f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            titlePanel.text = item.categoryName;
        }).setOnComplete(() =>
        {
            panelItem.interactable = true;
            panelItem.blocksRaycasts = true;
        });
    }

    public void closePanel()
    {
        LeanTween.alphaCanvas(panelItem, 0f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            panelItem.interactable = false;
            panelItem.blocksRaycasts = false;
        }).setOnComplete(() =>
        {
            for (int i = 0; i < uiItemList.Count; i++)
            {
                uiItemList[i].gameObject.SetActive(false);
            }
        });
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
        indexItemType = itemSelected.itemType;
        itemType[indexItemType].SetActive(true);
    }
    public void CheckAndDestroyItem(GameObject newPrefab, string name)
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
                Debug.Log(name);

                GameController.instance.addReportLine("Adicionado item " + name);
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
        StartCoroutine(ZoomToActionAndReturn(hitAction));
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
        // ActionsAnimations.instance.CallFirstCoroutine();
        actionsAnimations[indexSelected].CallFirstCoroutine(indexSelected);

        GameController.instance.addReportLine("Ação iniciada: " + actionSelected.actionName + ".");
    }

    public void ChangeRequest(SelectItem item)
    {
        confirmBtn.onClick.RemoveAllListeners();
        if (confirmBtn != null)
        {
            // confirmBtn.onClick.AddListener(() => { ChangeItem(item); });
            confirmBox.SetActive(true);
        }
        else
        {
            Debug.Log("Need to set Confirm Button on Select Item");
        }
    }
    public void ChangeItem(SelectItem item)
    {
        item.NewItemInstance();
    }

    public void changeScene(string changeSceneName)
    {
        if (SceneController.instance)
        {
            SceneController.instance.ChangeScene(changeSceneName);
        }
        else
        {
            Debug.Log("SceneController doesnt exist!");
        }
    }

    IEnumerator ZoomToActionAndReturn(GameObject actionObject)
    {
        float zoomAction = actionObject.GetComponent<ActionTemplate>().zoomSize;

        CameraController.instance.LerpToZoomPosition(actionObject, zoomAction);
        yield return true;
        // yield return new WaitForSeconds(5f);
        // CameraController.instance.ReturnToBasePosition();

    }
    public void addReportLine(string reportString)
    {
        reportList.Add(reportString);
    }
}

