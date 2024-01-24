using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();

    private void Update()
    {
       SetAnimation(); 
    }


    /// <summary>
    /// Set animator parameter (in Update())
    /// </summary>
    private void SetAnimation()
    {
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
    }
}
