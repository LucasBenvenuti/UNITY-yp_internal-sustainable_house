using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Screen : MonoBehaviour
{
    public Material screenMat;
    public Texture2D[] imagesList;
    public Texture2D blackScreenTexture;
    public float imageDuration = 4f;
    int currentIndex;

    bool alreadyAnimated;

    // int teste1 = -8;
    // int teste2 = 1;
    // int teste3 = 2;
    // int teste4 = 3;

    // valorTotal = teste1 + teste2 + teste3 + teste4

    void Start()
    {
        alreadyAnimated = false;

        currentIndex = 0;
        screenMat.mainTexture = imagesList[currentIndex];
        screenMat.color = new Color(1f, 1f, 1f, 1f);

        StartCoroutine(changeMatSprite());
    }

    void Update()
    {
        if (!GameController.instance.tvOn)
        {
            if (!alreadyAnimated)
            {
                alreadyAnimated = true;

                StopCoroutine(changeMatSprite());

                ScreenOff();
            }
        }
    }

    IEnumerator changeMatSprite()
    {
        while (GameController.instance.tvOn)
        {
            yield return new WaitForSeconds(imageDuration);

            if (currentIndex >= (imagesList.Length - 1))
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            if (!alreadyAnimated)
            {
                screenMat.mainTexture = imagesList[currentIndex];
            }
        }
    }

    public void ScreenOff()
    {
        screenMat.mainTexture = blackScreenTexture;
    }
}
