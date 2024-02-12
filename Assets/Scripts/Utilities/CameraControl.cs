using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    //[Header("Component")]
    private CinemachineConfiner2D confiner2D => GetComponent<CinemachineConfiner2D>();
    public CinemachineImpulseSource impulseSource;
    
    [Header("Event")]
    public VoidEventSO cameraShakeEvent;
    public VoidEventSO afterSceneLoadEventSO;
    
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraSkakeEvent;
        afterSceneLoadEventSO.OnEventRaised += OnAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraSkakeEvent;
        afterSceneLoadEventSO.OnEventRaised -= OnAfterSceneLoadEvent;
    }

    private void OnAfterSceneLoadEvent()
    {
        GetNewCameraBound();
    }

    private void OnCameraSkakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    #endregion 
    

    private void GetNewCameraBound()
    {
        var obj = GameObject.FindWithTag("Bounce");

        if(obj == null) return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<PolygonCollider2D>();
        confiner2D.InvalidateCache();
    }
}
