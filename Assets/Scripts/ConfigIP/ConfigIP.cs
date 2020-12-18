using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


[System.Serializable]
public class FormJSON
{
    public string school;
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

public class ConfigIP : MonoBehaviour
{
    public TMP_InputField inputField;

    public string JSON_File;

    void Start()
    {
        if (SceneController.instance)
        {
            SceneController.instance.StartScene();
        }
        else
        {
            Debug.Log("Scene Controller doesnt exist on current scene or it is disabled.");
        }

        FormJSON formJSON = new FormJSON();

        formJSON.school = "Teste Escola";
        formJSON.user = "Teste Usuário";
        formJSON.finished = false;
        formJSON.image = "base64";
        formJSON.chooses.Add(new ChoosesJSON("Climatização", "Ventilador"));
        formJSON.chooses.Add(new ChoosesJSON("Secagem de Roupa", "Secadora"));
        formJSON.chooses.Add(new ChoosesJSON("Lâmpadas", "LED"));
        formJSON.chooses.Add(new ChoosesJSON("Janelas", "Janelas Maiores"));
        formJSON.chooses.Add(new ChoosesJSON("Abastecimento de energia elétrica", "Poste de rede pública"));
        formJSON.chooses.Add(new ChoosesJSON("Abastecimento de água", "Hidrômetro de rede pública"));
        formJSON.chooses.Add(new ChoosesJSON("Esgotamento Sanitário", "Rede Pública"));
        formJSON.chooses.Add(new ChoosesJSON("Chuveiro", "Chuveiro Elétrico"));
        formJSON.chooses.Add(new ChoosesJSON("Descarga", "Vaso com caixa acoplada e descarga de acionamento único"));
        formJSON.chooses.Add(new ChoosesJSON("Torneira", "Torneira Convencional"));
        formJSON.chooses.Add(new ChoosesJSON("Geladeira", "Geladeira com selo Procel C"));
        formJSON.chooses.Add(new ChoosesJSON("Lixeiras", "2 lixeiras sendo uma com reciclagem"));
        formJSON.chooses.Add(new ChoosesJSON("Máquina de Lavar", "Máquina de abertura superior com selo Procel"));
        formJSON.chooses.Add(new ChoosesJSON("Água de Reuso", "Sem coleta de água de reuso"));
        formJSON.chooses.Add(new ChoosesJSON("Televisão", "Televisão de tela plana sem selo Procel"));
        formJSON.chooses.Add(new ChoosesJSON("Telhado", "Telha de cerâmica"));

        formJSON.actions.Add(new ActionsJSON("Lavar Louça", true));
        formJSON.actions.Add(new ActionsJSON("Celular fora da tomada", true));
        formJSON.actions.Add(new ActionsJSON("Ler livro com TV desligada", false));
        formJSON.actions.Add(new ActionsJSON("Lavar Roupa", false));
        formJSON.actions.Add(new ActionsJSON("Escovar os dentes", false));

        JSON_File = JsonUtility.ToJson(formJSON);

        Debug.Log(JSON_File);
    }

    public void SendIP()
    {
        Debug.Log("Check IP here!");
        Debug.Log("Current IP to test - " + inputField.text);

        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        UnityWebRequest www = UnityWebRequest.Put("http://192.168.127.1:2433/reports", JSON_File);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }

    }
}