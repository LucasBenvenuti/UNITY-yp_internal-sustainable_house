using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainRegister_Manager : MonoBehaviour
{
    public CanvasGroup registerScreen;
    public CanvasGroup playScreen;

    public LeanTweenType easeInOut;
    public float tweenDuration = 0.2f;
    public float tweenTransitionOffset = 1f;

    void Awake()
    {
        playScreen.alpha = 0f;
    }

    public void RegisterScreenShow()
    {
        LeanTween.alphaCanvas(registerScreen, 1f, tweenDuration).setEase(easeInOut);
    }
    public void RegisterScreenHide()
    {
        LeanTween.alphaCanvas(registerScreen, 0f, tweenDuration).setEase(easeInOut);
    }
    public void PlayScreenShow()
    {
        LeanTween.alphaCanvas(playScreen, 1f, tweenDuration).setEase(easeInOut);
    }
    public void PlayScreenHide()
    {
        LeanTween.alphaCanvas(playScreen, 0f, tweenDuration).setEase(easeInOut);
    }

    public void RegisterToPlay()
    {
        StopCoroutine(RegisterToPlayCoroutine());
        StopCoroutine(PlayToRegisterCoroutine());

        StartCoroutine(RegisterToPlayCoroutine());
    }

    public void PlayToRegister()
    {
        StopCoroutine(RegisterToPlayCoroutine());
        StopCoroutine(PlayToRegisterCoroutine());

        StartCoroutine(PlayToRegisterCoroutine());
    }

    IEnumerator RegisterToPlayCoroutine()
    {
        playScreen.gameObject.SetActive(true);

        RegisterScreenHide();

        float resultFloat = tweenDuration / tweenTransitionOffset;

        yield return new WaitForSeconds(resultFloat);

        PlayScreenShow();

        yield return new WaitForSeconds(tweenDuration - resultFloat);

        registerScreen.gameObject.SetActive(false);
    }

    IEnumerator PlayToRegisterCoroutine()
    {
        registerScreen.gameObject.SetActive(true);

        PlayScreenHide();

        float resultFloat = tweenDuration / tweenTransitionOffset;

        yield return new WaitForSeconds(resultFloat);

        RegisterScreenShow();

        yield return new WaitForSeconds(tweenDuration - resultFloat);

        playScreen.gameObject.SetActive(false);
    }

}
