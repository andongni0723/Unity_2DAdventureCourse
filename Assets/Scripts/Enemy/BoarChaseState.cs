using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("isRun", true);
    }

    public override void LogicUpdate()
    {
        bool isTouchWall = (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || 
                           (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0);
        
        // Prepare to turn back
        if ((isTouchWall || !currentEnemy.physicsCheck.isGround) && !currentEnemy.isWait)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
        
        // Check to Patrol
        if (currentEnemy.LostTargetTimer.isFinished)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isRun", false);
    }
}
