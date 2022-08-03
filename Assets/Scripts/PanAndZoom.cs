using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAndZoom : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        //pan
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -36.5f, 37.5f), Mathf.Clamp(transform.position.y, 10, 60), Mathf.Clamp(transform.position.z, -45.5f, 20.5f));
        }else if (Input.touchCount == 2)         //pinch to zoom
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.fieldOfView += deltaMagnitudDiff * 0.1f;

            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 50f);

        }

    }

    //void NewPinchZoom()
    //{
    //    Lean.Touch.LeanGesture.
    //}
}
