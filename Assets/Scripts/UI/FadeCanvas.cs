using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    [Header("Event")]
    public FadeEventSO fadeEventSO;
    
    [Header("Component")]
    public Image fadeImage;
    //[Header("Settings")]
    //[Header("Debug")]

    private void OnEnable()
    {
        fadeEventSO.OnEventRaised += OnFadeEvent;
        
    }
    private void OnDisable()
    {
        fadeEventSO.OnEventRaised -= OnFadeEvent;
    }


    /// <summary>
    /// Fade In/Out function on UI
    /// </summary>
    /// <param name="fadeTarget">1: fade in, 0: fade out</param>
    /// <param name="duration"></param>
    public void OnFadeEvent(int fadeTarget, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        if (fadeTarget == 0)
            sequence.AppendInterval(1);
        sequence.Append(fadeImage.DOFade(fadeTarget, duration));
    }
}
