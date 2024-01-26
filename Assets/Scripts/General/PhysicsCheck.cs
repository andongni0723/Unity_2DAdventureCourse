using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D collider => GetComponent<CapsuleCollider2D>();
    
    [Header("Settings")] 
    public bool isManual; 
    
    [Space(10)]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float CheckRadius = 0.1f;
    public LayerMask groundLayer;
    
    [Header("Debug")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake()
    {
        if (!isManual)
        {
            rightOffset = new Vector2(collider.bounds.size.x / 2f + collider.offset.x, collider.bounds.size.y / 2f);
            leftOffset = new Vector2(-collider.bounds.size.x / 2f + collider.offset.x, collider.bounds.size.y / 2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), CheckRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), CheckRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), CheckRadius);
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        // Check ground
        isGround = Physics2D.OverlapCircle(
            (Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), CheckRadius, groundLayer);
        
        // Check left wall
        touchLeftWall = Physics2D.OverlapCircle(
            (Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), CheckRadius, groundLayer);
        
        // Check right wall
        touchRightWall = Physics2D.OverlapCircle(
            (Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), CheckRadius, groundLayer);
    }
    
    
}
