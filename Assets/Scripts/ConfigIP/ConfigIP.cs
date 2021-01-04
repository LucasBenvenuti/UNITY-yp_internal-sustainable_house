using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ConfigIP : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject invalidIPText;

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

        invalidIPText.SetActive(false);
    }

    public void SendIP()
    {
        IPAddress ip;

        Debug.Log("Check IP here!");
        Debug.Log("Current IP to test - " + inputField.text);

        bool ValidateIP = IPAddress.TryParse(inputField.text, out ip);

        if (ValidateIP)
        {
            invalidIPText.SetActive(false);

            StartCoroutine(DataStorage.instance.Upload(inputField.text));
        }
        else
        {
            Debug.Log("Ip is not valid!");

            invalidIPText.SetActive(true);
        }
    }
}