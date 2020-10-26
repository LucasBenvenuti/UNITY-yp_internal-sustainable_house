using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform cameraTransform;
    public Transform itemZoomPosition;
    public Vector3 itemZoomOffset;
    public Vector3 cameraBasePosition;
    public bool zoomControl;
    public bool zoomTeste;
    public LeanDragCamera leanDrag;
    public LeanTwistRotateAxis leanTwist;
    public LeanPinchScale leanPinch;

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
        cameraBasePosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        if (zoomControl)
        {
            LerpToZoomPosition(GameController.instance.itemHolder);
        }
        else if (!zoomControl && zoomTeste)
        {
            ReturnToBasePosition();
        }
    }
    public void LerpToZoomPosition(GameObject itemSelected)
    {
        leanDrag.enabled = false;
        leanTwist.enabled = false;
        leanPinch.enabled = false;
        Vector3 itemZoomPosition = itemSelected.transform.position + itemZoomOffset;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, itemZoomPosition, Time.deltaTime * 5);
        zoomTeste = true;


    }
    public void ReturnToBasePosition()
    {
        leanDrag.enabled = true;
        leanTwist.enabled = true;
        leanPinch.enabled = true;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraBasePosition, Time.deltaTime * 5);
        GameController.instance.panelItem.SetActive(false);
    }

    public void ChangeZoomControl()
    {
        zoomControl = !zoomControl;
    }
}
