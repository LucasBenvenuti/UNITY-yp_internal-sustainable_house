using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

[System.Serializable]
public class FormJSON
{
    public string school;
    public Int64 id;
    public string user;
    public bool finished;
    public string image;
    public List<ChoosesJSON> chooses = new List<ChoosesJSON>();
    public List<ActionsJSON> actions = new List<ActionsJSON>();
}

[System.Serializable]
public class ChoosesJSON
{
    public string item;
    public string answer;


    public ChoosesJSON(string item, string answer)
    {
        this.item = item;
        this.answer = answer;
    }
}

[System.Serializable]
public class ActionsJSON
{
    public string name;
    public bool result;

    public ActionsJSON(string name, bool result)
    {
        this.name = name;
        this.result = result;
    }
}

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance;

    public string ipAddress;
    public Int64 uniqueID;
    public string JSON_File;
    public bool getJSON = false;

    // [HideInInspector]    
    string defaultImage = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAsTAAALEwEAmpwYAAAF0WlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDUgNzkuMTYzNDk5LCAyMDE4LzA4LzEzLTE2OjQwOjIyICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxOSAoV2luZG93cykiIHhtcDpDcmVhdGVEYXRlPSIyMDIxLTA5LTI0VDEwOjI2OjI5LTAzOjAwIiB4bXA6TWV0YWRhdGFEYXRlPSIyMDIxLTA5LTI0VDEwOjI2OjI5LTAzOjAwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAyMS0wOS0yNFQxMDoyNjoyOS0wMzowMCIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDowNWMzNTdhNC1mNTFlLTdkNDktOGJjNy0zZjg0Mjg3NzAxMjAiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDo1NzhiN2RlZi0yYzg3LWEyNDQtYWY0OS01ZjQxNDEzM2NlYTgiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDoyZjk5MzFkMy04NTM0LWJhNGItODEzZS0zNzYzN2JkODgwODkiIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDoyZjk5MzFkMy04NTM0LWJhNGItODEzZS0zNzYzN2JkODgwODkiIHN0RXZ0OndoZW49IjIwMjEtMDktMjRUMTA6MjY6MjktMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE5IChXaW5kb3dzKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6MDVjMzU3YTQtZjUxZS03ZDQ5LThiYzctM2Y4NDI4NzcwMTIwIiBzdEV2dDp3aGVuPSIyMDIxLTA5LTI0VDEwOjI2OjI5LTAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxOSAoV2luZG93cykiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+nEzyuQAAAC5JREFUWIXtzjEBAAAIw7CBf89DBk9qoJm2+Wxf7wAAAAAAAAAAAAAAAAAAAEkOQLQDPbe2lDgAAAAASUVORK5CYII=";

    [Space]
    [Space]
    public string image;

    public bool soundMuted = false;
    public string userName;
    public bool hasProgress = false;
    public float currentTime;
    public float currentMoney;
    public float currentSustainability;
    public List<string> reportList;
    public List<int> sceneObjectsList;
    public List<bool> openedObjectsMenu;
    public List<bool> actionsDone;
    public List<string> objectNames;
    public List<string> actionsName;

    public float startSus;
    public float startResources;

    [HideInInspector]
    public int interactedObjectsQuantity;
    [HideInInspector]
    public int doneActionsQuantity;

    public bool gameFinished = false;

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

        image = defaultImage;
    }

    public void addReportLine(string reportString)
    {
        reportList.Add(reportString);
    }

    public void DeleteAllData()
    {
        DataStorage.instance.gameFinished = false;

        image = defaultImage;

        uniqueID = 0;

        JSON_File = "";

        userName = "";
        hasProgress = false;
        currentTime = 0;
        currentMoney = 0;
        currentSustainability = 0;

        for (int i = 0; i < sceneObjectsList.Count; i++)
        {
            sceneObjectsList[i] = -1;
        }
        for (int i = 0; i < openedObjectsMenu.Count; i++)
        {
            openedObjectsMenu[i] = false;
        }
        for (int i = 0; i < actionsDone.Count; i++)
        {
            actionsDone[i] = false;
        }
        for (int i = 0; i < objectNames.Count; i++)
        {
            objectNames[i] = "";
        }

        reportList.Clear();
    }

    public void UpdateTaskValues(string type)
    {
        if (type == "actions")
        {
            int qty = 0;

            foreach (bool doneAction in actionsDone)
            {
                if (doneAction)
                {
                    qty++;
                }
            }

            doneActionsQuantity = qty;
        }
        else if (type == "objects")
        {
            int qty = 0;

            foreach (bool changedObjects in openedObjectsMenu)
            {
                if (changedObjects)
                {
                    qty++;
                }
            }

            interactedObjectsQuantity = qty;
        }

    }

    public IEnumerator UpdateJSON()
    {
        yield return null;

        FormJSON formJSON = new FormJSON();

        formJSON.school = "Escola";
        formJSON.id = uniqueID;
        formJSON.user = userName;
        formJSON.finished = gameFinished;
        formJSON.image = image;
        formJSON.chooses.Add(new ChoosesJSON("Climatização", objectNames[0]));
        formJSON.chooses.Add(new ChoosesJSON("Secagem de Roupa", objectNames[1]));
        formJSON.chooses.Add(new ChoosesJSON("Lâmpadas", objectNames[2]));
        formJSON.chooses.Add(new ChoosesJSON("Janelas", objectNames[3]));
        formJSON.chooses.Add(new ChoosesJSON("Abastecimento de energia elétrica", objectNames[4]));
        formJSON.chooses.Add(new ChoosesJSON("Abastecimento de água", objectNames[5]));
        formJSON.chooses.Add(new ChoosesJSON("Esgotamento Sanitário", objectNames[6]));
        formJSON.chooses.Add(new ChoosesJSON("Chuveiro", objectNames[7]));
        formJSON.chooses.Add(new ChoosesJSON("Descarga", objectNames[8]));
        formJSON.chooses.Add(new ChoosesJSON("Torneira", objectNames[9]));
        formJSON.chooses.Add(new ChoosesJSON("Geladeira", objectNames[10]));
        formJSON.chooses.Add(new ChoosesJSON("Separação e destinação dos resíduos", objectNames[11]));
        formJSON.chooses.Add(new ChoosesJSON("Máquina de Lavar Roupa", objectNames[12]));
        formJSON.chooses.Add(new ChoosesJSON("Água de Reuso", objectNames[13]));
        formJSON.chooses.Add(new ChoosesJSON("Televisão", objectNames[14]));
        formJSON.chooses.Add(new ChoosesJSON("Telhado", objectNames[15]));

        formJSON.actions.Add(new ActionsJSON(actionsName[0], actionsDone[0]));
        formJSON.actions.Add(new ActionsJSON(actionsName[1], actionsDone[1]));
        formJSON.actions.Add(new ActionsJSON(actionsName[2], actionsDone[2]));
        formJSON.actions.Add(new ActionsJSON(actionsName[3], actionsDone[3]));
        formJSON.actions.Add(new ActionsJSON(actionsName[4], actionsDone[4]));

        JSON_File = JsonUtility.ToJson(formJSON);

        Debug.Log(JSON_File);
    }

    public IEnumerator UpdateChangeJSON(string type, string category, string name)
    {
        yield return null;

        FormJSON formJSON = new FormJSON();

        formJSON.school = "Escola";
        formJSON.id = uniqueID;
        formJSON.user = userName;
        formJSON.finished = gameFinished;
        // formJSON.image = image;

        if (type == "item")
        {
            formJSON.chooses.Add(new ChoosesJSON(category, name));
        }
        else if (type == "action")
        {
            int categoryID = Convert.ToInt32(category);

            formJSON.actions.Add(new ActionsJSON(actionsName[categoryID], true));
        }

        // formJSON.chooses.Add(new ChoosesJSON(category, name);
        // formJSON.chooses.Add(new ChoosesJSON("Secagem de Roupa", objectNames[1]));
        // formJSON.chooses.Add(new ChoosesJSON("Lâmpadas", objectNames[2]));
        // formJSON.chooses.Add(new ChoosesJSON("Janelas", objectNames[3]));
        // formJSON.chooses.Add(new ChoosesJSON("Abastecimento de energia elétrica", objectNames[4]));
        // formJSON.chooses.Add(new ChoosesJSON("Abastecimento de água", objectNames[5]));
        // formJSON.chooses.Add(new ChoosesJSON("Esgotamento Sanitário", objectNames[6]));
        // formJSON.chooses.Add(new ChoosesJSON("Chuveiro", objectNames[7]));
        // formJSON.chooses.Add(new ChoosesJSON("Descarga", objectNames[8]));
        // formJSON.chooses.Add(new ChoosesJSON("Torneira", objectNames[9]));
        // formJSON.chooses.Add(new ChoosesJSON("Geladeira", objectNames[10]));
        // formJSON.chooses.Add(new ChoosesJSON("Lixeiras", objectNames[11]));
        // formJSON.chooses.Add(new ChoosesJSON("Máquina de Lavar", objectNames[12]));
        // formJSON.chooses.Add(new ChoosesJSON("Água de Reuso", objectNames[13]));
        // formJSON.chooses.Add(new ChoosesJSON("Televisão", objectNames[14]));
        // formJSON.chooses.Add(new ChoosesJSON("Telhado", objectNames[15]));

        // formJSON.actions.Add(new ActionsJSON("Lavar Louça", actionsDone[0]));
        // formJSON.actions.Add(new ActionsJSON("Celular fora da tomada", actionsDone[1]));
        // formJSON.actions.Add(new ActionsJSON("Ler livro com TV desligada", actionsDone[2]));
        // formJSON.actions.Add(new ActionsJSON("Lavar Roupa", actionsDone[3]));
        // formJSON.actions.Add(new ActionsJSON("Escovar os dentes", actionsDone[4]));

        JSON_File = JsonUtility.ToJson(formJSON);

        Debug.Log(JSON_File);
    }

    public void UploadJSON()
    {
        if (!DataStorage.instance.hasProgress)
        {
            StartCoroutine(Upload(null));
        }
    }

    public void UploadJSON(string type, string category, string name)
    {
        StartCoroutine(UploadChange(type, category, name));
    }

    public IEnumerator Upload(string url)
    {
        string completeURL = "";

        if (url != null && url != "update")
        {
            completeURL = "http://" + url + ":2433/events/ivTkLsSh5B2OvvAKjYL1/reports";
        }
        else
        {
            completeURL = ipAddress;
        }

        if (completeURL == "")
        {
            yield break;
        }

        var www = new UnityWebRequest(completeURL, "POST");

        if (getJSON)
        {
            yield return UpdateJSON();

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JSON_File);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
        }

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            ConfigIP configIP = FindObjectOfType<ConfigIP>();
            if (configIP)
            {
                configIP.invalidIPText.SetActive(true);
                configIP.okButton.interactable = true;
                configIP.inputField.interactable = true;
            }

            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");

            if (!getJSON)
            {
                ipAddress = completeURL;

                getJSON = true;

                if (SceneController.instance)
                {
                    SceneController.instance.ChangeScene("QRCode");
                }
                else
                {
                    Debug.Log("Scene Controller doesnt exist on current scene or it is disabled.");
                }
            }
        }
    }

    public IEnumerator UploadChange(string type, string category, string name)
    {
        string completeURL = "";

        completeURL = ipAddress;

        if (completeURL == "")
        {
            yield break;
        }

        var www = new UnityWebRequest(completeURL, "POST");

        if (getJSON)
        {
            yield return UpdateChangeJSON(type, category, name);
            // yield return UpdateJSON();

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JSON_File);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
        }

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            ConfigIP configIP = FindObjectOfType<ConfigIP>();
            if (configIP)
            {
                configIP.invalidIPText.SetActive(true);
                configIP.okButton.interactable = true;
                configIP.inputField.interactable = true;
            }

            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload Change complete!");
        }
    }

    public IEnumerator SendLastData()
    {
        yield return Upload(null);

        DeleteAllData();
    }
}
