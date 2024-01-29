using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState<Bee>
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public override void OnEnter(Bee enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("isRun", true);
    }

    public override void LogicUpdate()
    {
        currentEnemy.targetPos = currentEnemy.transform.position;

        if (!currentEnemy.hasTarget || !currentEnemy.isInPatrolRange)
            currentEnemy.SwitchState(NPCState.Patrol);
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isRun", false);
    }
}
