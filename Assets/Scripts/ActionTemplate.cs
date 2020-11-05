using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTemplate : MonoBehaviour
{
    public int actionIndex;
    public int actionCost;
    public bool haveDoneAction;
    public SpriteRenderer actionSprite;

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
            actionSprite.color = Color.gray;
            //actionSprite.enabled = false;
            Debug.Log("you finished this action with sucess");
            haveDoneAction = true;
        }
        else
        {
            Debug.Log("u alredy done this action try again next month");
        }
    }

    public void EnableAction()
    {
        actionSprite.color = Color.cyan;
        //actionSprite.enabled = true;
        haveDoneAction = false;
    }
}