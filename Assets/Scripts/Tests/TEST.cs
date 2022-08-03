using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    int numberOfFingers = 0;

    // Start is called before the first frame update
    void Start()
    {
        LeanTouch.OnGesture += HandleGesture;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleGesture(List<Lean.Touch.LeanFinger> fingers)
    {
        numberOfFingers = fingers.Count;
    }

    public void TapFunc()
    {
        if (numberOfFingers == 1)
        {
            Debug.Log("Tapped");
        }
    }
}
