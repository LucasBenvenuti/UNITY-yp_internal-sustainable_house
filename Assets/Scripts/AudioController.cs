using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource buttonSource;
    public float animDuration;
    public LeanTweenType easeInOut;

    public float volume = 0.04f;

    // Update is called once per frame
    public void ToggleBackgroundAudio()
    {
        if (audioSource.volume != 0f)
        {
            LeanTween.value(this.gameObject, this.audioSource.volume, 0f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                audioSource.volume = flt;
            });
            buttonSource.volume = 0f;
        }
        else
        {
            buttonSource.volume = 0.2f;
            LeanTween.value(this.gameObject, this.audioSource.volume, volume, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
 {
     audioSource.volume = flt;
 });
        }
    }
}
