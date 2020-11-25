using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class OutlineAnimationController : MonoBehaviour
{
    public LeanTweenType easeInOut;
    public float tweenDuration = 1f;

    public Outliner[] outlineArray;
    public GameObject mainCamera;

    void Start()
    {
        outlineArray = mainCamera.GetComponents<Outliner>();

        foreach (Outliner lines in outlineArray)
        {
            LeanTween.value(this.gameObject, 0.5f, 2, tweenDuration).setEase(easeInOut).setOnUpdate((float flt) =>
                {
                    lines.DilateShift = flt;
                }).setLoopPingPong();
        }
    }
}
