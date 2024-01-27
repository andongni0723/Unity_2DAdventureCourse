using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    //[Header("Component")]
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [HideInInspector] public Animator animator => GetComponent<Animator>();
    protected SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    
    protected CapsuleCollider2D capsuleCollider => GetComponent<CapsuleCollider2D>();
    [HideInInspector] public PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();
    
    [Header("Settings")] 
    public float normalSpeed = 10;
    public float chaseSpeed = 15;
    public float hurtForce = 5;
    public float waitTime = 1;
    public float lostTargetTime;
    
    [Space(20)]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask playerLayer;
    
    [Header("Debug")]
    public Vector3 faceDir;
    [HideInInspector] public float currentSpeed;
    public Transform attacker;
    public bool isWait;
    public bool isHurt;
    public bool isDead;
    public bool hasTarget;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    
    [HideInInspector] public Timer waitTimer = new Timer();
    [HideInInspector] public Timer LostTargetTimer = new Timer();

    protected virtual void Awake()
    {
        currentSpeed = normalSpeed;
    }

    #region Event

    private void OnEnable()
    {
        // State
        currentState = patrolState;
        currentState.OnEnter(this);
        
        // Timer
        waitTimer.timerStartEvent += TimerStart;
        waitTimer.timerFinishEvent += TimerFinish;
        
        LostTargetTimer.timerFinishEvent += LostTargetTimerFinish;
    }

    private void OnDisable()
    {
        // State
        currentState.OnExit();
        
        // Timer
        waitTimer.timerStartEvent -= TimerStart;
        waitTimer.timerFinishEvent -= TimerFinish;
        
        LostTargetTimer.timerFinishEvent -= LostTargetTimerFinish;
    }

    private void TimerStart()
    {
        isWait = true;
        transform.localScale = new Vector3(faceDir.x, 1, 1);
    }

    private void TimerFinish()
    {
        isWait = false;
        // transform.localScale = new Vector3(faceDir.x, 1, 1);
    }

    private void LostTargetTimerFinish()
    {
        hasTarget = false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + 
                              new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }

    private void Update()
    {
        // State
        currentState.LogicUpdate();
        
        // left: -1, right: 1
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        //Check Lost Target
        if (hasTarget && !FoundPlayer() && !LostTargetTimer.isTiming)
        {
            LostTargetTimer.StartTimer(lostTargetTime);
            Debug.Log("Lost Timer Start");
        }
        else if (hasTarget && FoundPlayer() && LostTargetTimer.isTiming)
        {
            LostTargetTimer.StopTimer();
            Debug.Log("Lost Timer End S");

        }
    }
    private void FixedUpdate()
    {
        // State
        currentState.PhysicsUpdate();
        
        // Move
        if(!isHurt && !isDead && !isWait) 
            Move();

    }

    /// <summary>
    /// Base move function
    /// </summary>
    protected virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }

    
    /// <summary>
    /// Is FindPlayer
    /// </summary>
    /// <returns></returns>
    public bool FoundPlayer()
    {
        var result = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0,
            faceDir, checkDistance, playerLayer);

        if (result) hasTarget = true;
        
        return result;
    }

    /// <summary>
    /// Switch state
    /// </summary>
    /// <param name="state">the state want switch to</param>
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        
        currentState.OnExit();
        currentState = newState;
        currentState?.OnEnter(this);
    }

    
    #region Unity Event

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        
        // Flip
        if(attackTrans.position.x - transform.position.x > 0) 
            transform.localScale = new Vector3(-1, 1, 1);
        if(attackTrans.position.x - transform.position.x < 0) 
            transform.localScale = new Vector3(1, 1, 1);
        
        // Knock-back
        isHurt = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        var ctsInfo = TaskSingol.CreatCts();
        OnHurt(dir, ctsInfo.cts.Token);
    }

    public void OnDead()
    {
        animator.SetBool("isDead", true);
        capsuleCollider.enabled = false;
        isDead = true;
    }

    async UniTask OnHurt(Vector2 dir, CancellationToken ctx)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: ctx);
        isHurt = false;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
    #endregion
}
