using System.Collections;
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
    //bools to control actions for update values and receive month salary
    public bool updateValues;
    //floats to set the max values
    public float moneyMaxValue;
    public float sustainabilityMaxValue;
    //floats to check itens values in scene 
    public float moneyBaseValue;
    public float sustainabilityBaseValue;
    float newMoneyValue;
    float newSustainabilityValue;
    float controlMoney;
    float controlSustainability;


    public GameObject[] itemsSelectables;

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
        if (DataStorage.instance.hasProgress)
        {
            return;
        }

        CheckStartBaseValues();
        moneySlider.value = moneyBaseValue;
        sustainabilitySlider.value = sustainabilityBaseValue;

        DataStorage.instance.addReportLine("Jogo iniciado.");
        DataStorage.instance.addReportLine("Recursos iniciais: " + controlMoney + ".");
        DataStorage.instance.addReportLine("Sustentabilidade inicial: " + sustainabilityBaseValue + ".");
    }

    public void CheckStartBaseValues()
    {
        moneyBaseValue = 0;
        sustainabilityBaseValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
        }
        //amount of money availiable 
        float newMoney = moneyMaxValue - moneyBaseValue;
        controlMoney = newMoney;
    }

    public void NewUpdateValues(float oldPrice, float oldSus, float newPrice, float newSus)
    {
        controlMoney = moneySlider.value;
        controlSustainability = sustainabilitySlider.value;
        float neutralMoney = controlMoney - oldPrice;
        float neutralSus = controlSustainability - oldSus;
        float newMoneyValue = neutralMoney + newPrice;
        float newSusValue = neutralSus + newSus;

        DataStorage.instance.addReportLine("Recursos anteriores: " + controlMoney + ". Novos Recursos: " + newMoneyValue + ".");
        DataStorage.instance.addReportLine("Sustentabilidade anterior: " + controlSustainability + ". Nova Sustentabilidade: " + newSusValue + ".");
        sustainabilitySlider.value = newSusValue;
        moneySlider.value = newMoneyValue;
    }
}
