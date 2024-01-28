using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PhysicsCheck), typeof(Character ), typeof(Attack))]
public class Enemy : MonoBehaviour
{
    //[Header("Component")]
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [HideInInspector] public Animator animator => GetComponent<Animator>();
    protected SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    
    protected CapsuleCollider2D capsuleCollider => GetComponent<CapsuleCollider2D>();
    [HideInInspector] public PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();
    [HideInInspector] public Character character => GetComponent<Character>();
    
    [Header("Settings")] 
    public float normalSpeed = 10;
    public float chaseSpeed = 15;
    public float hurtForce = 5;
    public float waitTime = 1;
    public float lostTargetTime = 2;
    
    [Space(20)]
    public Vector2 centerOffset = new Vector2(0, 0.6f);
    public Vector2 checkSize = new Vector2(1, 1);
    public float checkDistance = 4.5f;
    public LayerMask playerLayer;
    
    [Header("Debug")]
    public Vector3 faceDir;
    [HideInInspector] public float currentSpeed;
    public Transform attacker;
    public bool isWait;
    public bool isHurt;
    public bool isDead;
    public bool hasTarget;

    public NPCState currentNPCState;
    public BaseState currentState;
    [HideInInspector] public BaseState patrolState;
    [HideInInspector] public BaseState chaseState;
    [HideInInspector] public BaseState skillState;
    
    [HideInInspector] public Timer waitTimer = new Timer();
    [HideInInspector] public Timer LostTargetTimer = new Timer();

    protected virtual void Awake()
    {
        currentSpeed = normalSpeed;
    }

    #region Event

    protected virtual void OnEnable()
    {
        // State
        currentState = patrolState;
        currentNPCState = NPCState.Patrol;
        currentState.OnEnter(this);
        
        // Timer
        waitTimer.timerStartEvent += FlipTimerStart; // prepare to turn back
        waitTimer.timerFinishEvent += FlipTimerFinish;// turn back
        
        LostTargetTimer.timerFinishEvent += LostTargetTimerFinish; // lost target
    }

    protected virtual void OnDisable()
    {
        // State
        currentState.OnExit();
        
        // Timer
        waitTimer.timerStartEvent -= FlipTimerStart;
        waitTimer.timerFinishEvent -= FlipTimerFinish;
        
        LostTargetTimer.timerFinishEvent -= LostTargetTimerFinish;
    }

    private void FlipTimerStart()
    {
        isWait = true;
        transform.localScale = new Vector3(faceDir.x, 1, 1);
        Debug.Log("S");
    }

    private void FlipTimerFinish()
    {
        isWait = false;
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
        
        // Check Player
        FoundPlayer();
        
        // left: -1, right: 1
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        //Check Lost Target
        if (hasTarget && !FoundPlayer() && !LostTargetTimer.isTiming)
        {
            LostTargetTimer.StartTimer(lostTargetTime);
        }
        else if (hasTarget && FoundPlayer() && LostTargetTimer.isTiming)
        {
            LostTargetTimer.StopTimer();

        }
    }
    protected virtual void FixedUpdate()
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
    public virtual bool FoundPlayer()
    {
        var result = Physics2D.BoxCast(transform.position + (Vector3)centerOffset + 
                                       new Vector3(checkDistance * -transform.localScale.x, 0), checkSize, 0,
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
            NPCState.Skill => skillState,
            _ => null
        };
        
        currentState.OnExit();
        currentState = newState;
        currentNPCState = state;
        currentState?.OnEnter(this);
    }

    
    #region Unity Event

    public virtual void OnTakeDamage(Transform attackTrans)
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
