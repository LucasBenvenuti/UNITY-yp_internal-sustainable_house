using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Screen : MonoBehaviour
{
    public Material screenMat;
    public Texture2D[] imagesList;
    public float imageDuration = 4f;
    int currentIndex;

    // int teste1 = -8;
    // int teste2 = 1;
    // int teste3 = 2;
    // int teste4 = 3;

    // valorTotal = teste1 + teste2 + teste3 + teste4

    void Start()
    {
        currentIndex = 0;
        screenMat.mainTexture = imagesList[currentIndex];

        StartCoroutine(changeMatSprite());
    }

    IEnumerator changeMatSprite()
    {
        while (true)
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

            screenMat.mainTexture = imagesList[currentIndex];
        }
    }
}
