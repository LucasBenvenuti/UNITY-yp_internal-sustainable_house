using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemOption : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public Slider itemCostPos;
    public Slider itemCostNeg;
    public Slider itemSusPos;
    public Slider itemSusNeg;
    public Button button;

    public SelectItem selectItem;
}
