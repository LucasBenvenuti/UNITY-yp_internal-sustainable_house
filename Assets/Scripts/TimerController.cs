using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public TaskList taskList;

    public CanvasGroup EndCanvas;
    public GameObject highText;
    public GameObject lowText;
    public GameObject highGirl;
    public GameObject lowGirl;
    public float endTweenDuration = 0.3f;
    public LeanTweenType easeInOutEnd;

    public float totalTime; //sets 10 minutes for timer
    public float actionTimer;
    public float actionBaseTime;
    public TMP_Text timeText;
    public int indexAction;

    public ReportGenerator report;

    public ActionTemplate[] actions;

    int months;

    [HideInInspector]
    public bool tutorialMode;
    // [HideInInspector]
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

        EndCanvas.alpha = 0;
        EndCanvas.interactable = false;
        EndCanvas.blocksRaycasts = false;

        highText.SetActive(false);
        lowText.SetActive(false);
        highGirl.SetActive(false);
        lowGirl.SetActive(false);
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
                if (totalTime < 5.1f && !AudioController.instance.playedEndGame)
                {
                    AudioController.instance.PlayEndTimerAudio();
                    AudioController.instance.playedEndGame = true;
                }
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

        if (DataStorage.instance.hasProgress)
        {
            GameController.instance.GetStorageValues();

            Tutorial.instance.SkipTutorial();
        }
    }

    public void FinishGame()
    {
        taskList.HideFinishButton();

        taskList.CloseTaskList();

        StartCoroutine(ReportCoroutine());
    }

    IEnumerator ReportCoroutine()
    {
        inGame = false;

        DataStorage.instance.gameFinished = true;

        StartCoroutine(report.PrintFunc());

        Debug.Log("Function called when the timer is over for finish the game");
        timeText.text = string.Format("{0:00}:{1:00}", 0, 0);

        if (UIController.instance.sustainabilitySlider.value <= DataStorage.instance.startSus)
        {
            lowGirl.SetActive(true);
            lowText.SetActive(true);
        }
        else
        {
            highGirl.SetActive(true);
            highText.SetActive(true);
        }

        // yield return report.PrintFunc();

        yield return new WaitForSeconds(2f);
        AudioController.instance.PlayOpenFinalScreenAudio();

        LeanTween.alphaCanvas(EndCanvas, 1f, endTweenDuration).setEase(easeInOutEnd).setOnComplete(() =>
        {
            EndCanvas.interactable = true;
            EndCanvas.blocksRaycasts = true;
        });
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
            bool controlAction = DataStorage.instance.actionsDone[indexAction];
            if (controlAction)
            {
                actionTimer = actionBaseTime;
                actions[indexAction].enabled = false;
                indexAction++;
            }
            else
            {
                if (actionTimer < 0.1f)
                {
                    actions[indexAction].EnableAction();
                }
            }

        }
    }
}
