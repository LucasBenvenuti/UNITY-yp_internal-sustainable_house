using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ActionsAnimations : MonoBehaviour
{
    public static ActionsAnimations instance;
    Animator animatorTemplate;
    Animator animatorWaterKitchen;
    Animator animatorWaterLaundry;
    public NavMeshAgent myAgent;
    public Transform finalPosition;
    public Transform finalPositionBathroom;
    public Transform finalCellphoneBathroom;
    public Transform basePosition;

    public GameObject cellPhone;
    public GameObject chargingCellPhone;
    public GameObject charger;
    public GameObject toothbrush;
    public GameObject sinkToothbrush;
    public GameObject book;
    public GameObject plate;

    public GameObject waterKitchen;
    public GameObject waterLaundry;

    int laundryCounter;
    int kitchenCounter;
    int bathroomCounter;

    bool tookBrush = false;
    bool readingControl = false;
    bool canEndCoroutine = false;

    void Awake()
    {
        if (cellPhone && chargingCellPhone && charger)
        {
            cellPhone.SetActive(false);
            charger.SetActive(false);
        }
        if (toothbrush && sinkToothbrush)
        {
            toothbrush.SetActive(false);
            sinkToothbrush.SetActive(true);

        }
        if (book)
        {
            book.SetActive(false);
        }
        if (waterKitchen)
        {
            animatorWaterKitchen = waterKitchen.GetComponent<Animator>();
            kitchenCounter = 0;
        }
        if (waterLaundry)
        {
            animatorWaterLaundry = waterLaundry.GetComponent<Animator>();
            laundryCounter = 0;
        }
    }

    void Start()
    {
        animatorTemplate = gameObject.GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
        if (book)
        {
            if (DataStorage.instance.actionsDone[2])
            {
                GameController.instance.tvOn = false;
                animatorTemplate.SetTrigger("ReadTrigger");
            }
        }
    }


    public void CallFirstCoroutine(int index)
    {
        if (index == 0)
        {
            StartCoroutine(DoWashingAnimation());
        }
        if (index == 1)
        {
            StartCoroutine(DoTextingAnimation());
        }
        if (index == 2)
        {
            StartCoroutine(DoReadingAnimation());
        }
        if (index == 3)
        {
            StartCoroutine(DoWashingAnimation());
        }
        if (index == 4)
        {
            StartCoroutine(DoBrushingAnimation());
        }
    }

    IEnumerator IdleTransition()
    {
        animatorTemplate.SetTrigger("ReturnToIdleTrigger");

        float newDelay = 1f;

        if (toothbrush)
        {
            if (tookBrush)
            {
                newDelay = 2f;
            }
        }

        yield return new WaitForSeconds(newDelay);
    }
    IEnumerator DoTextingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        if (Vector3.Distance(transform.position, myAgent.destination) >= 0.5f)
        {
            myAgent.transform.LeanRotateY(0f, 0.5f);
        }
        yield return IdleTransition();
        animatorTemplate.SetTrigger("TextTrigger");
        yield return new WaitForSeconds(6f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        if (TimerController.instance.inGame)
        {
            CameraController.instance.ReturnToBasePosition();
        }

    }
    IEnumerator DoReadingAnimation()
    {
        GameController.instance.tvOn = false;
        animatorTemplate.SetTrigger("ReadTrigger");
        yield return new WaitForSeconds(5f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        if (TimerController.instance.inGame)
        {
            CameraController.instance.ReturnToBasePosition();
        }
    }

    IEnumerator DoBrushingAnimation()
    {
        cellPhone.SetActive(false);
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPositionBathroom.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        yield return IdleTransition();
        animatorTemplate.SetTrigger("BrushTrigger");
        yield return new WaitForSeconds(7f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        if (TimerController.instance.inGame)
        {
            CameraController.instance.ReturnToBasePosition();
        }

        yield return new WaitForSeconds(5f);

        Debug.Log("Idle Transition");

        yield return IdleTransition();

        myAgent.SetDestination(basePosition.position);
        animatorTemplate.SetTrigger("WalkTrigger");
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }

        animatorTemplate.SetTrigger("ReturnToIdleTrigger");
    }
    IEnumerator DoWashingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 0.8f)
        {
            yield return null;
        }
        if (Vector3.Distance(transform.position, myAgent.destination) >= 0.5f)
        {
            if (waterLaundry)
            {
                myAgent.transform.LeanRotateY(90f, 0.4f);
            }
            else
            {
                myAgent.transform.LeanRotateY(90f, 0.3f);
            }
        }
        yield return IdleTransition();
        animatorTemplate.SetTrigger("WashTrigger");
        yield return new WaitForSeconds(5f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        if (TimerController.instance.inGame)
        {
            CameraController.instance.ReturnToBasePosition();
        }

        if (TimerController.instance.tutorialMode)
        {
            yield return CameraController.instance.ReturnToBasePositionNew();

            Tutorial.instance.canContinue = false;

            Tutorial.instance.TutorialBoxShow(true);
        }
    }

    public void CallEndCoroutine()
    {
        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        yield return IdleTransition();
        myAgent.SetDestination(basePosition.position);
        animatorTemplate.SetTrigger("WalkTrigger");
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        animatorTemplate.SetTrigger("ReturnToIdleTrigger");
    }

    public void GrabCellPhone(int active)
    {
        if (active == 1)
        {
            chargingCellPhone.SetActive(false);
            cellPhone.SetActive(true);
            //myAgent.transform.LeanRotateY(180f, 0.5f);
        }
        else
        {
            cellPhone.SetActive(false);
        }
    }

    public void GrabToothbrush(int active)
    {
        if (active == 1 && !tookBrush)
        {
            sinkToothbrush.SetActive(false);
            toothbrush.SetActive(true);

            tookBrush = true;
        }
        else
        {
            toothbrush.SetActive(false);
            sinkToothbrush.SetActive(true);
        }
    }

    public void ReadBook(int active)
    {
        if (active == 1 && !readingControl)
        {
            Debug.Log("entrou");
            book.SetActive(true);
            readingControl = true;
        }
    }

    public void Wash(int active)
    {
        if (active == 1)
        {
            if (waterKitchen)
            {
                if (kitchenCounter == 0)
                {
                    animatorWaterKitchen.SetTrigger("FillSinkTrigger");
                }
            }
            else if (waterLaundry)
            {
                if (laundryCounter == 0)
                {
                    animatorWaterLaundry.SetTrigger("FillTrigger");
                }
            }
        }
        else
        {
            if (waterKitchen)
            {
                kitchenCounter++;
                if (kitchenCounter >= 2)
                {
                    animatorWaterKitchen.SetTrigger("EmptySinkTrigger");
                    StartCoroutine(EndCoroutine());
                }
            }
            else if (waterLaundry)
            {
                laundryCounter++;
                if (laundryCounter >= 2)
                {
                    animatorWaterLaundry.SetTrigger("EmptyTrigger");
                    StartCoroutine(EndCoroutine());
                }
            }
        }
    }

    public void CallRandomIdle()
    {
        int i = 0;
        i = Random.Range(0, 10);

        StartCoroutine(WaitRandom());

        if (i > 5 && i < 8)
        {
            animatorTemplate.SetTrigger("Idle_2");
        }
        else if (i >= 8)
        {
            animatorTemplate.SetTrigger("Idle_3");
        }
    }

    IEnumerator WaitRandom()
    {
        float delay = 0f;

        delay = Random.Range(0f, 3f);

        yield return new WaitForSeconds(delay);

    }

    public void PlaceCharger(int active)
    {
        if (active == 1)
        {
            charger.SetActive(true);
        }
    }

    public void CallFinalCellphoneAnimation()
    {
        StartCoroutine(FinalCellphoneAnimation());
    }

    IEnumerator FinalCellphoneAnimation()
    {
        if (cellPhone)
        {
            animatorTemplate.SetTrigger("WalkTrigger");
            myAgent.SetDestination(finalCellphoneBathroom.position);
            while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
            {
                yield return null;
            }
            if (Vector3.Distance(transform.position, myAgent.destination) >= 0.5f)
            {
                myAgent.transform.LeanRotateY(-30f, 0.5f);
            }
            yield return IdleTransition();
            animatorTemplate.SetTrigger("FinalCellphoneTrigger");
        }
    }

    public void CallRotateCharacter()
    {
        myAgent.transform.LeanRotateY(-20f, 0.5f);
        animatorTemplate.SetTrigger("PickingTrigger");
    }

}




