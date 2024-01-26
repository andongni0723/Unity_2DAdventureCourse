using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[Header("Component")]
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    protected Animator animator => GetComponent<Animator>();
    protected SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();
    
    [Header("Settings")] 
    public float normalSpeed = 10;
    public float chaseSpeed = 15;
    public float waitTime = 1;
    
    //[Header("Debug")]
    private float currentSpeed;
    public Vector3 faceDir;
    public bool isWait;
    
    private Timer waitTimerCounter = new Timer();

    private void Awake()
    {
        currentSpeed = normalSpeed;
    }

    #region Event

    private void OnEnable()
    {
        waitTimerCounter.timerStartEvent += TimerStart;
        waitTimerCounter.timerFinishEvent += TimerFinish;
    }

    private void OnDisable()
    {
        waitTimerCounter.timerStartEvent -= TimerStart;
        waitTimerCounter.timerFinishEvent -= TimerFinish;
    }

    private void TimerStart()
    {
        isWait = true;
    }

    private void TimerFinish()
    {
        isWait = false;
        transform.localScale = new Vector3(faceDir.x, 1, 1);
    }

    #endregion 
    
    private void Update()
    {
        // left: -1, right: 1
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // Touch wall and not wait
        if (((physicsCheck.touchLeftWall && faceDir.x < 0) || 
            (physicsCheck.touchRightWall && faceDir.x > 0)) && !isWait)
        {
            waitTimerCounter.StartTimer(waitTime);
            animator.SetBool("isWalk", false); 
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Base move function
    /// </summary>
    protected virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }
}
