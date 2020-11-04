using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class RegisterController : MonoBehaviour
{
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;

    void Start()
    {
        PlayerPrefs.DeleteKey("Player_FirstName");
        PlayerPrefs.DeleteKey("Player_LastName");
    }

    public void RegisterUser()
    {
        //NEED TO CREATE VERIFICATION TO INPUTS THAT WERE NOT USED

        string fullName = firstNameInput.text + " " + lastNameInput.text;

        PlayerPrefs.SetString("Player_Name", fullName);

        Debug.Log("Saved First Name - " + PlayerPrefs.GetString("Player_Name"));
    }
}
