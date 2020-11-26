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
        CheckBaseValues();
        moneySlider.value = moneyBaseValue;
        sustainabilitySlider.value = sustainabilityBaseValue;

        GameController.instance.addReportLine("Jogo iniciado.");
        GameController.instance.addReportLine("Recursos iniciais: " + controlMoney + ".");
        GameController.instance.addReportLine("Sustentabilidade inicial: " + sustainabilityBaseValue + ".");
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

        float newMoney = moneyMaxValue - moneyBaseValue;

        // GameController.instance.addReportLine("Recursos anteriores: " + controlMoney + ". Novos Recursos: " + newMoney + ".");

        controlMoney = newMoney;
    }

    public void NewUpdateValues(float oldPrice, float oldSus, float newPrice, float newSus)
    {
        // controlMoney = moneySlider.value;
        // controlSustainability = sustainabilitySlider.value;
        // Debug.Log("PRICE: " + price);
        // // if (controlMoney < price)
        // // {
        // //     Debug.Log("VOCE NAO TEM DINHEIRO SUFICIENTE");
        // //     return false;
        // // }
        // // else
        // // {
        // GameController.instance.addReportLine("Recursos anteriores: " + controlMoney + ". Novos Recursos: " + (controlMoney + price) + ".");
        // GameController.instance.addReportLine("Sustentabilidade anterior: " + controlSustainability + ". Nova Sustentabilidade: " + (controlSustainability + sustainability) + ".");

        // float newSusValue = controlSustainability + sustainability;
        // float newMoneyValue = controlMoney + price;
        // // if (newSusValue < 0)
        // // {
        // //     sustainabilitySlider.value = 0;
        // //     Debug.Log("SUSTENTABILIDADE NEGATIVA");
        // // }
        // // else
        // //{
        // sustainabilitySlider.value = newSusValue;
        // moneySlider.value = newMoneyValue;
        // //}
        // //return true;
        controlMoney = moneySlider.value;
        controlSustainability = sustainabilitySlider.value;
        controlMoney = controlMoney - oldPrice;
        controlSustainability = controlSustainability - oldSus;
        float newMoneyValue = controlMoney + newPrice;
        float newSusValue = controlSustainability + newSus;
        sustainabilitySlider.value = newSusValue;
        moneySlider.value = newMoneyValue;
    }
}
//}