using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    [Header("Events")] 
    public PlayAudioEventSO playAudioEvent;
    //[Header("Component")]

    [Header("Settings")]
    public bool playOnEnable;
    public AudioClip audioClip;
    
    //[Header("Debug")]

    private void OnEnable()
    {
        if(playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}
