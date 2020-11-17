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

    public Camera mainCamera;
    float cameraBaseSize;

    public LeanPitchYaw leanTwist;
    public LeanPinchCamera leanPinch;
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

    public void LerpToZoomPosition(GameObject itemSelected, float cameraZoom)
    {
        if (!GameController.instance.canGoToObject)
        {
            return;
        }

        cameraBasePosition = this.gameObject.transform.position;
        cameraBaseSize = mainCamera.orthographicSize;
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
        leanPinch.enabled = false;

        int childCount = itemSelected.transform.childCount;
        Vector3 itemZoomPosition = new Vector3(0, 0, 0);

        if (childCount > 0)
        {
            //GET FIRST CHILD POSITION - CAMERA POINT
            itemZoomPosition = itemSelected.transform.GetChild(childCount - 1).position;
            LeanTween.move(this.gameObject, itemZoomPosition, tweenDuration).setEase(inOutType);
            LeanTween.value(this.gameObject, mainCamera.orthographicSize, cameraZoom, tweenDuration).setEase(inOutType).setOnUpdate((float flt) =>
            {
                mainCamera.orthographicSize = flt;
            });
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
        leanTwist.SetYaw(yawAngle);
        leanTwist.Dampening = rotationDampening;

        LeanTween.move(this.gameObject, cameraBasePosition, tweenDuration / 1f).setEase(inOutType);
        LeanTween.value(this.gameObject, mainCamera.orthographicSize, cameraBaseSize, tweenDuration).setEase(inOutType).setOnUpdate((float flt) =>
            {
                mainCamera.orthographicSize = flt;
            });

        yield return new WaitForSeconds(tweenDuration);

        leanTwist.Dampening = rotationDampening;

        leanDrag.Sensitivity = dragSensitivity;
        leanTwist.YawSensitivity = yawSensitivity;

        leanDrag.enabled = true;
        leanPinch.enabled = true;

        GameController.instance.canGoToObject = true;
    }
}
