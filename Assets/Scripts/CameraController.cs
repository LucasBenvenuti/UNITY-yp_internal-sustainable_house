﻿using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LeanTweenType inOutType;

    public static CameraController instance;
    public GameObject cameraTransform;
    public Transform itemZoomPosition;
    public Vector3 itemZoomOffset;
    public Vector3 cameraBasePosition;
    public bool zoomControl;
    public bool zoomTeste;
    public LeanDragCamera leanDrag;
    // public LeanTwistRotateAxis leanTwist;
    public LeanPinchScale leanPinch;

    public ClampTransform clampTransform;

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
        cameraBasePosition = cameraTransform.transform.position;
    }

    public void LerpToZoomPosition(GameObject itemSelected)
    {
        cameraBasePosition = cameraTransform.transform.position;

        clampTransform.enabled = false;

        leanDrag.enabled = false;
        // leanTwist.enabled = false;
        leanPinch.enabled = false;

        int childCount = itemSelected.transform.childCount;

        //GET FIRST CHILD POSITION - CAMERA POINT
        Vector3 itemZoomPosition = itemSelected.transform.GetChild(childCount - 1).position;

        LeanTween.move(cameraTransform, itemZoomPosition, tweenDuration).setEase(inOutType);

        zoomTeste = true;
    }
    public void ReturnToBasePosition()
    {
        StartCoroutine(ReturnToBasePositionNew());
    }

    public IEnumerator ReturnToBasePositionNew()
    {
        leanDrag.enabled = true;
        // leanTwist.enabled = true;
        leanPinch.enabled = true;

        LeanTween.move(cameraTransform, cameraBasePosition, tweenDuration).setEase(inOutType);

        GameController.instance.panelItem.SetActive(false);

        yield return new WaitForSeconds(tweenDuration);

        clampTransform.enabled = true;
    }

    public void ChangeZoomControl()
    {
        zoomControl = !zoomControl;
    }
}
