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
    public CanvasGroup maskCircleCanvas;

    public Button closeStoreBtn;
    public Button tutorialButton;

    public float tweenDuration = 0.3f;
    public LeanTweenType easeInOut;

    public int textIndex = 0;
    [HideInInspector]
    public bool canContinue = false;
    bool showUI = true;

    public int currentTutorial;
    public LeanPinchCamera leanPinch;
    public LeanDragCamera leanDrag;
    public LeanFingerTap leanTap;

    float storedDragSensivity;

    float storedCameraSize;

    [HideInInspector]
    public Vector3 initialDelta;
    [HideInInspector]
    public Vector3 finalDelta;

    public int numberOfFingers;
    public float checkZoom;

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
        maskCircleCanvas.alpha = 0;

        tutorialCanvasParent.alpha = 1;
        tutorialCanvasParent.blocksRaycasts = true;
        tutorialCanvasParent.interactable = true;

        tutorialCanvas.alpha = 0;
        tutorialCanvas.blocksRaycasts = false;
        tutorialCanvas.interactable = false;


        storedDragSensivity = leanDrag.Sensitivity;

        leanDrag.Use.RequiredFingerCount = 1;
        leanDrag.Sensitivity = 0f;
        leanDrag.enabled = false;
        storedCameraSize = mainCamera.orthographicSize;

        leanPinch.Use.RequiredFingerCount = 2;
        checkZoom = mainCamera.orthographicSize;
        leanPinch.enabled = false;

        leanTap.enabled = false;

        tutorialButton.enabled = false;

        currentTutorial = 0;

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

        // Coroutine startCoroutine = null;
        // startCoroutine = StartCoroutine(NewTutorialCoroutine());

        // StopCoroutine(startCoroutine);

        closeStoreBtn.enabled = true;

        leanDrag.Use.RequiredFingerCount = 1;
        leanDrag.Sensitivity = storedDragSensivity;
        leanDrag.enabled = true;

        leanTap.enabled = true;

        mainCamera.orthographicSize = storedCameraSize;

        leanPinch.enabled = true;

        leanPinch.Use.RequiredFingerCount = 2;

        currentTutorial = textList.Length - 1;
    }

    public void StartTutorial()
    {
        TimerController.instance.tutorialMode = true;

        StartCoroutine(NewTutorialCoroutine());
    }

    IEnumerator NewTutorialCoroutine()
    {
        LeanTouch.OnGesture += HandleGesture;

        if (!TimerController.instance.tutorialMode)
        {
            yield break;
        }

        TutorialBoxShow(true);

        for (int i = 0; i < textList.Length; i++)
        {
            yield return new WaitUntil(() => canContinue == false);
            yield return new WaitForSeconds(delayToShow);

            if (i == 2 || i == 3 || i == 4 || i == 5 || i == 6)
            {
                TutorialBoxShow(true);

                if (i == 2)
                {
                    leanDrag.enabled = false;
                }
                if (i == 3)
                {
                    leanPinch.enabled = false;
                    leanTap.enabled = true;
                }
            }

            if (i == 3)
            {
                leanPinch.enabled = true;

                goToObject();
            }
            else if (i == 4)
            {
                leanTap.enabled = false;
            }
            else if (i == 8)
            {
                GameController.instance.closePanel();
                CameraController.instance.ReturnToBasePosition();
            }

            Debug.Log(textList[i]);
            canContinue = true;
            currentTutorial = i;
            textIndex = i;
            LeanTween.alphaCanvas(textList[i], 1f, tweenDuration).setEase(easeInOut);

            yield return new WaitForSeconds(delaysToShowContinue[i]);

            if (i == 3)
            {
                LeanTween.alphaCanvas(handCanvas, 1f, tweenDuration).setEase(easeInOut);
                LeanTween.alphaCanvas(maskCircleCanvas, 0.6f, tweenDuration).setEase(easeInOut);
            }

            LeanTween.alphaCanvas(continueCanvas, 1f, tweenDuration).setEase(easeInOut).setOnComplete(() => { tutorialButton.enabled = true; });
        }
    }

    public void NewContinueTouch()
    {
        StopCoroutine(NewContinueTouchCoroutine());
        StartCoroutine(NewContinueTouchCoroutine());
    }

    public IEnumerator NewContinueTouchCoroutine()
    {
        if (numberOfFingers == 1)
        {
            Debug.Log("Touched");

            tutorialButton.enabled = false;

            if (textIndex == 1 || textIndex == 2 || textIndex == 3 || textIndex == 5)
            {
                TutorialBoxShow(false);

                LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);

                LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut);

                yield return new WaitForSeconds(tweenDuration);
                yield return new WaitForSeconds(0.25f);

                if (textIndex == 1)
                {
                    leanDrag.Sensitivity = storedDragSensivity;
                    leanDrag.enabled = true;

                    Lean.Touch.LeanTouch.OnFingerTap += HandleFingerTap;
                    Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
                }
                else if (textIndex == 2)
                {
                    leanPinch.enabled = true;

                    Lean.Touch.LeanTouch.OnFingerTap += HandleFingerTap;
                    Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
                }

                yield break;
            }

            if (textIndex == 9)
            {
                SkipTutorial();
                Debug.Log("<size='14'><color='green'><b>Ended Tutorial</b></color></size>");
            }

            Debug.Log("<size='12'><color='red'><b>Continue</b></color></size>");

            LeanTween.alphaCanvas(continueCanvas, 0f, tweenDuration).setEase(easeInOut);

            LeanTween.alphaCanvas(textList[textIndex], 0f, tweenDuration).setEase(easeInOut).setOnComplete(() => { canContinue = false; });
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
    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        if (textIndex == 1)
        {
            getFinalDelta(finger.ScreenPosition);
        }

        if (textIndex == 2)
        {
            checkPinch();
        }
    }

    public void getFinalDelta(Vector3 vector)
    {
        compareDeltas();
    }

    public void compareDeltas()
    {
        if (!tapped)
        {
            if (numberOfFingers == 1)
            {
                Debug.Log("Dragged");

                Lean.Touch.LeanTouch.OnFingerTap -= HandleFingerTap;
                Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;

                leanDrag.Sensitivity = 0;
                leanDrag.Use.RequiredFingerCount = 50;

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
        if (!tapped)
        {
            Debug.Log(mainCamera.orthographicSize);

            if (numberOfFingers == 2 && mainCamera.orthographicSize != checkZoom)
            {
                Debug.Log("Pinched - " + pinched);

                leanPinch.Use.RequiredFingerCount = 50;

                Lean.Touch.LeanTouch.OnFingerTap -= HandleFingerTap;
                Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;

                canContinue = false;
            }
        }
        else
        {
            tapped = false;
        }

        numberOfFingers = 0;
    }

    public void HandleGesture(List<Lean.Touch.LeanFinger> fingers)
    {
        numberOfFingers = fingers.Count;
    }

    public void goToObject()
    {
        float animDuration = tweenDuration * 3;

        LeanTween.move(mainCamera.gameObject.transform.parent.gameObject, goToObjectPosition.position, animDuration).setEase(easeInOut);
        LeanTween.value(mainCamera.gameObject, mainCamera.orthographicSize, 7f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                leanPinch.Zoom = flt;
                Debug.Log(flt);
                Debug.Log(mainCamera.orthographicSize);
            });
    }
}
