using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class OutlineAnimationController : MonoBehaviour
{
    public LeanTweenType easeInOut;
    public float tweenDuration = 1f;

    [HideInInspector]
    public Outliner[] outlineArray;
    public GameObject mainCamera;

    public static float curTimeValue = 0f;
    public static bool isGoing = true;

    void Start()
    {
        outlineArray = mainCamera.GetComponents<Outliner>();

        foreach (Outliner lines in outlineArray)
        {
            LeanTween.value(0.5f, 2, tweenDuration).setEase(easeInOut).setOnUpdate((float flt) =>
                {
                    lines.DilateShift = flt;
                }).setLoopPingPong();
        }

        LeanTween.value(0f, 1f, tweenDuration).setEase(easeInOut).setOnUpdate((float flt) =>
        {
            curTimeValue = flt;

            if (curTimeValue == 1f)
            {
                isGoing = false;
               // Debug.Log(isGoing);
            }
            else if (curTimeValue == 0f)
            {
                isGoing = true;
                //Debug.Log(isGoing);
            }

        }).setLoopPingPong();
    }
}
