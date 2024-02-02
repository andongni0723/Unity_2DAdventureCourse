using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Events")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;
    
    [Header("Component")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEventRaised;
        BGMEvent.OnEventRaised += OnBGMEventRaised;
    }

    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEventRaised;
        BGMEvent.OnEventRaised -= OnBGMEventRaised; 
    }

    private void OnBGMEventRaised(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    private void OnFXEventRaised(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }

    #endregion 
}
