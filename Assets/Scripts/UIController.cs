using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    //sliders
    public Slider moneySlider;
    public Slider sustainabilitySlider;
    //public Image fillMoneyImage;
    //public Image fillSustainabilityImage;
    //sliders text
    public Text fillSustainabilityText;
    public Text fillMoneyText;
    public float moneyBaseValue;
    public float sustainabilityBaseValue;
    public bool updateValues;
    float newMoneyValue;
    float newSustainabilityValue;
    float moneyMaxValue;
    float sustainabilityMaxValue;

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
        moneyMaxValue = moneySlider.maxValue;
        sustainabilityMaxValue = sustainabilitySlider.maxValue;
    }
    private void Start()
    {
        //fillMoneyImage.fillAmount = 0.5f;
        //fillSustainabilityImage.fillAmount = 0.5f;
        //fillMoneyText.text = "Money: " + fillMoneyImage.fillAmount;
        //fillSustainabilityText.text = "Sustainability: " + fillSustainabilityImage.fillAmount;
        CheckBaseValues();
        moneySlider.value = moneyMaxValue - moneyBaseValue;
        sustainabilitySlider.value = sustainabilityBaseValue;
        fillMoneyText.text = "Money: " + moneySlider.value;
        fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
    }
    private void LateUpdate()
    {
        if (salaryCheck)
        {
            PaySalary();
            salaryCheck = false;
        }
    }

    //public void UpdateFillIndicators()
    //{
    //    newMoneyValue = 0;
    //    newSustainabilityValue = 0;
    //    for (int i = 0; i < itemsSelectables.Length; i++)
    //    {
    //        newMoneyValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
    //        newSustainabilityValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
    //        print("new money:" + newMoneyValue);
    //        print("new sus:" + newSustainabilityValue);
    //    }
    //    if (newMoneyValue != moneyBaseValue || newSustainabilityValue != sustainabilityBaseValue)
    //    {
    //        moneySlider.value = controlMoney - Mathf.Abs(moneyBaseValue - newMoneyValue);
    //        sustainabilitySlider.value = newSustainabilityValue;
    //        fillMoneyText.text = "Money: " + moneySlider.value;
    //        fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
    //    }
    //}

    public void CheckBaseValues()
    {
        moneyBaseValue = 0;
        sustainabilityBaseValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
            print("checked money value:" + moneyBaseValue);
            print("checked sustainability value:" + sustainabilityBaseValue);
            print("/////////");
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

    public bool NewUpdateValues(float price, float sustainability)
    {
        controlMoney = moneySlider.value;
        controlSustainability = sustainabilitySlider.value;
        Debug.Log("PRICE: " + price);
        if (controlMoney < price)
        {
            Debug.Log("VOCE NAO TEM DINHEIRO SUFICIENTE");
            return false;
        }
        else
        {

            moneySlider.value = controlMoney - price;
            fillMoneyText.text = "Money: " + moneySlider.value;
            float newSusValue = controlSustainability + sustainability;
            if(newSusValue < 0)
            {
                sustainabilitySlider.value = 0;
                Debug.Log("SUSTENTABILIDADE NEGATIVA");
            }
            else
            {
                sustainabilitySlider.value = newSusValue;
            }
            fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
            return true;
        }
    }
}