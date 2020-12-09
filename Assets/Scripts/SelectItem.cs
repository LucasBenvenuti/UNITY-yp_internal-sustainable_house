﻿using System.Collections;
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
        Debug.Log("STARTED NEW ITEM INSTANCE");

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

            GameObject prefab = Instantiate(itemPrefab);
            prefab.transform.parent = GameController.instance.itemHolder.transform;
            prefab.transform.SetAsFirstSibling();
            prefab.transform.position = itemPosition.position;
            GameController.instance.destroyOriginalItem = false;

            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = true;
            GameController.instance.currentOption = itemSelectedTemplate.itemOption;
            GameController.instance.uiItemList[GameController.instance.currentOption].button.interactable = false;

            DataStorage.instance.sceneObjectsList[itemSelectedTemplate.itemType] = itemSelectedTemplate.itemOption;

            GameController.instance.goToObjectShop = true;
            CameraController.instance.LerpToZoomPosition(prefab, itemSelectedTemplate.zoomSize);

            Debug.Log(itemSelectedTemplate.particleSystem);
        }
    }
    public void ClickOnItem()
    {
        GameController.instance.ChangeRequest(this);
    }
}
