using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionsAnimations : MonoBehaviour
{
    public static ActionsAnimations instance;
    Animator animatorTemplate;
    public NavMeshAgent myAgent;
    public Transform finalPosition;
    public Transform finalPositionBathroom;
    public Transform basePosition;

    public GameObject cellPhone;
    public GameObject toothbrush;
    public GameObject book;
    public GameObject plate;
    public GameObject cloth;
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
        if (cellPhone)
        {
            cellPhone.SetActive(false);
        }
        if (toothbrush)
        {
            toothbrush.SetActive(false);
        }
        if (book)
        {
            book.SetActive(false);
        }
        if (plate)
        {
            plate.SetActive(false);
            kitchenCounter = 0;
        }
        if (cloth)
        {
            cloth.SetActive(false);
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
    IEnumerator DoTextingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        myAgent.transform.LeanRotateY(180f, 1f);
        animatorTemplate.SetTrigger("TextTrigger");
        yield return new WaitForSeconds(6f);
        CameraController.instance.ReturnToBasePosition();

    }
    IEnumerator DoReadingAnimation()
    {
        GameController.instance.tvOn = false;
        animatorTemplate.SetTrigger("ReadTrigger");
        yield return new WaitForSeconds(5f);
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
        animatorTemplate.SetTrigger("BrushTrigger");
        yield return new WaitForSeconds(7f);
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
        animatorTemplate.SetTrigger("WashTrigger");
        yield return new WaitForSeconds(5f);
        CameraController.instance.ReturnToBasePosition();
    }

    public void CallEndCoroutine()
    {
        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        myAgent.SetDestination(basePosition.position);
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
            cellPhone.SetActive(true);
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
            // if (loopCounter == 0)
            // {
            Debug.Log("entrou");
            book.SetActive(true);
            //}
        }
        // else(active == 0)
        // {

        //     Debug.Log("loop warning");
        //     loopCounter++;
        //     Debug.Log("loop value:" + loopCounter);
        // }
        // else
        // {
        //     if (loopCounter >= 3)
        //     {
        //         book.SetActive(false);
        //     }
        // }

    }
    public void Wash(int active)
    {
        if (active == 1)
        {
            if (plate)
            {

                plate.SetActive(true);
            }
            else if (cloth)
            {

                cloth.SetActive(true);
            }
        }
        else if (active == 2)
        {
            if (plate)
            {

                plate.SetActive(false);
            }
            else if (cloth)
            {

                cloth.SetActive(false);
            }
        }
        else
        {
            if (plate)
            {

                kitchenCounter++;
                if (kitchenCounter >= 2)
                {
                    StartCoroutine(EndCoroutine());
                }
            }
            else if (cloth)
            {
                laundryCounter++;
                if (laundryCounter >= 2)
                {
                    StartCoroutine(EndCoroutine());
                }
            }
        }
    }
}




