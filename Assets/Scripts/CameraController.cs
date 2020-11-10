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

        int childCount = itemSelected.transform.childCount;
        Vector3 itemZoomPosition = new Vector3(0, 0, 0);

        if (childCount > 0)
        {
            //GET FIRST CHILD POSITION - CAMERA POINT
            itemZoomPosition = itemSelected.transform.GetChild(childCount - 1).position;
            LeanTween.move(this.gameObject, itemZoomPosition, tweenDuration).setEase(inOutType);
        }
        else
        {
            tweenDuration = tweenDuration * 1.3f;

            itemZoomPosition = itemSelected.transform.position;
            LeanTween.move(this.gameObject.transform.parent.gameObject, itemZoomPosition, tweenDuration).setEase(inOutType);
        }
    }
    public void ReturnToBasePosition()
    {
        StartCoroutine(ReturnToBasePositionNew());
    }

    public IEnumerator ReturnToBasePositionNew()
    {
        if (GameController.instance.backBtn.activeInHierarchy)
        {
            GameController.instance.backBtn.SetActive(false);
        }

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
}
