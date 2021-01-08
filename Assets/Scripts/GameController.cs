using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPOOutline;

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
    public GameObject[] itemType;
    public GameObject[] actionType;

    public CanvasGroup inGameTutorial;
    public CanvasGroup panelItem;
    public TMP_Text titlePanel;
    public List<ItemOption> uiItemList;

    public GameObject panelAction;
    public GameObject itemHolder;
    public CanvasGroup[] confirmBox;
    public Button[] itemUIBtn;
    public int confirmBoxIndex;
    public int indexItemType;
    public bool destroyOriginalItem;
    public bool itemPanelActive;
    public bool canGoToObject = true;
    public bool goToObjectShop = false;
    public float itemSelectedPrice;

    public List<ListItems> prefabsList = new List<ListItems>();

    public ActionsAnimations[] actionsAnimations;

    public float tweenDuration = 0.3f;
    public LeanTweenType easeInOut;

    [HideInInspector]
    float newMoneyValue;
    float newSusValue;

    [HideInInspector]
    public int currentOption;

    public bool tvOn = true;

    public bool simulateChange = false;

    [HideInInspector]
    float simulateOldItemPrice;
    float simulateOldItemSus;


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

        // confirmBox.alpha = 0;
        // confirmBox.interactable = false;
        // confirmBox.blocksRaycasts = false;

        for (int i = 0; i < uiItemList.Count; i++)
        {
            uiItemList[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        AudioController.instance.backgroundIngameSource.Play();
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
        if (canGoToObject && TimerController.instance.inGame)
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
                // if (Tutorial.instance.currentTutorial == "interact")
                if (Tutorial.instance.textIndex == 3)
                {
                    LeanTween.alphaCanvas(Tutorial.instance.handCanvas, 0f, Tutorial.instance.tweenDuration).setEase(Tutorial.instance.easeInOut);
                    LeanTween.alphaCanvas(Tutorial.instance.maskCircleCanvas, 0f, Tutorial.instance.tweenDuration).setEase(Tutorial.instance.easeInOut);
                    Tutorial.instance.canContinue = false;
                }
                else
                {
                    return;
                }
            }
        }
        AudioController.instance.PlayActionsAudio();
        itemHolder = hitObject.transform.parent.gameObject;

        ItemTemplate hitItem = hitObject.GetComponent<ItemTemplate>();
        float zoomValue = hitItem.zoomSize;

        //DO HERE PANEL APPEAR
        showPanel(hitItem);
        CameraController.instance.LerpToZoomPosition(hitObject, zoomValue);

        DataStorage.instance.openedObjectsMenu[hitItem.itemType] = true;

        DataStorage.instance.UpdateTaskValues("objects");

        if (DataStorage.instance.doneActionsQuantity == DataStorage.instance.actionsDone.Count && DataStorage.instance.interactedObjectsQuantity == DataStorage.instance.sceneObjectsList.Count)
        {
            TaskList taskList = FindObjectOfType<TaskList>();

            taskList.actionsObjective.alpha = 0.7f;
            taskList.objectsObjective.alpha = 0.7f;

            taskList.ShowFinishButton();
        }
    }

    public void showPanel(ItemTemplate item)
    {
        currentOption = item.itemOption;

        for (int i = 0; i < prefabsList[item.itemType].prefabsList.Count; i++)
        {
            if (currentOption == i)
            {
                uiItemList[i].ownImage.color = GameController.instance.uiItemList[GameController.instance.currentOption].outlineColor;
                uiItemList[i].outlineCanvas.alpha = 1;
                uiItemList[i].button.interactable = false;
            }
            else
            {
                uiItemList[i].outlineCanvas.alpha = 0;
                uiItemList[i].ownImage.color = new Color(1f, 1f, 1f, 1f);
                uiItemList[i].button.interactable = true;
            }


            uiItemList[i].gameObject.SetActive(true);

            uiItemList[i].icon.sprite = prefabsList[item.itemType].prefabsList[i].itemSprite;
            uiItemList[i].itemName.text = prefabsList[item.itemType].prefabsList[i].itemName;

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
            uiItemList[i].selectItem.itemOption = prefabsList[item.itemType].prefabsList[i].itemOption;
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
                // DataStorage.instance.addReportLine("Adicionado item " + name);
                UIController.instance.NewUpdateValues(oldItemPrice, oldItemSus, newItemPrice, newItemSus);
                Destroy(prefab);
                destroyOriginalItem = true;
            }
            else
            {
                destroyOriginalItem = false;
            }
        }
    }

    public void SelectAction(GameObject hitAction)
    {
        AudioController.instance.PlayActionsAudio();
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
        actionsAnimations[indexSelected].CallFirstCoroutine(indexSelected);

        //CALL HERE JSON CHANGE FUNCTION!!!
        // StartCoroutine(DataStorage.instance.Upload(null));
        StartCoroutine(DataStorage.instance.UploadChange("action", actionSelected.actionIndex.ToString(), null));

        // DataStorage.instance.addReportLine("Ação iniciada: " + actionSelected.actionName + ".");
    }

    public void ChangeRequest(SelectItem item)
    {
        //SAVE CURRENT VALUES BEFORE SIMULATE
        if (!simulateChange)
        {
            UIController.instance.CheckBaseSliderValues();
            simulateChange = true;
        }

        if (itemHolder != null)
        {
            GameObject prefab = itemHolder.transform.GetChild(0).gameObject;
            simulateOldItemPrice = prefab.GetComponent<ItemTemplate>().itemPrice;
            simulateOldItemSus = prefab.GetComponent<ItemTemplate>().itemSustainability;
        }
        UIController.instance.SimulateUpdateValues(simulateOldItemPrice, simulateOldItemSus, item.itemSelectedTemplate.itemPrice, item.itemSelectedTemplate.itemSustainability);
        if (confirmBox[item.itemOption] != null)
        {
            for (int i = 0; i <= 3; i++)
            {
                confirmBox[i].gameObject.SetActive(false);
                if (i != currentOption)
                {
                    Debug.Log("item option: " + currentOption);
                    itemUIBtn[i].interactable = true;
                }
                else
                {
                    itemUIBtn[i].interactable = false;
                }
            }
            ShowConfirmBox(item.itemOption);
        }
        else
        {
            Debug.Log("Need to set Confirm Button on Select Item");
        }
    }
    public void ChangeItem(SelectItem item)
    {
        item.NewItemInstance();
        simulateChange = false;
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

    public void SetLastScene()
    {
        if (!TimerController.instance.tutorialMode)
        {
            DataStorage.instance.hasProgress = true;

            DataStorage.instance.currentTime = TimerController.instance.totalTime;
            DataStorage.instance.currentMoney = UIController.instance.moneySlider.value;
            DataStorage.instance.currentSustainability = UIController.instance.sustainabilitySlider.value;
        }
    }

    public void GetStorageValues()
    {
        TimerController.instance.totalTime = DataStorage.instance.currentTime;
        UIController.instance.moneySlider.value = DataStorage.instance.currentMoney;
        UIController.instance.sustainabilitySlider.value = DataStorage.instance.currentSustainability;
        UIController.instance.moneyPositiveSlider.value = DataStorage.instance.currentMoney;
        UIController.instance.sustainabilityPositiveSlider.value = DataStorage.instance.currentSustainability;
        UIController.instance.moneyNegativeSlider.value = DataStorage.instance.currentMoney;
        UIController.instance.sustainabilityNegativeSlider.value = DataStorage.instance.currentSustainability;
    }

    public void ShowConfirmBox(int index)
    {
        confirmBox[index].gameObject.SetActive(true);
        itemUIBtn[index].interactable = false;
        confirmBoxIndex = index;
        LeanTween.alphaCanvas(confirmBox[index], 1f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            confirmBox[index].blocksRaycasts = true;
            confirmBox[index].interactable = true;
        });
    }

    public void HideConfirmBox()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (i != currentOption)
            {
                Debug.Log("item option: " + currentOption);
                itemUIBtn[i].interactable = true;
            }
            else
            {
                itemUIBtn[i].interactable = false;
            }
        }
        confirmBox[confirmBoxIndex].gameObject.SetActive(false);
        LeanTween.alphaCanvas(confirmBox[confirmBoxIndex], 0f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            confirmBox[confirmBoxIndex].blocksRaycasts = false;
            confirmBox[confirmBoxIndex].interactable = false;
        });

        if (simulateChange)
        {
            simulateChange = false;
        }
    }
}

