using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LeanTweenType inOutType;

    [HideInInspector]
    public static CameraController instance;

    [HideInInspector]
    public Transform itemZoomPosition;
    [HideInInspector]
    public Vector3 cameraBasePosition;

    public LeanPitchYaw leanTwist;
    public LeanMaintainDistance leanDistance;
    public LeanMultiPinch leanMultiPinch;
    public LeanDragCamera leanDrag;
    // public LeanTwistRotateAxis leanTwist;
    // public  leanPinch;
    [HideInInspector]
    public float yawAngle;
    [HideInInspector]
    public float yawSensitivity;
    [HideInInspector]
    public float rotationDampening;
    [HideInInspector]
    public float dragSensitivity;

    public float tweenDuration = 0.5f;

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

    private void Start()
    {
        cameraBasePosition = this.gameObject.transform.position;
    }

    public void LerpToZoomPosition(GameObject itemSelected)
    {
        if (!GameController.instance.canGoToObject)
        {
            return;
        }

        cameraBasePosition = this.gameObject.transform.position;
        GameController.instance.canGoToObject = false;
        // leanDrag.enabled = false;
        // leanTwist.enabled = false;
        // leanPinch.enabled = false;

        int childCount = itemSelected.transform.childCount;

        //GET FIRST CHILD POSITION - CAMERA POINT
        Vector3 itemZoomPosition = itemSelected.transform.GetChild(childCount - 1).position;

        dragSensitivity = leanDrag.Sensitivity;
        yawSensitivity = leanTwist.YawSensitivity;
        rotationDampening = leanTwist.Dampening;
        yawAngle = this.transform.parent.eulerAngles.y;

        leanTwist.Dampening = 7f;
        leanDrag.Sensitivity = 0f;
        leanTwist.YawSensitivity = 0f;
        leanTwist.SetYaw(45);

        leanDrag.enabled = false;
        leanDistance.enabled = false;
        leanMultiPinch.enabled = false;

        LeanTween.move(this.gameObject, itemZoomPosition, tweenDuration).setEase(inOutType);

        // zoomTeste = true;
    }
    public void ReturnToBasePosition()
    {
        StartCoroutine(ReturnToBasePositionNew());
    }

    public IEnumerator ReturnToBasePositionNew()
    {
        // leanDrag.enabled = true;
        // leanTwist.enabled = true;
        // leanPinch.enabled = true;


        leanTwist.SetYaw(yawAngle);
        leanTwist.Dampening = rotationDampening;

        LeanTween.move(this.gameObject, cameraBasePosition, tweenDuration / 1f).setEase(inOutType);

        GameController.instance.panelItem.SetActive(false);

        yield return new WaitForSeconds(tweenDuration);

        leanTwist.Dampening = rotationDampening;

        leanDrag.Sensitivity = dragSensitivity;
        leanTwist.YawSensitivity = yawSensitivity;

        leanDrag.enabled = true;
        leanDistance.enabled = true;
        leanMultiPinch.enabled = true;

        GameController.instance.canGoToObject = true;
    }

    // public void ChangeZoomControl()
    // {
    //     zoomControl = !zoomControl;
    // }
}
