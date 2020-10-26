using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image fillMoneyImage;
    public Image fillSustainabilityImage;
    public Text fillSustainabilityText;
    public Text fillMoneyText;
    public float moneyBaseValue;
    public float sustainabilityBaseValue;
    public bool updateValues;
    float newMoneyValue;
    float newSustainabilityValue;

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
    }
    private void Start()
    {
        fillMoneyImage.fillAmount = 0.5f;
        fillSustainabilityImage.fillAmount = 0.5f;
        fillMoneyText.text = "Money: " + fillMoneyImage.fillAmount;
        fillSustainabilityText.text = "Sustainability: " + fillSustainabilityImage.fillAmount;
    }
    private void LateUpdate()
    {
        if (updateValues)
        {
            UpdateFillIndicators();
            updateValues = false;
        }
    }

    public void UpdateFillIndicators()
    {
        newMoneyValue = 0;
        newSustainabilityValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            newMoneyValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice / 10);
            newSustainabilityValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability / 10);
            print("new money:" + newMoneyValue);
            print("new sus:" + newSustainabilityValue);
        }
        if (newMoneyValue != moneyBaseValue || newSustainabilityValue != sustainabilityBaseValue)
        {
            fillMoneyImage.fillAmount = newMoneyValue;
            fillSustainabilityImage.fillAmount = newSustainabilityValue;
            fillMoneyText.text = "Money: " + fillMoneyImage.fillAmount;
            fillSustainabilityText.text = "Sustainability: " + fillSustainabilityImage.fillAmount;
        }

    }

    public void CheckBaseValues()
    {
        moneyBaseValue = 0;
        sustainabilityBaseValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice / 10);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability / 10);
            print("checked money value:" + moneyBaseValue);
            print("checked sustainability value:" + sustainabilityBaseValue);
            print("/////////");
        }
    }
}