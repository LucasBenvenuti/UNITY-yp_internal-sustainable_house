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
    }

    public void SendIP()
    {
        Debug.Log("Check IP here!");
        Debug.Log("Current IP to test - " + inputField.text);

        StartCoroutine(DataStorage.instance.Upload(inputField.text));
    }
}