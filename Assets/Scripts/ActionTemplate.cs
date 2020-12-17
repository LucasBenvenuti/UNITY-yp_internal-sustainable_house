using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTemplate : MonoBehaviour
{
    public string actionName;
    public int actionIndex;
    public int actionCost;
    public bool haveDoneAction;
    public SpriteRenderer actionSprite;

    public BoxCollider boxCollider;
    public float zoomSize = 5f;


    void Start()
    {
        if (actionIndex <= 0)
        {
            actionIndex = 0;
        }
        if (actionCost <= 0)
        {
            actionCost = 0;
        }
    }

    public void DoneAction()
    {
        if (!haveDoneAction)
        {
            //actionSprite.enabled = false;
            actionSprite.gameObject.SetActive(false);
            Debug.Log("you finished this action with sucess");
            haveDoneAction = true;

            DataStorage.instance.actionsDone[actionIndex] = true;

            DataStorage.instance.UpdateTaskValues("actions");

            if (DataStorage.instance.doneActionsQuantity == DataStorage.instance.actionsDone.Count && DataStorage.instance.interactedObjectsQuantity == DataStorage.instance.sceneObjectsList.Count)
            {
                TaskList taskList = FindObjectOfType<TaskList>();

                taskList.actionsObjective.alpha = 0.7f;
                taskList.objectsObjective.alpha = 0.7f;

                taskList.ShowFinishButton();
            }
        }
        else
        {
            Debug.Log("u alredy done this action");
        }
    }

    public void EnableAction()
    {
        boxCollider.enabled = true;
        actionSprite.enabled = true;
        haveDoneAction = false;
    }
}
