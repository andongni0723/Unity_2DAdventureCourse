using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Events")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO volumeChangeEvent;
    public VoidEventSO pauseEvent;
    public FloatEventSO syncVolumeEvent;
    
    [Header("Component")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioMixer audioMixer;
    
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEventRaised;
        BGMEvent.OnEventRaised += OnBGMEventRaised;
        volumeChangeEvent.OnEventRaised += OnVolumeChangeEvent;
        pauseEvent.OnEventRaised += OnPauseEvent;
    }

    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEventRaised;
        BGMEvent.OnEventRaised -= OnBGMEventRaised; 
        volumeChangeEvent.OnEventRaised -= OnVolumeChangeEvent;
        pauseEvent.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        audioMixer.GetFloat("MasterVolume", out var amount);
        
        syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeChangeEvent(float amount)
    {
        audioMixer.SetFloat("MasterVolume", amount);
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
