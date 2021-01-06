using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotHighRes : MonoBehaviour
{
    public int resWidth = 1280;
    public int resHeight = 800;

    public PDF_Generator MainGO;

    public Camera mainCamera;
    public Canvas BarCanvas;
    private Texture2D _screenShot;
    public Image canvasImage;

    [HideInInspector]
    public byte[] byteTest;

    void Start()
    {
        if (BarCanvas)
        {
            BarCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format(System.Environment.CurrentDirectory + @"\Assets\screenshots\screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public IEnumerator TakeScreenShot()
    {
        if (BarCanvas)
        {
            BarCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        }

        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        mainCamera.targetTexture = rt;
        _screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        mainCamera.Render();
        RenderTexture.active = rt;
        _screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        _screenShot.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        string filename = ScreenShotName(resWidth, resHeight);

        // byte[] bytes = _screenShot.EncodeToPNG();
        byteTest = _screenShot.EncodeToPNG();
        // System.IO.File.WriteAllBytes(filename, byteTest);

        Debug.Log(string.Format("Took screenshot to: {0}", filename));

        if (canvasImage)
        {
            Sprite tempSprite = Sprite.Create(_screenShot, new Rect(0, 0, resWidth, resHeight), new Vector2(0, 0));
            canvasImage.sprite = tempSprite;
        }

        Debug.Log("Ended TAKESCREENSHOT");

        if (BarCanvas)
        {
            BarCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
}
