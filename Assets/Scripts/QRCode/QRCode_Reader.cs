using ZXing;
using ZXing.QrCode;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class QRCode_Reader : MonoBehaviour
{
    // private WebCamTexture camTexture;
    // public Image cameraSpace;
    public LeanTweenType inOutType;
    public CanvasGroup centerIcon;

    public string acceptText = "YellowPanda";
    public string SceneToGo = "MainMenu";

    bool canLerp;

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

    bool cameraInitialized;

    void Awake()
    {
        image.material.color = new Color(1f, 1f, 1f, 0f);

        canLerp = true;
    }

    IEnumerator Start()
    {
        if (DataStorage.instance)
        {
            DataStorage.instance.DeleteAllData();
        }

        if (SceneController.instance)
        {
            SceneController.instance.StartScene();
        }
        else
        {
            Debug.Log("SceneController doesnt exist!");
        }

        while (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("Webcam Not Authorized!");

            yield return null;
        }
        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
            yield return null;
        }

        // Get the device's cameras and create WebCamTextures with them
        frontCameraDevice = WebCamTexture.devices.Last();
        backCameraDevice = WebCamTexture.devices.First();

        frontCameraTexture = new WebCamTexture(frontCameraDevice.name, 1280, 720, 30);
        backCameraTexture = new WebCamTexture(backCameraDevice.name, 1280, 720, 30);

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
        image.material.color = new Color(1f, 1f, 1f, 1f);

        activeCameraTexture.Play();
        cameraInitialized = true;
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
        if (activeCameraTexture)
        {
            if (activeCameraTexture.width < 100)
            {
                Debug.Log("Still waiting another frame for correct info...");
                return;
            }
        }
        else
        {
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

        if (cameraInitialized)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                // var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                var result = barcodeReader.Decode(activeCameraTexture.GetPixels32(), activeCameraTexture.width, activeCameraTexture.height);
                if (result != null)
                {
                    StopCoroutine(ReturnIcon());

                    if (canLerp && centerIcon.alpha == 1)
                    {
                        canLerp = false;
                        LeanTween.alphaCanvas(centerIcon, 0f, 0.5f).setEase(inOutType);
                    }

                    if (result.Text == acceptText)
                    {

                        Debug.Log("CORRECT TEXT! QR Code text: " + result.Text);
                        Debug.Log("REDIRECTING TO REGISTER...");

                        // camTexture = null;
                        activeCameraTexture = null;

                        SceneController.instance.ChangeScene("MainMenu");
                    }
                    else
                    {
                        Debug.Log("WRONG TEXT FROM QR CODE! QR Code text is: " + result.Text + ", it MUST be " + acceptText);
                    }
                }
                else
                {
                    if (centerIcon.alpha == 0)
                    {
                        StopCoroutine(ReturnIcon());
                        StartCoroutine(ReturnIcon());
                    }
                }
            }
            catch (UnityException ex) { Debug.LogWarning(ex.Message); }
        }

    }

    IEnumerator ReturnIcon()
    {
        yield return new WaitForSeconds(3f);

        if (canLerp == false)
        {
            Debug.Log(canLerp);

            LeanTween.alphaCanvas(centerIcon, 1f, 0.5f).setEase(inOutType);

            canLerp = true;

            StopCoroutine(ReturnIcon());
        }
        else
        {
            yield return null;
        }
    }
}