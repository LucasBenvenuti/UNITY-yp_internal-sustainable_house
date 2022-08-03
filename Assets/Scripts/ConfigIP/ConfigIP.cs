using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class ConfigIP : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject invalidIPText;

    public Button okButton;

    public bool PermissionBool;

    IEnumerator Start()
    {
        inputField.interactable = false;
        okButton.interactable = false;

        yield return null;

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return new WaitForSeconds(1f);

            Debug.Log("Waiting for Permission");
        }
#endif

        if (SceneController.instance)
        {
            SceneController.instance.StartScene();

            inputField.interactable = true;
            okButton.interactable = true;
        }
        else
        {
            Debug.Log("Scene Controller doesnt exist on current scene or it is disabled.");
        }

        invalidIPText.SetActive(false);

        Debug.Log(PlayerPrefs.GetString("savedIP"));

        if(PlayerPrefs.GetString("savedIP") != "")
        {
            inputField.text = PlayerPrefs.GetString("savedIP");

            okButton.interactable = false;
            inputField.interactable = false;

            yield return new WaitForSeconds(1f);

            SendIP();
        }
    }

    public void SendIP()
    {
        okButton.interactable = false;
        inputField.interactable = false;

        string finalIP = "";

        if(PlayerPrefs.GetString("savedIP") != "")
        {
            finalIP = PlayerPrefs.GetString("savedIP");

            Debug.Log("From PLAYERPREFS - " + finalIP);

            inputField.text = finalIP;
        }
        else
        {
            finalIP = inputField.text;
        }

        IPAddress ip;
        
        Debug.Log("Check IP here!");
        Debug.Log("Current IP to test - " + inputField.text);

        bool ValidateIP = IPAddress.TryParse(inputField.text, out ip);

        if (ValidateIP)
        {
            invalidIPText.SetActive(false);

            PlayerPrefs.SetString("savedIP", finalIP);
            
            Debug.Log(PlayerPrefs.GetString("savedIP"));

            StartCoroutine(DataStorage.instance.Upload(inputField.text));
        }
        else
        {
            Debug.Log("Ip is not valid!");

            invalidIPText.SetActive(true);
            okButton.interactable = true;
            inputField.interactable = true;
        }
    }
}