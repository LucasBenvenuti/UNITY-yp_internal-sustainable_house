using ZXing;
using ZXing.QrCode;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class QRCode_Reader : MonoBehaviour
{
    private WebCamTexture camTexture;
    public Image cameraSpace;

    public string acceptText = "YellowPanda";
    public string SceneToGo = "MainMenu";

    void Start()
    {
        // screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            cameraSpace.material.mainTexture = camTexture;

            camTexture.Play();
        }
    }

    void OnGUI()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if (result != null)
            {
                if (result.Text == acceptText)
                {
                    Debug.Log("CORRECT TEXT! QR Code text: " + result.Text);
                    Debug.Log("REDIRECTING TO REGISTER...");

                    SceneManager.LoadScene(SceneToGo, LoadSceneMode.Single);
                }
                else
                {
                    Debug.Log("WRONG TEXT FROM QR CODE! QR Code text is: " + result.Text + ", it MUST be " + acceptText);
                }
            }
        }
        catch (UnityException ex) { Debug.LogWarning(ex.Message); }
    }
}