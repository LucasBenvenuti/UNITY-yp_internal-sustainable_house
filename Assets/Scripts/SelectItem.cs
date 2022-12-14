using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform itemPosition;
    public ItemTemplate itemSelectedTemplate;

    public string itemName;
    public int itemOption;

    public void NewItemInstance()
    {
        GameController.instance.CheckAndDestroyItem(itemPrefab, itemName);
        if (GameController.instance.destroyOriginalItem)
        {
            if (TimerController.instance.tutorialMode)
            {
                // if (Tutorial.instance.currentTutorial == "choose")
                if (Tutorial.instance.textIndex == 5)
                {
                    Tutorial.instance.canContinue = false;
                }
            }

            GameObject prefab = Instantiate(itemPrefab);
            prefab.transform.parent = GameController.instance.itemHolder.transform;
            prefab.transform.SetAsFirstSibling();
            prefab.transform.position = itemPosition.position;
            GameController.instance.destroyOriginalItem = false;

            GameController.instance.uiItemList[GameController.instance.currentOption].outlineCanvas.alpha = 0;
            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = true;
            GameController.instance.uiItemList[GameController.instance.currentOption].ownImage.color = new Color(1f, 1f, 1f, 1f);
            GameController.instance.currentOption = itemSelectedTemplate.itemOption;
            GameController.instance.uiItemList[GameController.instance.currentOption].outlineCanvas.alpha = 1;
            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = false;
            GameController.instance.uiItemList[GameController.instance.currentOption].ownImage.color = GameController.instance.uiItemList[GameController.instance.currentOption].outlineColor;

            DataStorage.instance.sceneObjectsList[itemSelectedTemplate.itemType] = itemSelectedTemplate.itemOption;
            DataStorage.instance.objectNames[itemSelectedTemplate.itemType] = itemSelectedTemplate.itemName;

            GameController.instance.goToObjectShop = true;
            CameraController.instance.LerpToZoomPosition(prefab, itemSelectedTemplate.zoomSize);

            //CALL HERE JSON CHANGE FUNCTION!!!
            StartCoroutine(DataStorage.instance.UploadChange("item", itemSelectedTemplate.categoryName, itemSelectedTemplate.itemName));
        }
    }

    public void ClickOnItem()
    {
        GameController.instance.ChangeRequest(this);
    }

    // public void SimulateChanges(){
    //     UIController.instance.SimulateNewValues(itemSelectedTemplate)
    // }
}
