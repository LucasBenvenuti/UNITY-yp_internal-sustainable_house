using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public CanvasGroup fadePanel;
    public Image fadeMaterial;
    public LeanTweenType easeInOut;
    public float tweenDuration;
    public float fadeInDuration;

    public Color fadeColor;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartScene()
    {
        fadeMaterial.color = fadeColor;

        StartCoroutine(FadeIn());
    }

    public void ChangeScene(string scene)
    {
        fadeMaterial.color = fadeColor;

        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeInDuration);

        LeanTween.alphaCanvas(fadePanel, 0f, tweenDuration);
    }

    IEnumerator FadeOut(string scene)
    {
        LeanTween.alphaCanvas(fadePanel, 1f, tweenDuration);

        yield return new WaitForSeconds(tweenDuration);

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
