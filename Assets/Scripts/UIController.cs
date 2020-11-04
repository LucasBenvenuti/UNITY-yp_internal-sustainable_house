using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //singleton
    public static UIController instance;
<<<<<<< Updated upstream

    public Image fillMoneyImage;
    public Image fillSustainabilityImage;
=======
    //sliders
    public Slider moneySlider;
    public Slider sustainabilitySlider;
    //sliders text
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

=======
    //floats to control (max value - value of itens in scene)
    float controlMoney;
    float controlSustainability;
    //list of itens that can be modified
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        fillMoneyImage.fillAmount = 0.5f;
        fillSustainabilityImage.fillAmount = 0.5f;
        fillMoneyText.text = "Money: " + fillMoneyImage.fillAmount;
        fillSustainabilityText.text = "Sustainability: " + fillSustainabilityImage.fillAmount;
=======
        
        CheckBaseValues();
        moneySlider.value = controlMoney;
        sustainabilitySlider.value = sustainabilityBaseValue;
        fillMoneyText.text = "Money: " + moneySlider.value;
        fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
>>>>>>> Stashed changes
    }
    private void LateUpdate()
    {
        if (updateValues)
        {
            CheckBaseValues();
            UpdateFillIndicators();
            updateValues = false;
        }
        if (salaryCheck)
        {

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
<<<<<<< Updated upstream
            fillMoneyImage.fillAmount = newMoneyValue;
            fillSustainabilityImage.fillAmount = newSustainabilityValue;
            fillMoneyText.text = "Money: " + fillMoneyImage.fillAmount;
            fillSustainabilityText.text = "Sustainability: " + fillSustainabilityImage.fillAmount;
=======
            print("teste");
            moneySlider.value = moneyBaseValue - newMoneyValue;
            sustainabilitySlider.value = newSustainabilityValue;
            fillMoneyText.text = "Money: " + moneySlider.value;
            fillSustainabilityText.text = "Sustainability: " + sustainabilitySlider.value;
>>>>>>> Stashed changes
        }

    }

    public void CheckBaseValues()
    {
        moneyBaseValue = 0;
        sustainabilityBaseValue = 0;
        for (int i = 0; i < itemsSelectables.Length; i++)
        {
<<<<<<< Updated upstream
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice / 10);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability / 10);
            print("checked money value:" + moneyBaseValue);
            print("checked sustainability value:" + sustainabilityBaseValue);
            print("/////////");
=======
            moneyBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemPrice);
            sustainabilityBaseValue += (itemsSelectables[i].GetComponentInChildren<ItemTemplate>().itemSustainability);
>>>>>>> Stashed changes
        }
        controlMoney = moneyMaxValue - moneyBaseValue;
        controlSustainability = sustainabilityMaxValue - sustainabilityBaseValue;
    }

}