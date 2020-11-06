using ZXing;
using ZXing.QrCode;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

using UnityEngine.SceneManagement;

public class QRCode_Reader : MonoBehaviour
{
    // private WebCamTexture camTexture;
    // public Image cameraSpace;

    public string acceptText = "YellowPanda";
    public string SceneToGo = "MainMenu";

    //NEW

    public RawImage image;
    public RectTransform imageParent;
    public AspectRatioFitter imageFitter;

    // Device cameras
    WebCamDevice frontCameraDevice;
    WebCamDevice backCameraDevice;
    WebCamDevice activeCameraDevice;

    WebCamTexture frontCameraTexture;
    WebCamTexture backCameraTexture;
    WebCamTexture activeCameraTexture;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Image uvRect
    Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
    Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

    // Image Parent's scale
    Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    Vector3 fixedScale = new Vector3(-1f, 1f, 1f);



    // void Start()
    // {
    //     // screenRect = new Rect(0, 0, Screen.width, Screen.height);
    //     camTexture = new WebCamTexture();
    //     camTexture.requestedHeight = Screen.height / 2;
    //     camTexture.requestedWidth = Screen.width / 2;
    //     if (camTexture != null)
    //     {
    //         cameraSpace.material.mainTexture = camTexture;

    //         camTexture.requestedFPS = 60;

    //         camTexture.Play();
    //     }
    // }

    void Start()
    {
        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
            return;
        }

        // Get the device's cameras and create WebCamTextures with them
        frontCameraDevice = WebCamTexture.devices.Last();
        backCameraDevice = WebCamTexture.devices.First();

        frontCameraTexture = new WebCamTexture(frontCameraDevice.name);
        backCameraTexture = new WebCamTexture(backCameraDevice.name);

        // Set camera filter modes for a smoother looking image
        frontCameraTexture.filterMode = FilterMode.Trilinear;
        backCameraTexture.filterMode = FilterMode.Trilinear;

        // Set the camera to use by default
        SetActiveCamera(backCameraTexture);
    }

    // Set the device camera to use and start it
    public void SetActiveCamera(WebCamTexture cameraToUse)
    {
        if (activeCameraTexture != null)
        {
            activeCameraTexture.Stop();
        }

        activeCameraTexture = cameraToUse;
        activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device =>
            device.name == cameraToUse.deviceName);

        image.texture = activeCameraTexture;
        image.material.mainTexture = activeCameraTexture;

        activeCameraTexture.Play();
    }

    // Switch between the device's front and back camera
    public void SwitchCamera()
    {
        SetActiveCamera(activeCameraTexture.Equals(frontCameraTexture) ?
            backCameraTexture : frontCameraTexture);
    }

    // Make adjustments to image every frame to be safe, since Unity isn't 
    // guaranteed to report correct data as soon as device camera is started
    void Update()
    {
        // Skip making adjustment for incorrect camera data
        if (activeCameraTexture.width < 100)
        {
            Debug.Log("Still waiting another frame for correct info...");
            return;
        }

        // Rotate image to show correct orientation 
        rotationVector.z = -activeCameraTexture.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // Set AspectRatioFitter's ratio
        float videoRatio =
            (float)activeCameraTexture.width / (float)activeCameraTexture.height;
        imageFitter.aspectRatio = videoRatio;

        // Unflip if vertically flipped
        image.uvRect =
            activeCameraTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

        // Mirror front-facing camera's image horizontally to look more natural
        imageParent.localScale =
            activeCameraDevice.isFrontFacing ? fixedScale : defaultScale;
    }

    void OnGUI()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            // var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            var result = barcodeReader.Decode(activeCameraTexture.GetPixels32(), activeCameraTexture.width, activeCameraTexture.height);
            if (result != null)
            {
                if (result.Text == acceptText)
                {
                    Debug.Log("CORRECT TEXT! QR Code text: " + result.Text);
                    Debug.Log("REDIRECTING TO REGISTER...");

                    // camTexture = null;
                    activeCameraTexture = null;

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