using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAnimations : MonoBehaviour
{
    [SerializeField] Vector3[] dogListPositions;
    int curDestination;
    private static System.Random rng = new System.Random();
    public float randomIdleTime;
    public bool isCloseToPosition;
    public bool firstDestinationSet;
    public NavMeshAgent dogAgent;
    public Animator dogAnimator;
    void Start()
    {
        RandomizeRoomList();
        curDestination = Random.Range(0, dogListPositions.Length);
        dogAnimator = gameObject.GetComponent<Animator>();
        dogAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!TimerController.instance.tutorialMode)
        {
            if (!firstDestinationSet)
            {
                dogAgent.SetDestination(dogListPositions[curDestination]);
                dogAnimator.SetTrigger("DogWalkTrigger");
                dogAgent.stoppingDistance = 1;
                firstDestinationSet = true;
            }
            CheckNextDestination();
        }

    }

    void CheckNextDestination()
    {
        if (!isCloseToPosition)
        {
            if (Vector3.Distance(transform.position, dogAgent.destination) < 2f)
            {
                curDestination++;
                isCloseToPosition = true;
                dogAgent.isStopped = true;
                dogAnimator.SetTrigger("DogIdleTrigger");
                if (curDestination >= dogListPositions.Length)
                {
                    RandomizeRoomList();
                    curDestination = 0;
                }
            }
        }
        else
        {
            randomIdleTime -= randomIdleTime * Time.deltaTime;
            if (randomIdleTime <= 0.1f)
            {
                dogAgent.isStopped = false;
                dogAnimator.SetTrigger("DogWalkTrigger");
                dogAgent.SetDestination(dogListPositions[curDestination]);
                dogAgent.stoppingDistance = 1;
                isCloseToPosition = false;
                randomIdleTime = Random.Range(2f, 5f);
            }
        }
    }
    public static void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    void RandomizeRoomList()
    {
        System.Random rdm = new System.Random();
        List<Vector3> listRandom = new List<Vector3>(dogListPositions);
        Shuffle<Vector3>(ref listRandom);
        dogListPositions = listRandom.ToArray();
    }

}
