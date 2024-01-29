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
        // target is player
        currentEnemy.targetPos = currentEnemy.player.transform.position + 
                                 Vector3.right * -currentEnemy.faceDir.x + Vector3.up * 2;

        if(currentEnemy.transform.position == currentEnemy.targetPos)
            currentEnemy.animator.SetTrigger("attack");
        
        if (!currentEnemy.hasTarget || !currentEnemy.isInMoveArea)
            currentEnemy.SwitchState(NPCState.Patrol);
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isRun", false);
        Debug.Log("Exit");
    }
}
