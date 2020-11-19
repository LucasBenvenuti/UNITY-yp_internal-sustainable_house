using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public CanvasGroup[] textList;
    public float delayToShow = 0.3f;
    public float[] delaysToShowContinue;
    public CanvasGroup tutorialCanvas;
    public CanvasGroup continueCanvas;
    public Animator handIcon;

    public float tweenDuration = 0.3f;
    public LeanTweenType easeInOut;

    int textIndex = 0;
    bool canContinue = false;

    bool showUI = true;

    public string currentTutorial;
    public LeanPinchCamera leanPinch;
    public LeanDragCamera leanDrag;

    float storedDragSensivity;

    void Awake()
    {
        tutorialCanvas.alpha = 0;
        tutorialCanvas.blocksRaycasts = false;
        tutorialCanvas.interactable = false;

        storedDragSensivity = leanDrag.Sensitivity;

        leanDrag.Sensitivity = 0f;
        leanDrag.enabled = false;

        leanPinch.enabled = false;

        currentTutorial = "start";
    }

    public void StartTutorial()
    {
        StartCoroutine(TutorialCoroutine());
    }

    IEnumerator TutorialCoroutine()
    {
        yield return new WaitUntil(() => canContinue == false);

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        //START TEXT
        Debug.Log("First TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        yield return new WaitForSeconds(delayToShow);

        //Second TEXT
        Debug.Log("Second TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "drag";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);
    }

    public void ContinueTouch()
    {
        if (canContinue)
        {
            if (!showUI)
            {
                TutorialBoxShow(false);

                if (currentTutorial == "drag")
                {
                    leanDrag.Sensitivity = storedDragSensivity;
                    leanDrag.enabled = true;
                }

                if (currentTutorial == "pinch")
                {
                    leanPinch.enabled = true;
                }

                if (currentTutorial == "ended")
                {
                    leanDrag.Sensitivity = storedDragSensivity;
                    leanDrag.enabled = true;
                    leanPinch.enabled = true;
                }

            }

            Debug.Log("Continue Touched");
            canContinue = false;

            LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);

            LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);

            textIndex++;
        }

    }

    public void TutorialBoxShow(bool show)
    {
        if (show)
        {
            LeanTween.alphaCanvas(tutorialCanvas, 1f, tweenDuration).setOnComplete(() => { tutorialCanvas.blocksRaycasts = true; tutorialCanvas.interactable = true; });
        }
        else
        {
            LeanTween.alphaCanvas(tutorialCanvas, 0f, tweenDuration).setOnComplete(() => { tutorialCanvas.blocksRaycasts = false; tutorialCanvas.interactable = false; });
        }
    }

    // void Update()
    // {
    //     if (TimerController.instance.tutorialMode)
    //     {
    //         Lean.Touch.LeanTouch.OnFingerTap += HandleFingerTap;
    //     }
    // }
    public void testeDrag()
    {
        Debug.Log("TESTE DRAG");
    }
}
