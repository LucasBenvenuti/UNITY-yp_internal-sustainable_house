using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public GameObject EndCanvas;

    public float totalTime; //sets 12 minutes for timer
    public float actionTimer;
    public float actionBaseTime;
    public TMP_Text timeText;
    public bool timeIsRunning;
    public int indexAction;

    public ReportGenerator report;

    public ActionTemplate[] actions;

    int months;

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

        EndCanvas.SetActive(false);
    }
    void Start()
    {
        WaitForTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsRunning)
        {
            if (totalTime > 0)
            {
                totalTime -= Time.deltaTime;
                ActionsDisplay();
            }
            else
            {
                FinishGame();
            }
        }
        DisplayTimeMinAndSec(totalTime);
    }

    void DisplayTimeMinAndSec(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    void WaitForTutorial()
    {
        Debug.Log("Function for wait finish tutorial and start timer");
        timeIsRunning = true;
    }

    void FinishGame()
    {
        report.PrintScene();

        Debug.Log("Function called when the timer is over for finish the game");
        totalTime = 0;
        timeIsRunning = false;

        EndCanvas.SetActive(true);
    }

    public void ActionsDisplay()
    {
        actionTimer -= Time.deltaTime;
        if (indexAction == actions.Length)
        {
            return;
        }
        else
        {
            bool controlAction = actions[indexAction].haveDoneAction;
            if (actionTimer < 0.1f)
            {
                actions[indexAction].EnableAction();
                if (controlAction)
                {
                    actionTimer = actionBaseTime;
                    actions[indexAction].enabled = false;
                    indexAction++;
                }
            }
        }
    }
}
