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
    public Transform basePosition;

    public GameObject cellPhone;
    public GameObject chargingCellPhone;
    public GameObject toothbrush;
    public GameObject sinkToothbrush;
    public GameObject book;
    public GameObject plate;

    public GameObject waterKitchen;
    public GameObject waterLaundry;

    int laundryCounter;
    int kitchenCounter;

    // private void Awake()
    // {
    //     if (!instance)
    //     {
    //         instance = this;
    //     }
    //     if (instance != this)
    //     {
    //         Destroy(this);
    //     }
    // }

    void Awake()
    {
        if (cellPhone && chargingCellPhone)
        {
            cellPhone.SetActive(false);
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
            // plate.SetActive(false);
            //bubbleParticles.SetActive(false);
            animatorWaterKitchen = waterKitchen.GetComponent<Animator>();
            // waterKitchen.SetActive(false);
            kitchenCounter = 0;
        }
        if (waterLaundry)
        {
            //cloth.SetActive(false);
            animatorWaterLaundry = waterLaundry.GetComponent<Animator>();
            laundryCounter = 0;
        }
    }

    void Start()
    {
        animatorTemplate = gameObject.GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
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
        yield return new WaitForSeconds(0.8f);
    }
    IEnumerator DoTextingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        //myAgent.transform.LeanRotateY(-50f, 0.5f);
        yield return IdleTransition();
        animatorTemplate.SetTrigger("TextTrigger");
        yield return new WaitForSeconds(6f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        CameraController.instance.ReturnToBasePosition();

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

        CameraController.instance.ReturnToBasePosition();

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

        CameraController.instance.ReturnToBasePosition();

    }
    IEnumerator DoWashingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        myAgent.transform.LeanRotateY(90f, 0.7f);
        yield return IdleTransition();
        animatorTemplate.SetTrigger("WashTrigger");
        yield return new WaitForSeconds(5f);

        //RETORNO AGORA
        GameController.instance.actionCanvas.interactable = false;
        GameController.instance.actionCanvas.blocksRaycasts = false;
        LeanTween.alphaCanvas(GameController.instance.actionCanvas, 0f, GameController.instance.actionTweenDuration).setEase(GameController.instance.actionEaseInOut);

        CameraController.instance.ReturnToBasePosition();
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
            myAgent.transform.LeanRotateY(180f, 1f);

        }
        else
        {
            cellPhone.SetActive(false);
        }
    }

    public void GrabToothbrush(int active)
    {
        if (active == 1)
        {
            sinkToothbrush.SetActive(false);
            toothbrush.SetActive(true);
        }
        else
        {
            toothbrush.SetActive(false);
        }
    }

    public void ReadBook(int active)
    {

        if (active == 1)
        {
            Debug.Log("entrou");
            book.SetActive(true);
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

}




