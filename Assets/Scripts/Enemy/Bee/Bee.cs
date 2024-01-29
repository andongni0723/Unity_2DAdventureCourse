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
    
    [Header("Debug")]
    public bool isInMoveArea;
    public bool isInPatrolArea;
    public Vector3 originPos; 
    public Vector3 targetPos;
    
    
    public GameObject player => GameObject.FindWithTag("Player");

    

    protected override void Awake()
    {
        base.Awake();
        enemyType = this; 
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
        currentState = patrolState;
        originPos = transform.position;
    }

    #region Event
    protected override void FlipTimerStart()
    {
        isWait = true;
        Debug.Log("S");
    }

    protected override void FlipTimerFinish()
    {
        isWait = false;
        
        if(currentNPCState != NPCState.Chase)
            NewTargetPos();
        
    }

    #endregion 

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPos, patrolRange);
        Gizmos.DrawWireSphere(originPos, chaseRange);
    }

    protected override void Update()
    {
        base.Update();
        
        // Flip
        transform.localScale = targetPos.x - transform.position.x > 0 ?
            new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

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
    }
}
