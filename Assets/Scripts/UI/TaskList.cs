using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskList : MonoBehaviour
{
    public static TaskList instance;

    [HideInInspector]
    public bool opened = false;
    public Animator animator;

    public float DelayDuration = 7f;
    float currentTime;

    public CanvasGroup closeCanvas;

    [Space]
    [Space]
    public CanvasGroup objectsObjective;
    public CanvasGroup actionsObjective;

    public TMP_Text objectsText;
    public TMP_Text actionsText;

    [Space]
    [Space]
    public CanvasGroup finishButton;
    [Space]
    public float tweenDuration = 0.5f;
    public LeanTweenType easeInOut;

    void Start()
    {
        finishButton.interactable = false;
        finishButton.blocksRaycasts = false;
        finishButton.alpha = 0f;

        actionsObjective.alpha = 1f;
        objectsObjective.alpha = 1f;

        DataStorage.instance.UpdateTaskValues("objects");
        DataStorage.instance.UpdateTaskValues("actions");

        if (DataStorage.instance.doneActionsQuantity == DataStorage.instance.actionsDone.Count && DataStorage.instance.interactedObjectsQuantity == DataStorage.instance.sceneObjectsList.Count)
        {
            actionsObjective.alpha = 0.7f;
            objectsObjective.alpha = 0.7f;

            ShowFinishButton();
        }
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

    public void ShowFinishButton()
    {
        LeanTween.alphaCanvas(finishButton, 1f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
        {
            finishButton.interactable = true;
            finishButton.blocksRaycasts = true;
        });
    }

    public void HideFinishButton()
    {
        if (finishButton.interactable = false)
        {
            return;
        }

        if (GameController.instance.actionCanvas.alpha == 1f)
        {
            LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
            {
                GameController.instance.actionCanvas.interactable = false;
                GameController.instance.actionCanvas.blocksRaycasts = false;
            });
        }
        if (GameController.instance.panelItem.alpha == 1)
        {
            LeanTween.alphaCanvas(GameController.instance.panelItem, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
            {
                GameController.instance.panelItem.interactable = false;
                GameController.instance.panelItem.blocksRaycasts = false;
            });
        }

        LeanTween.alphaCanvas(finishButton, 0f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            finishButton.interactable = false;
            finishButton.blocksRaycasts = false;
        });
    }

}
