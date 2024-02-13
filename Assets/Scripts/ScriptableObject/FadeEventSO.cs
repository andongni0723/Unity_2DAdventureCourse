using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<int, float> OnEventRaised;
    
    public void RaiseEvent(int fadeTarget, float duration)
    {
        OnEventRaised?.Invoke(fadeTarget, duration);
    }
    
    public void FadeIn(float duration)
    {
        RaiseEvent(1, duration);
    }
    
    public void FadeOut(float duration)
    {
        RaiseEvent(0, duration);
    }
}
