using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainRegister_Manager : MonoBehaviour
{
    public CanvasGroup registerScreen;
    public CanvasGroup playScreen;
    public CanvasGroup confirmScreen;

    public LeanTweenType easeInOut;
    public float tweenDuration = 0.2f;
    public float tweenTransitionOffset = 1f;

    public PDF_Generator pdfGenerator;

    public bool gameEnded = false;

    void Awake()
    {
        if (DataStorage.instance.hasProgress)
        {
            registerScreen.alpha = 0f;
        }
        else
        {
            playScreen.alpha = 0f;
        }

        confirmScreen.alpha = 0;
        confirmScreen.blocksRaycasts = false;
        confirmScreen.interactable = false;

    }

    void Start()
    {
        if (DataStorage.instance.gameFinished)
        {
            DataStorage.instance.DeleteAllData();
        }
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
        if (DataStorage.instance.hasProgress)
        {
            ShowConfirmScreen();

            yield break;
        }

        registerScreen.gameObject.SetActive(true);

        PlayScreenHide();

        float resultFloat = tweenDuration / tweenTransitionOffset;

        yield return new WaitForSeconds(resultFloat);

        RegisterScreenShow();

        yield return new WaitForSeconds(tweenDuration - resultFloat);

        playScreen.gameObject.SetActive(false);
    }

    public void ConfirmDeleteProgress()
    {
        gameEnded = true;

        //GENERATE REPORT BEFORE DELETE ALL DATA
        DataStorage.instance.addReportLine("Fim de Jogo.");
        DataStorage.instance.addReportLine("Recursos Finais: " + DataStorage.instance.currentMoney + ".");
        DataStorage.instance.addReportLine("Sustentabilidade Final: " + DataStorage.instance.currentSustainability + ".");
        pdfGenerator.GeneratePDF();
        // DataStorage.instance.DeleteAllData();

        // HideConfirmScreen();
        // PlayToRegister();
    }

    public void ShowConfirmScreen()
    {
        LeanTween.alphaCanvas(confirmScreen, 1f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            confirmScreen.blocksRaycasts = true;
            confirmScreen.interactable = true;
        });
    }

    public void HideConfirmScreen()
    {
        LeanTween.alphaCanvas(confirmScreen, 0f, tweenDuration).setEase(easeInOut).setOnStart(() =>
        {
            confirmScreen.blocksRaycasts = false;
            confirmScreen.interactable = false;
        });
    }
}
