﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //singleton
    public static UIController instance;
    //sliders
    public Slider moneySlider;
    public Slider sustainabilitySlider;
    //sliders text
    public Text fillSustainabilityText;
    public Text fillMoneyText;
    //bools to control actions for update values and receive month salary
    public bool updateValues;
    public bool salaryCheck;
    //floats to set the max values
    public float moneyMaxValue;
    public float sustainabilityMaxValue;
    //floats to check itens values in scene 
    public float moneyBaseValue;
    public float sustainabilityBaseValue;
    //floats to check new values when update itens
    float newMoneyValue;
    float newSustainabilityValue;
    //floats to control (max value - value of itens in scene)
    float controlMoney;
    float controlSustainability;
    //list of itens that can be modified
    public GameObject[] itemsSelectables;

    public float baseSalary;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(this);
        }
        moneySlider.maxValue = moneyMaxValue;
        sustainabilitySlider.maxValue = sustainabilityMaxValue;
    }
    private void Start()
    {
        CheckBaseValues();
        moneySlider.value = controlMoney;
        sustainabilitySlider.value = sustainabilityBaseValue;
        fillMoneyText.text = "Money: " + moneySlider.value;
        fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
    }
    private void LateUpdate()
    {
        if (updateValues)
        {
            //CheckBaseValues();
            //UpdateFillIndicators();

            updateValues = false;
        }
        if (salaryCheck)
        {
            PaySalary();
            salaryCheck = false;
        }
    }

    public void UpdateFillIndicators()
    {
        newMoneyValue = 0;
        newSustainabilityValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            newMoneyValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
            newSustainabilityValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
            print("new money:" + newMoneyValue);
            print("new sus:" + newSustainabilityValue);
        }
        if (newMoneyValue != moneyBaseValue || newSustainabilityValue != sustainabilityBaseValue)
        {
            moneySlider.value = controlMoney - Mathf.Abs(moneyBaseValue - newMoneyValue);
            sustainabilitySlider.value = newSustainabilityValue;
            fillMoneyText.text = "Money: " + moneySlider.value;
            fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
        }
    }

    public void CheckBaseValues()
    {
        moneyBaseValue = 0;
        sustainabilityBaseValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
        }
        //amount of money availiable 
        controlMoney = moneyMaxValue - moneyBaseValue;
    }

    public void PaySalary()
    {
        controlMoney = moneySlider.value;
        moneySlider.value = controlMoney + baseSalary;
        fillMoneyText.text = "Money: " + moneySlider.value;
    }

    public void NewUpdateValues(float price, float sustainability)
    {
        controlMoney = moneySlider.value;
        moneySlider.value = controlMoney - price;
        fillMoneyText.text = "Money: " + moneySlider.value;

            sustainabilitySlider.value = sustainabilityBaseValue + sustainability;
        
        fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
        CheckBaseValues();
    }
}