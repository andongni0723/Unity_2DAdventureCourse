using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("Settings")]
    public Vector2 bottomCheckOffset;
    public float CheckRadius = 0.1f;
    public LayerMask groundLayer;
    
    [Header("Debug")]
    public bool isGround;

    private void Awake()
    {
        //groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)bottomCheckOffset, CheckRadius);
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        // Check ground
        isGround = Physics2D.OverlapCircle(
            transform.position + (Vector3)bottomCheckOffset, CheckRadius, groundLayer);
    }
}
