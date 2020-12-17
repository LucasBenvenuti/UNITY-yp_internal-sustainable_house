using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskList : MonoBehaviour
{
    [HideInInspector]
    public bool opened = false;
    public Animator animator;

    public float DelayDuration = 7f;
    float currentTime;

    public CanvasGroup closeCanvas;

    [Space]
    public CanvasGroup objectsObjective;
    public CanvasGroup actionsObjective;

    public TMP_Text objectsText;
    public TMP_Text actionsText;

    void Start()
    {
        actionsObjective.alpha = 1f;
        objectsObjective.alpha = 1f;

        DataStorage.instance.UpdateTaskValues("objects");
        DataStorage.instance.UpdateTaskValues("actions");
    }

    void Update()
    {
        if (opened)
        {
            if (currentTime < 0.5f)
            {
                currentTime = 0;
                OpenCloseTaskList();
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }
    public void OpenCloseTaskList()
    {
        if (!opened)
        {
            objectsText.text = DataStorage.instance.interactedObjectsQuantity + " / " + DataStorage.instance.sceneObjectsList.Count;
            actionsText.text = DataStorage.instance.doneActionsQuantity + " / " + DataStorage.instance.actionsDone.Count;

            if (DataStorage.instance.interactedObjectsQuantity == DataStorage.instance.sceneObjectsList.Count)
            {
                objectsObjective.alpha = 0.7f;
            }
            else if (DataStorage.instance.doneActionsQuantity == DataStorage.instance.actionsDone.Count)
            {
                actionsObjective.alpha = 0.7f;
            }

            animator.SetTrigger("Open");
            closeCanvas.interactable = true;
            currentTime = DelayDuration;
        }
        else
        {
            animator.SetTrigger("Close");
            closeCanvas.interactable = false;
        }

        opened = !opened;
    }

    public void CloseTaskList()
    {
        if (opened)
        {
            animator.SetTrigger("Close");

            opened = false;
        }
    }

}
