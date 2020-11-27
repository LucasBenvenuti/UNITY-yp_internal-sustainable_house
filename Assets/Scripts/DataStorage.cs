using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance;

    public bool soundMuted = false;
    public string userName;
    public bool hasProgress = false;
    public float currentTime;
    public float currentMoney;
    public float currentSustainability;
    public List<int> sceneObjectsList;
    public List<string> reportList;
    public List<bool> actionsDone;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addReportLine(string reportString)
    {
        reportList.Add(reportString);
    }

    public void DeleteAllData()
    {
        userName = "";
        hasProgress = false;
        currentTime = 0;
        currentMoney = 0;
        currentSustainability = 0;

        for (int i = 0; i < sceneObjectsList.Count; i++)
        {
            sceneObjectsList[i] = -1;
        }
        for (int i = 0; i < actionsDone.Count; i++)
        {
            actionsDone[i] = false;
        }

        reportList.Clear();
    }
}
