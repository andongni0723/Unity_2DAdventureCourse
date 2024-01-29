using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState<Bee>
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public override void OnEnter(Bee enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isWalk", true);
        currentEnemy.NewTargetPos();
    }


    public override void LogicUpdate()
    {
        if(currentEnemy.transform.position == currentEnemy.targetPos)
            currentEnemy.NewTargetPos();
        
        if(currentEnemy.FoundPlayer() && currentEnemy.isInPatrolRange)
            currentEnemy.SwitchState(NPCState.Chase);
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isWalk", false);
    }
}
