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
    public bool isInPatrolRange;
    public Vector3 originPos; 
    public Vector3 targetPos;
    
    
    private GameObject player => GameObject.FindWithTag("Player");

    protected override void OnEnable()
    {
        enemyType = this; 
        base.OnEnable();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPos, patrolRange);
        Gizmos.DrawWireSphere(originPos, chaseRange);
    }

    protected override void Awake()
    {
        
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
        currentState = patrolState;
        originPos = transform.position;
        
    }

    protected override void FixedUpdate()
    {
        // if(currentNPCState == NPCState.Patrol && transform.position == targetPos)
        // {
        //     targetPos = originPos + Vector3.one * Random.Range(0, patrolRange);
        // }
        // else if (currentNPCState == NPCState.Chase)
        // {
        //     targetPos = player.transform.position;
        // }
        
        base.FixedUpdate();
    }


    protected override void Move()
    {
        // targetPos = originPos + Vector3.one * Random.Range(0, patrolRange);
        Vector2.MoveTowards(transform.position, targetPos , currentSpeed);
    }

    public void NewTargetPos()
    {
        targetPos = originPos + Vector3.one * Random.Range(0, patrolRange);
    }
}
