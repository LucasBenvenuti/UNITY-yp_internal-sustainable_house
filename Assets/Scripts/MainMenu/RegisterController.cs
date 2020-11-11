﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class RegisterController : MonoBehaviour
{
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;

    public GameObject RegisterCanvas;
    public GameObject StartMenuCanvas;


    public void Awake()
    {
        PlayerPrefs.DeleteKey("Player_Name");

        RegisterCanvas.SetActive(true);
        StartMenuCanvas.SetActive(false);
    }

    public void RegisterUser()
    {
        if (TextValidation(firstNameInput.text) && TextValidation(lastNameInput.text))
        {
            string fullName = firstNameInput.text + " " + lastNameInput.text;

            PlayerPrefs.SetString("Player_Name", fullName);

            Debug.Log("Saved First Name - " + PlayerPrefs.GetString("Player_Name"));

            RegisterCanvas.SetActive(false);
            StartMenuCanvas.SetActive(true);
        }
    }

    public bool TextValidation(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            Debug.Log("WARNING! Input is null or empty!");
            return false;
        }
        else if (string.IsNullOrWhiteSpace(text))
        {
            Debug.Log("WARNING! Input is null or has only white spaces!");
            return false;
        }

        Debug.Log("Input OK!");
        return true;
    }
}