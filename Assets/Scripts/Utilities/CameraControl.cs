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
    
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraSkakeEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraSkakeEvent;
    }

    private void OnCameraSkakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    #endregion 
    
    private void Start()
    {
        //TODO: New scene
        GetNewCameraBound();
    }

    private void GetNewCameraBound()
    {
        var obj = GameObject.FindWithTag("Bounce");

        if(obj == null) return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<PolygonCollider2D>();
        confiner2D.InvalidateCache();
    }
}
