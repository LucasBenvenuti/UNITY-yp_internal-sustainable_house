using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public Animator moneyHelperAnimator;
    public TMP_Text moneyHelperText;
    public GameObject[] moneyHelperIcon;

    public Animator sustainabilityHelperAnimator;
    public TMP_Text sustainabilityHelperText;
    public GameObject[] sustainabilityHelperIcon;

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

        DataStorage.instance.addReportLine("Nome de usuário: " + DataStorage.instance.userName + ".");

        DataStorage.instance.addReportLine("Jogo iniciado.");
        DataStorage.instance.addReportLine("Recursos iniciais: " + moneyBaseValue + ".");
        DataStorage.instance.addReportLine("Sustentabilidade inicial: " + sustainabilityBaseValue + ".");

        DataStorage.instance.startSus = sustainabilityBaseValue;
        DataStorage.instance.startResources = moneyBaseValue;
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

        if (newPrice >= 0)
        {
            moneyHelperText.text = "+" + newPrice.ToString();
            moneyHelperText.color = new Color32(44, 201, 130, 255);
            moneyHelperIcon[0].SetActive(true);
            moneyHelperIcon[1].SetActive(false);
            moneyHelperAnimator.SetTrigger("IncreaseMoney");
        }
        else
        {
            moneyHelperText.text = newPrice.ToString();
            moneyHelperText.color = new Color32(227, 36, 98, 255);
            moneyHelperIcon[1].SetActive(true);
            moneyHelperIcon[0].SetActive(false);
            moneyHelperAnimator.SetTrigger("DecreaseMoney");
        }
        if (newSus >= 0)
        {
            sustainabilityHelperText.text = "+" + newSus.ToString();
            sustainabilityHelperText.color = new Color32(178, 108, 215, 255);
            sustainabilityHelperIcon[0].SetActive(true);
            sustainabilityHelperIcon[1].SetActive(false);
            sustainabilityHelperAnimator.SetTrigger("IncreaseSustainability");
        }
        else
        {
            sustainabilityHelperText.text = newSus.ToString();
            sustainabilityHelperText.color = new Color32(227, 36, 98, 255);
            sustainabilityHelperIcon[1].SetActive(true);
            sustainabilityHelperIcon[0].SetActive(false);
            sustainabilityHelperAnimator.SetTrigger("DecreaseSustainability");
        }

        DataStorage.instance.addReportLine("Recursos anteriores: " + controlMoney + ". Novos Recursos: " + newMoneyValue + ".");
        DataStorage.instance.addReportLine("Sustentabilidade anterior: " + controlSustainability + ". Nova Sustentabilidade: " + newSusValue + ".");

        sustainabilitySlider.value = newSusValue;
        moneySlider.value = newMoneyValue;
    }
}
