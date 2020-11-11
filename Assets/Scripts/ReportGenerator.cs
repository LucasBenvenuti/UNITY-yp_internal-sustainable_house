using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportGenerator : MonoBehaviour
{
    public GameObject printPoint;
    public GameObject interfaceObject;

    public ScreenShotHighRes screenShot;

    public PDF_Generator pdfGenerator;

    public float printCameraZoom = 15f;

    public void PrintScene()
    {
        StartCoroutine(PrintFunc());
    }

    public IEnumerator PrintFunc()
    {
        if (!GameController.instance.canGoToObject)
        {
            CameraController.instance.ReturnToBasePosition();

            yield return new WaitForSeconds(CameraController.instance.tweenDuration);
        }

        Debug.Log(printPoint.transform.position);

        CameraController.instance.LerpToZoomPosition(printPoint, printCameraZoom);

        yield return new WaitForSeconds(CameraController.instance.tweenDuration);

        pdfGenerator.Click();
    }

}
