using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportGenerator : MonoBehaviour
{
    public GameObject printPoint;
    public PDF_Generator pdfGenerator;
    public float printCameraZoom = 15f;
    public Camera mainCamera;
    public Vector3 endCameraPosition;
    public float tweenDuration = 0.5f;
    public LeanTweenType easeInOut;
    public LeanPinchCamera leanPinch;

    public void GenerateReport()
    {
        StartCoroutine(PrintFunc());
    }

    public IEnumerator PrintFunc()
    {
        yield return new WaitUntil(() => TimerController.instance.inGame == false);

        GameController.instance.closePanel();

        DataStorage.instance.addReportLine("Fim de Jogo.");
        DataStorage.instance.addReportLine("Recursos Finais: " + UIController.instance.moneySlider.value + ".");
        DataStorage.instance.addReportLine("Sustentabilidade Final: " + UIController.instance.sustainabilitySlider.value + ".");

        LeanTween.move(mainCamera.gameObject.transform.parent.gameObject, printPoint.transform.position, tweenDuration).setEase(easeInOut);
        LeanTween.move(mainCamera.gameObject, printPoint.transform.position, tweenDuration).setEase(easeInOut);
        LeanTween.value(mainCamera.gameObject, mainCamera.orthographicSize, printCameraZoom, tweenDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                if (!GameController.instance.canGoToObject)
                {
                    mainCamera.orthographicSize = flt;
                }
                else
                {
                    leanPinch.Zoom = flt;
                }
            }).setOnComplete(() => { pdfGenerator.GeneratePDF(); });
    }
}
