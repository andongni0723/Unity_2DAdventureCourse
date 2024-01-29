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
        // Prepare to go to target point
        if (currentEnemy.transform.position == currentEnemy.targetPos && !currentEnemy.isWait)
        {
            currentEnemy.waitTimer.StartTimer(currentEnemy.waitTime);
            Debug.Log("timer");
        }

        // Player in the patrol range to chase
        if (currentEnemy.FoundPlayer() && currentEnemy.isInPatrolArea)
        {
            currentEnemy.waitTimer.StopTimer();
            currentEnemy.isWait = false;
            currentEnemy.SwitchState(NPCState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isWalk", false);
    }
}
