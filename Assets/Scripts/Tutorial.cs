using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Camera mainCamera;
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
    float storedCameraSize;

    [HideInInspector]
    public Vector3 initialDelta;
    [HideInInspector]
    public Vector3 finalDelta;

    public int numberOfFingers;

    bool tapped = false;
    bool pinched = false;

    public Transform goToObjectPosition;

    void Awake()
    {
        tutorialCanvas.alpha = 0;
        tutorialCanvas.blocksRaycasts = false;
        tutorialCanvas.interactable = false;

        storedDragSensivity = leanDrag.Sensitivity;

        leanDrag.Sensitivity = 0f;
        leanDrag.enabled = false;
        storedCameraSize = mainCamera.orthographicSize;

        leanPinch.enabled = false;

        currentTutorial = "start";

        numberOfFingers = 0;
    }

    public void StartTutorial()
    {
        StartCoroutine(TutorialCoroutine());
    }

    IEnumerator TutorialCoroutine()
    {
        yield return new WaitUntil(() => canContinue == false);

        // 1

        LeanTouch.OnGesture += HandleGesture;

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        //START TEXT
        Debug.Log("First TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 2

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

        // 3

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        //Third TEXT
        Debug.Log("Third TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "pinch";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        //FOURTH TEXT
        Debug.Log("Fourth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "select";

        goToObject();

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

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

                    Lean.Touch.LeanTouch.OnFingerTap += HandleFingerTap;
                    Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
                    Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;

                    LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);
                    LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);
                }

                if (currentTutorial == "pinch")
                {
                    leanDrag.Sensitivity = 0;
                    leanDrag.enabled = false;

                    leanPinch.enabled = true;

                    LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);
                    LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);
                }

                if (currentTutorial == "ended")
                {
                    leanDrag.Sensitivity = storedDragSensivity;
                    leanDrag.enabled = true;
                    leanPinch.enabled = true;
                }

            }
            else
            {
                Debug.Log("Continue Touched");
                canContinue = false;

                LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);

                LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);
            }

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

    void HandleFingerTap(Lean.Touch.LeanFinger finger)
    {
        tapped = true;
    }

    void HandleFingerDown(Lean.Touch.LeanFinger finger)
    {
        getInitialDelta(finger.ScreenPosition);
    }
    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        getFinalDelta(finger.ScreenPosition);

        Debug.Log(pinched);

        checkPinch();
    }

    public void getInitialDelta(Vector3 vector)
    {
        if (currentTutorial == "drag")
        {
            initialDelta = vector;
        }
    }

    public void getFinalDelta(Vector3 vector)
    {
        if (currentTutorial == "drag")
        {
            finalDelta = vector;

            compareDeltas();
        }
    }

    public void compareDeltas()
    {
        if (!tapped)
        {
            if (numberOfFingers == 1)
            {
                Debug.Log("Dragged");

                Lean.Touch.LeanTouch.OnFingerTap -= HandleFingerTap;
                Lean.Touch.LeanTouch.OnFingerDown -= HandleFingerDown;
                // Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;

                canContinue = false;
            }

            numberOfFingers = 0;

        }
        else
        {
            tapped = false;
        }
    }

    public void checkPinch()
    {
        if (currentTutorial == "pinch")
        {
            if (numberOfFingers == 2 && pinched)
            {
                Debug.Log("Pinched");

                Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
                LeanTouch.OnGesture -= HandleGesture;

                leanPinch.enabled = false;

                canContinue = false;
            }

            numberOfFingers = 0;
        }
    }

    public void HandleGesture(List<Lean.Touch.LeanFinger> fingers)
    {
        numberOfFingers = fingers.Count;

        if (currentTutorial == "pinch")
        {
            if (numberOfFingers == 2 && LeanGesture.GetPinchScale(fingers) != 1)
            {
                pinched = true;
            }
        }
    }

    public void goToObject()
    {
        LeanTween.move(mainCamera.gameObject, goToObjectPosition.position, tweenDuration).setEase(easeInOut);
        LeanTween.value(mainCamera.gameObject, mainCamera.orthographicSize, storedCameraSize, tweenDuration).setEase(easeInOut);
    }
}
