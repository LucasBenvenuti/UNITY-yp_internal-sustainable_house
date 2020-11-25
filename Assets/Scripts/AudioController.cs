using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource backgroundIngameSource;
    public AudioSource buttonSource;
    public float animDuration;
    public LeanTweenType easeInOut;

    public float volumeBackgroundInGame = 0.04f;
    public float volumeButtons = 0.1f;

    public bool muted = false;

    // Update is called once per frame

    public void Start()
    {
        StartSound();
    }

    public void StartSound()
    {
        backgroundIngameSource.volume = volumeBackgroundInGame;
        buttonSource.volume = volumeButtons;
    }

    public void ToggleBackgroundAudio()
    {
        if (!muted)
        {
            buttonSource.volume = 0f;
            LeanTween.value(this.gameObject, backgroundIngameSource.volume, 0f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                backgroundIngameSource.volume = flt;
            }).setOnComplete(() =>
            {
                muted = true;
            });
        }
        else
        {
            buttonSource.volume = volumeButtons;
            LeanTween.value(this.gameObject, backgroundIngameSource.volume, volumeBackgroundInGame, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
                {
                    backgroundIngameSource.volume = flt;
                }).setOnComplete(() =>
                {
                    muted = false;
                });
        }
    }
}
