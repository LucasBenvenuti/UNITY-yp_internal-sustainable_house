using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public float totalTime = 720; //sets 12 minutes for timer
    public Text timeText;
    public Text monthText;
    public bool timeIsRunning;
    public bool monthCheck;

    float timeBaseMonth;
    int months;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(this);
        }
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
            }
            else
            {
                FinishGame();
            }
        }
        DisplayTimeMinAndSec(totalTime);
        MonthsCounter();
    }

    void DisplayTimeMinAndSec(float timeToDisplay){
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
        Debug.Log("Function called when the timer is over for finish the game");
        totalTime = 0;
        timeIsRunning = false;
    }
    public void MonthsCounter()
    {
        timeBaseMonth += Time.deltaTime;
        if(timeBaseMonth >= 59.9f)
        {
            months++;
            monthCheck = true;
            timeBaseMonth = 0;
            PaySalary();
        }
        monthText.text = "Month: " + months;
    }

    void PaySalary()
    {
        Debug.Log("function to pay a salary");
        monthCheck = false;
    }

}
