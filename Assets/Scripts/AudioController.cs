using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource backgroundIngameSource;
    public AudioSource buttonSource;
    public AudioSource actionsSource;
    public AudioSource changeItemsSource;
    public AudioSource endTimerSource;
    public AudioSource openFinalScreenSource;
    public AudioSource menuBackgroundSource;
    public float animDuration;
    public LeanTweenType easeInOut;

    public float volumeBackgroundInGame = 0.04f;
    public float volumeButtons = 0.1f;
    public float volumeActions = 0.1f;
    public float volumeChangeItem = 0.1f;
    public float volumeEndTimer = 0.1f;
    public float volumeOpenFinalScreen = 0.1f;
    public float volumeMenuBackground = 0.04f;

    public bool playedEndGame = false;

    public Animator soundIcon;

    // Update is called once per frame

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        StartSound();
    }

    public void StartSound()
    {
        if (!DataStorage.instance.soundMuted)
        {
            backgroundIngameSource.volume = volumeBackgroundInGame;
            buttonSource.volume = volumeButtons;
            actionsSource.volume = volumeActions;
            changeItemsSource.volume = volumeChangeItem;
            openFinalScreenSource.volume = volumeOpenFinalScreen;
            menuBackgroundSource.volume = volumeMenuBackground;

            if (TimerController.instance)
            {
                if (TimerController.instance.totalTime > 5f)
                {
                    endTimerSource.volume = volumeEndTimer;
                }
                else
                {
                    endTimerSource.volume = 0f;
                    endTimerSource.Stop();
                }
            }
        }
        else
        {
            buttonSource.volume = 0f;
            actionsSource.volume = 0f;
            changeItemsSource.volume = 0f;
            endTimerSource.volume = 0f;
            openFinalScreenSource.volume = 0f;
            openFinalScreenSource.volume = 0f;
            menuBackgroundSource.volume = 0f;
            backgroundIngameSource.volume = 0f;

            soundIcon.SetTrigger("Off");
        }
    }

    public void ToggleBackgroundAudio()
    {
        if (!DataStorage.instance.soundMuted)
        {
            buttonSource.volume = 0f;
            actionsSource.volume = 0f;
            changeItemsSource.volume = 0f;
            endTimerSource.volume = 0f;
            openFinalScreenSource.volume = 0f;
            openFinalScreenSource.volume = 0f;
            menuBackgroundSource.volume = 0f;
            LeanTween.value(this.gameObject, backgroundIngameSource.volume, 0f, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
            {
                backgroundIngameSource.volume = flt;
            }).setOnComplete(() =>
            {
                DataStorage.instance.soundMuted = true;
            });
        }
        else
        {
            buttonSource.volume = volumeButtons;
            actionsSource.volume = volumeActions;
            changeItemsSource.volume = volumeChangeItem;
            endTimerSource.volume = volumeEndTimer;
            openFinalScreenSource.volume = volumeOpenFinalScreen;
            menuBackgroundSource.volume = volumeMenuBackground;
            LeanTween.value(this.gameObject, backgroundIngameSource.volume, volumeBackgroundInGame, animDuration).setEase(easeInOut).setOnUpdate((float flt) =>
                {
                    backgroundIngameSource.volume = flt;
                }).setOnComplete(() =>
                {
                    DataStorage.instance.soundMuted = false;
                });
        }
    }

    public void PlayActionsAudio()
    {
        actionsSource.Play();
    }
    public void PlayEndTimerAudio()
    {
        endTimerSource.Play();
    }
    public void PlayOpenFinalScreenAudio()
    {
        openFinalScreenSource.Play();
    }
}
