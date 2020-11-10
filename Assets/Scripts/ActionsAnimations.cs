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
            StartCoroutine(DoTextingAnimation());
        }
        if (index == 1)
        {
            DoReadingAnimation();
        }
        if (index == 2)
        {
            StartCoroutine(DoBrushingAnimation());
        }
        if (index == 3)
        {
            StartCoroutine(DoWashingAnimation());
        }
        if (index == 4)
        {
            StartCoroutine(DoWashingAnimation());
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
        animatorTemplate.SetTrigger("TextTrigger");

    }
    void DoReadingAnimation()
    {
        animatorTemplate.SetTrigger("ReadTrigger");

    }


    IEnumerator DoBrushingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPositionBathroom.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        animatorTemplate.SetTrigger("BrushTrigger");

    }
    IEnumerator DoWashingAnimation()
    {
        animatorTemplate.SetTrigger("WalkTrigger");
        myAgent.SetDestination(finalPosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        animatorTemplate.SetTrigger("WashTrigger");

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
}



