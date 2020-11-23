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

    public CanvasGroup actionCanvas;
    public TMP_Text actionText;
    public LeanTweenType actionEaseInOut;
    public float actionTweenDuration = 0.3f;

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

    public CanvasGroup inGameTutorial;
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
    float newMoneyValue;
    float newSusValue;

    [HideInInspector]
    public int currentOption;

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

        actionCanvas.alpha = 0;
        actionCanvas.interactable = false;
        actionCanvas.blocksRaycasts = false;

        inGameTutorial.alpha = 0;
        inGameTutorial.interactable = false;
        inGameTutorial.blocksRaycasts = false;

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
                if (hit.transform.gameObject.tag == "BlockRaycast")
                {
                    return;
                }
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
        if (TimerController.instance.tutorialMode)
        {
            if (hitObject.name != "ChuveiroEletrico")
            {
                return;
            }
            else
            {
                if (Tutorial.instance.currentTutorial == "interact")
                {
                    LeanTween.alphaCanvas(Tutorial.instance.handCanvas, 0f, Tutorial.instance.tweenDuration).setEase(Tutorial.instance.easeInOut);
                    Tutorial.instance.canContinue = false;
                }
            }
        }

        itemHolder = hitObject.transform.parent.gameObject;

        ItemTemplate hitItem = hitObject.GetComponent<ItemTemplate>();
        float zoomValue = hitItem.zoomSize;

        //DO HERE PANEL APPEAR
        showPanel(hitItem);

        CameraController.instance.LerpToZoomPosition(itemHolder, zoomValue);

    }

    public void showPanel(ItemTemplate item)
    {
        currentOption = item.itemOption;

        Debug.Log(item);

        for (int i = 0; i < prefabsList[item.itemType].prefabsList.Count; i++)
        {
            if (currentOption == i)
            {
                uiItemList[i].button.interactable = false;
            }
            else
            {
                uiItemList[i].button.interactable = true;
            }


            uiItemList[i].gameObject.SetActive(true);

            uiItemList[i].icon.sprite = prefabsList[item.itemType].prefabsList[i].itemSprite;
            uiItemList[i].itemName.text = prefabsList[item.itemType].prefabsList[i].itemName;
            // uiItemList[i].itemCost.text = prefabsList[item.itemType].prefabsList[i].itemPrice.ToString();
            // uiItemList[i].itemSus.text = prefabsList[item.itemType].prefabsList[i].itemSustainability.ToString();

            if (prefabsList[item.itemType].prefabsList[i].baseSustainability >= 0)
            {
                uiItemList[i].itemSusPos.value = prefabsList[item.itemType].prefabsList[i].baseSustainability;
                uiItemList[i].itemSusNeg.value = 0f;
            }
            else
            {
                uiItemList[i].itemSusNeg.value = -1 * prefabsList[item.itemType].prefabsList[i].baseSustainability;
                uiItemList[i].itemSusPos.value = 0f;

            }
            if (prefabsList[item.itemType].prefabsList[i].basePrice >= 0)
            {
                uiItemList[i].itemCostPos.value = prefabsList[item.itemType].prefabsList[i].basePrice;
                uiItemList[i].itemCostNeg.value = 0f;
            }
            else
            {
                uiItemList[i].itemCostNeg.value = -1 * prefabsList[item.itemType].prefabsList[i].basePrice;
                uiItemList[i].itemCostPos.value = 0f;
            }

            uiItemList[i].selectItem.itemPrefab = prefabsList[item.itemType].prefabsList[i].gameObject;
            uiItemList[i].selectItem.itemName = prefabsList[item.itemType].prefabsList[i].itemName;
            uiItemList[i].selectItem.itemPosition = item.transform.parent;

            uiItemList[i].selectItem.itemSelectedTemplate = prefabsList[item.itemType].prefabsList[i];
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
            float oldItemPrice = prefab.GetComponent<ItemTemplate>().itemPrice;
            float oldItemSus = prefab.GetComponent<ItemTemplate>().itemSustainability;
            if (newOption != optionInScene)
            {
                if (Mathf.Abs(newItemPrice) != Mathf.Abs(oldItemPrice))
                {
                    if (oldItemPrice < newItemPrice)
                    {
                        newMoneyValue = Mathf.Abs(oldItemPrice) + Mathf.Abs(newItemPrice);
                    }
                    else
                    {
                        newMoneyValue = (Mathf.Abs(oldItemPrice) + Mathf.Abs(newItemPrice)) * -1;
                    }
                }
                else
                {
                    if (oldItemPrice < newItemPrice)
                    {
                        newMoneyValue = Mathf.Abs(newItemPrice) * 2;
                    }
                    else if (oldItemPrice > newItemPrice)
                    {
                        newMoneyValue = Mathf.Abs(newItemPrice) * -2;
                    }
                    else
                    {
                        newMoneyValue = 0;
                    }
                }
                if (Mathf.Abs(newItemSus) != Mathf.Abs(oldItemSus))
                {
                    if (oldItemSus < newItemSus)
                    {
                        newSusValue = Mathf.Abs(oldItemSus) + Mathf.Abs(newItemSus);
                    }
                    else
                    {
                        newSusValue = (Mathf.Abs(oldItemSus) + Mathf.Abs(newItemSus)) * -1;
                    }
                }
                else
                {
                    if (oldItemSus < newItemSus)
                    {
                        newSusValue = Mathf.Abs(newItemSus) * 2;
                    }
                    else if (oldItemSus > newItemSus)
                    {
                        newSusValue = Mathf.Abs(newItemSus) * -2;
                    }
                    else
                    {
                        newSusValue = 0;
                    }
                }
                Debug.Log(name);
                GameController.instance.addReportLine("Adicionado item " + name);
                UIController.instance.NewUpdateValues(newMoneyValue, newSusValue);
                Destroy(prefab);
                destroyOriginalItem = true;
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
    }

    IEnumerator PanelOff()
    {
        yield return new WaitForSeconds(3f);
        panelAction.SetActive(false);
        StopCoroutine(PanelOff());
    }

    void DisplayActionTypeUI(GameObject go)
    {
        // for (int i = 0; i < actionType.Length; i++)
        // {
        //     actionType[i].SetActive(false);
        // }
        if (actionSelected != go.GetComponent<ActionTemplate>())
        {
            actionSelected = go.GetComponent<ActionTemplate>();
        }
        actionText.text = actionSelected.actionName;
        LeanTween.alphaCanvas(actionCanvas, 1f, actionTweenDuration).setEase(actionEaseInOut).setOnComplete(() =>
        {
            actionCanvas.interactable = true;
            actionCanvas.blocksRaycasts = true;
        });

        actionSelected.DoneAction();
        int indexSelected = actionSelected.actionIndex;
        // actionType[indexSelected].SetActive(true);
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

    public void showInGameTutorial()
    {
        LeanTween.alphaCanvas(inGameTutorial, 1f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
        {
            inGameTutorial.interactable = true;
            inGameTutorial.blocksRaycasts = true;
        });
    }

    public void hideInGameTutorial()
    {
        LeanTween.alphaCanvas(inGameTutorial, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
        {
            inGameTutorial.interactable = false;
            inGameTutorial.blocksRaycasts = false;
        });
    }

    public void addReportLine(string reportString)
    {
        reportList.Add(reportString);
    }

}

