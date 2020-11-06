using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionsAnimations : MonoBehaviour
{
    public static ActionsAnimations instance;
     Animator washingAnimator;
    public NavMeshAgent myAgent;
    public Transform washingPosition;
    public Transform basePosition;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        washingAnimator = gameObject.GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CallFirstCoroutine()
    {
        StartCoroutine(DoWashingAnimation());
    }
    IEnumerator DoWashingAnimation()
    {
        washingAnimator.SetTrigger("WalkTrigger");
        myAgent.SetDestination(new Vector3(washingPosition.position.x, 0.135f, washingPosition.position.z));
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        washingAnimator.SetTrigger("WashTrigger");

    }

    public void EndFunction()
    {
        StartCoroutine(CallEndCoroutine());
    }

    IEnumerator CallEndCoroutine()
    {
        myAgent.SetDestination(basePosition.position);
        while (Vector3.Distance(transform.position, myAgent.destination) >= 1f)
        {
            yield return null;
        }
        washingAnimator.SetTrigger("ReturnToIdleTrigger");
    }
}



