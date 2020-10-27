using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public float totalTime = 600;
    public bool timeIsRunning;
    public Text timeText;
    public Text monthText;

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
        timeIsRunning = true;
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
                totalTime = 0;
                timeIsRunning = false;
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

    void MonthsCounter()
    {
        timeBaseMonth += Time.deltaTime;
        if(timeBaseMonth >= 59.9f)
        {
            months++;
            timeBaseMonth = 0;
        }
        monthText.text = "Month: " + months;
    }

}
