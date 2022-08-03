using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : MonoBehaviour
{
    /*item type -> 0: climatização
                   1: secagem de roupa
                   2: lampada
                   3: janelas
                   4: abastecimento de energia eletrica
                   5: abastecimento de agua
                   6: esgotamento sanitario
                   7: chuveiro
                   8: vaso sanitario
                   9: torneira de pia
                   10: geladeira
                   11: lixeiras
                   12: maquina de lavar roupa
                   13: agua de reuso
                   14: televisão
                   15: telhado               
     */
    public bool isSelectable;
    public int itemOption;

    //USED ON REPORT LINE
    public string itemName;
    public int itemType;
    public float itemPrice;
    public float itemSustainability;
    public float basePrice;
    public float baseSustainability;
    public Sprite itemSprite;
    public GameObject itemPrefab;

    //USE TO GET A STRING TO PUT AS TITLE OF UI
    public string categoryName;

    public float zoomSize = 5f;

    public ParticleSystem particleSystem;

    public bool hasChanged = true;

    public LeanTweenType materialEaseInOut;
    public float materialTweenDuration = 1f;

    public Color startMatColor;
    public Renderer[] objectMat;

    private Color firstTweenColor;
    private Color firstBaseColor;

    void Start()
    {
        if (!isSelectable)
        {
            isSelectable = true;
        }
        if (itemName == null)
        {
            itemName = "Default";
        }
        if (itemType < 0)
        {
            itemType = 0;
        }

        if (itemOption < 0)
        {
            itemOption = 0;
        }

        if (particleSystem)
        {
            particleSystem.Play();
        }

        float waitTime = 0f;

        if (OutlineAnimationController.isGoing)
        {
            waitTime = (1f - OutlineAnimationController.curTimeValue) + 1f;
        }
        else
        {
            waitTime = (OutlineAnimationController.curTimeValue);
        }

        foreach (Renderer render in objectMat)
        {
            LeanTween.value(this.gameObject, 0f, 1f, waitTime).setEase(materialEaseInOut).setOnComplete(() =>
            {
                LeanTween.value(this.gameObject, new Color(1f, 1f, 1f, 1f), startMatColor, materialTweenDuration).setEase(materialEaseInOut).setOnUpdate((Color flt) =>
                {
                    render.material.color = flt;
                }).setLoopPingPong();
            });
        }
    }
}

