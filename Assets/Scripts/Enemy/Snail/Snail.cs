using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    //[Header("Component")]
    //[Header("Settings")]
    [Header("Debug")]
    public bool isHide;
    
    protected override void Awake()
    {
        base.Awake();
        // State
        patrolState = new SnailPatrolState();
        skillState = new SnailSkillState(); 
        currentState = patrolState;
    }

    #region Event

    protected override void OnEnable()
    {
        base.OnEnable();
        LostTargetTimer.timerFinishEvent += HideEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LostTargetTimer.timerFinishEvent -= HideEnd;
    }

    private void HideEnd()
    {
        isHide = false;
    }

    #endregion 
    

    protected override void FixedUpdate()
    {
        // State
        currentState.PhysicsUpdate();
        
        // Move
        if(!isHurt && !isDead && !isWait && !isHide) 
            Move();
    }

    public override void OnTakeDamage(Transform attackTrans)
    {
        base.OnTakeDamage(attackTrans);
        isHide = true;
    }

    public override bool FoundPlayer()
    {
        var result = Physics2D.BoxCast(transform.position + (Vector3)centerOffset + 
                                       new Vector3(checkDistance * -transform.localScale.x, 0), checkSize, 0,
            faceDir, checkDistance, playerLayer);

        if (result)
        {
            hasTarget = true;
            isHide = true;
        }
            
        return result;
    }
}
