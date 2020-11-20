using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public float animDuration;
    public LeanTweenType easeInOut;
    void Start()
    {

    }

    // Update is called once per frame
    public void ToggleBackgroundAudio()
    {
        if (audioSource.volume != 0f)
        {
            LeanTween.value(this.gameObject, this.audioSource.volume, 0f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                audioSource.volume = flt;
            });
        }
        else
        {
            LeanTween.value(this.gameObject, this.audioSource.volume, 0.1f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
 {
     audioSource.volume = flt;
 });
        }
    }
}
