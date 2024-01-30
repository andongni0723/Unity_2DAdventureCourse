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
                                 Vector3.right * -currentEnemy.faceDir.x + Vector3.up * 1.5f;

        // near player to attack
        if (currentEnemy.playerDistance < 2.5f && !currentEnemy.isAttackWait)
        {
            currentEnemy.animator.SetTrigger("attack");
            currentEnemy.attackTimer.StartTimer(currentEnemy.attackTime);
        }
        
        // Switch to patrol
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
