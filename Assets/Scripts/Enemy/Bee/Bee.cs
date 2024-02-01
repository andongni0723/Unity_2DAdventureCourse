using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bee : Enemy<Bee>
{
    //[Header("Component")]
    [Header("Settings")]
    public float patrolRange;
    public float chaseRange;
    public float attackTime = 2.3f;
    
    [Header("Debug")]
    public bool isInMoveArea;
    public bool isInPatrolArea;
    public bool isAttackWait;
    public Vector3 originPos; 
    public Vector3 targetPos;
    public float playerDistance;
    public GameObject player => GameObject.FindWithTag("Player");
    
    [HideInInspector] public Timer checkFlipTimer = new Timer();
    [HideInInspector] public Timer attackTimer = new Timer();

    protected override void Awake()
    {
        base.Awake();
        enemyType = this; 
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
        currentState = patrolState;
        originPos = transform.position;
        
        checkFlipTimer.StartTimer(0);
    }

    #region Event

    protected override void OnEnable()
    {
        base.OnEnable();
        checkFlipTimer.timerFinishEvent += CheckFlipFinish;
        attackTimer.timerStartEvent += AttackTimerStart;
        attackTimer.timerFinishEvent += AttackTimerFinish;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        checkFlipTimer.timerFinishEvent -= CheckFlipFinish;
        attackTimer.timerStartEvent -= AttackTimerStart;
        attackTimer.timerFinishEvent -= AttackTimerFinish;
    }

    private void AttackTimerStart()
    {
        isAttackWait = true;
    }

    private void AttackTimerFinish()
    {
        isAttackWait = false;
    }

    private void CheckFlipFinish()
    {
        // Flip
        transform.localScale = MathF.Abs(targetPos.x) - MathF.Abs(transform.position.x) > 0?
            new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        checkFlipTimer.StartTimer(0.5f);
    }

    protected override void FlipTimerStart()
    {
        isWait = true;
    }

    protected override void FlipTimerFinish()
    {
        isWait = false;
        
        if(currentNPCState != NPCState.Chase)
            NewTargetPos();
    }

    #endregion 

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPos, patrolRange);
        Gizmos.DrawWireSphere(originPos, chaseRange);
    }

    protected override void Update()
    {
        base.Update();
        
        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // Check bee is in move range
        isInMoveArea = Mathf.Abs(transform.position.x - originPos.x) <= chaseRange &&
                        Mathf.Abs(transform.position.y - originPos.y) <= chaseRange;
        isInPatrolArea = Mathf.Abs(transform.position.x - originPos.x) <= patrolRange &&
                         Mathf.Abs(transform.position.y - originPos.y) <= patrolRange;
    }

    protected override void Move()
    {
        transform.position = 
            Vector3.MoveTowards(transform.position, targetPos , currentSpeed * Time.deltaTime);
    }

    public void NewTargetPos()
    {
        targetPos = originPos + new Vector3(Random.Range(-patrolRange, patrolRange), 
            Random.Range(-patrolRange, patrolRange), 0);
        
        transform.localScale = MathF.Abs(targetPos.x) - MathF.Abs(transform.position.x) > 0?
            new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }
}
