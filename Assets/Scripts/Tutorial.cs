using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    public Camera mainCamera;
    public CanvasGroup[] textList;
    public float delayToShow = 0.3f;
    public float[] delaysToShowContinue;
    public CanvasGroup tutorialCanvasParent;
    public CanvasGroup tutorialCanvas;
    public CanvasGroup continueCanvas;
    public CanvasGroup handCanvas;

    public Button closeStoreBtn;

    public float tweenDuration = 0.3f;
    public LeanTweenType easeInOut;

    int textIndex = 0;
    [HideInInspector]
    public bool canContinue = false;
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

    int tutorialCanvasLean;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(this);
        }

        handCanvas.alpha = 0;

        tutorialCanvasParent.alpha = 1;
        tutorialCanvasParent.blocksRaycasts = true;
        tutorialCanvasParent.interactable = true;

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

        closeStoreBtn.enabled = false;
    }

    //REVISION TO MAKE IT WORK
    public void SkipTutorial()
    {
        Debug.Log("SkipTutorial");
        LeanTween.alphaCanvas(tutorialCanvasParent, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() => { tutorialCanvasParent.blocksRaycasts = false; tutorialCanvasParent.interactable = false; tutorialCanvasParent.gameObject.SetActive(false); });

        TutorialBoxShow(false);

        TimerController.instance.tutorialMode = false;

        Coroutine startCoroutine = null;
        startCoroutine = StartCoroutine(TutorialCoroutine());

        StopCoroutine(startCoroutine);

        closeStoreBtn.enabled = true;

        leanDrag.Sensitivity = storedDragSensivity;
        leanDrag.enabled = true;
        mainCamera.orthographicSize = storedCameraSize;

        leanPinch.enabled = true;
        currentTutorial = "final_1";
    }

    public void StartTutorial()
    {
        TimerController.instance.tutorialMode = true;

        StartCoroutine(TutorialCoroutine());
    }

    IEnumerator TutorialCoroutine()
    {
        yield return new WaitUntil(() => canContinue == false);

        // 1 - START

        LeanTouch.OnGesture += HandleGesture;

        yield return new WaitForSeconds(delayToShow);

        if (!TimerController.instance.tutorialMode)
        {
            yield break;
        }

        TutorialBoxShow(true);

        Debug.Log("First TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 2 - DRAG

        yield return new WaitForSeconds(delayToShow);

        Debug.Log("Second TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "drag";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 3 - PINCH

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        Debug.Log("Third TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "pinch";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 4 - INTERACT

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        Debug.Log("Fourth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "interact";

        goToObject();

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        LeanTween.alphaCanvas(handCanvas, 1f, tweenDuration).setEase(easeInOut);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 5 - STORE 0

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        Debug.Log("Fifth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "read";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = true;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 6 - STORE 1

        yield return new WaitForSeconds(delayToShow);

        Debug.Log("Sixth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "choose";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = false;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 7 - STORE 2

        yield return new WaitForSeconds(delayToShow);

        TutorialBoxShow(true);

        Debug.Log("Seventh TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "confirmation";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        showUI = true;
        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 8 - BONUS

        yield return new WaitForSeconds(delayToShow);

        Debug.Log("Eigth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "bonus";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 9 - FINAL 0

        yield return new WaitForSeconds(delayToShow);

        Debug.Log("Nineth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        GameController.instance.closePanel();
        CameraController.instance.ReturnToBasePosition();

        currentTutorial = "final_0";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);

        yield return new WaitUntil(() => canContinue == false);

        // 10 - FINAL 1

        yield return new WaitForSeconds(delayToShow);

        Debug.Log("Tenth TEXT");
        LeanTween.alphaCanvas(textList[textIndex], 1f, tweenDuration).setEase(easeInOut);

        currentTutorial = "final_1";

        yield return new WaitForSeconds(delaysToShowContinue[textIndex]);

        canContinue = true;
        LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut);
    }

    public void ContinueTouch()
    {
        if (canContinue)
        {
            if (!showUI)
            {
                if (currentTutorial != "final_1")
                {
                    TutorialBoxShow(false);
                }

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

                if (currentTutorial == "interact")
                {
                    leanPinch.enabled = false;

                    LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);
                }

                if (currentTutorial == "choose")
                {
                    LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);
                }

            }
            else
            {
                if (currentTutorial == "final_1")
                {
                    // EndTutorial();
                    SkipTutorial();

                    return;
                }

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
            LeanTween.cancel(tutorialCanvasLean);

            tutorialCanvasLean = LeanTween.alphaCanvas(tutorialCanvas, 1f, tweenDuration).setEase(easeInOut).setOnComplete(() => { tutorialCanvas.blocksRaycasts = true; tutorialCanvas.interactable = true; }).id;
        }
        else
        {
            LeanTween.cancel(tutorialCanvasLean);

            tutorialCanvasLean = LeanTween.alphaCanvas(tutorialCanvas, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() => { tutorialCanvas.blocksRaycasts = false; tutorialCanvas.interactable = false; }).id;
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

                // leanPinch.enabled = false;

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
        // leanPinch.enabled = false;

        float animDuration = tweenDuration * 3;

        LeanTween.move(mainCamera.gameObject.transform.parent.gameObject, goToObjectPosition.position, animDuration).setEase(easeInOut);
        LeanTween.value(mainCamera.gameObject, mainCamera.orthographicSize, 7f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                leanPinch.Zoom = flt;
                Debug.Log(flt);
                Debug.Log(mainCamera.orthographicSize);
            });
    }

    public void EndTutorial()
    {
        StopCoroutine(TutorialCoroutine());

        Debug.Log("FIM CONTINUE BUTTON");

        LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut).setOnComplete(() =>
        {
            leanDrag.Sensitivity = storedDragSensivity;
            // leanDrag.enabled = true;
            // leanPinch.enabled = true;

            closeStoreBtn.enabled = true;

            TimerController.instance.tutorialMode = false;
        });

        LeanTween.alphaCanvas(tutorialCanvas, 0f, tweenDuration).setEase(easeInOut).setOnComplete(() => { tutorialCanvas.blocksRaycasts = false; tutorialCanvas.interactable = false; Debug.Log("Ended Tutorial fade out!"); });
    }
}
