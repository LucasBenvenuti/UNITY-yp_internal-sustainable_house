using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTemplate : MonoBehaviour
{
    public int actionIndex;
    public int actionCost;
    bool haveDoneAction;

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
    private void Update()
    {
        if (TimerController.instance.monthCheck)
        {
            haveDoneAction = false;
        }
    }

    public void DoneAction()
    {
        if (!haveDoneAction)
        {
            haveDoneAction = true;
            Debug.Log("you finished this action with sucess");
        }
        else
        {
            Debug.Log("u alredy done this action try again next month");
        }
    }
}
