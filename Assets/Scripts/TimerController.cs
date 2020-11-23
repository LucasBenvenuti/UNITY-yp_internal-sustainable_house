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
    public int indexAction;

    public ReportGenerator report;

    public ActionTemplate[] actions;

    int months;

    [HideInInspector]
    public bool tutorialMode;
    [HideInInspector]
    public bool inGame = true;

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
        if (!tutorialMode && inGame)
        {
            if (totalTime > 0)
            {
                totalTime -= Time.deltaTime;
                ActionsDisplay();
                DisplayTimeMinAndSec(totalTime);
            }
            else
            {
                FinishGame();
            }
        }
    }

    void DisplayTimeMinAndSec(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StartGame()
    {
        Tutorial.instance.closeStoreBtn.enabled = true;
    }

    void WaitForTutorial()
    {
        Debug.Log("Function for wait finish tutorial and start timer");

        Tutorial.instance.StartTutorial();
        // Tutorial.instance.SkipTutorial();
    }

    void FinishGame()
    {
        // report.PrintScene();

        Debug.Log("Function called when the timer is over for finish the game");
        timeText.text = string.Format("{0:00}:{1:00}", 0, 0);
        inGame = false;

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
