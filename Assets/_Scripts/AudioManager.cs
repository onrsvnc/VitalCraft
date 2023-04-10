using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get => _instance; }

    public AudioClip buttonClickedSound, placeBuildingSound, removeBuildingSound;
    public AudioSource effectAudioSource;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayButtonClickedSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = buttonClickedSound;
        effectAudioSource.Play();
    }

    public void PlayPlaceBuildSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = placeBuildingSound;
        effectAudioSource.Play();
    }

    public void PlayDemolitionSound()
    {
        Debug.Log("deleteme");
        effectAudioSource.Stop();
        effectAudioSource.clip = removeBuildingSound;
        effectAudioSource.Play();
    }

    
}
