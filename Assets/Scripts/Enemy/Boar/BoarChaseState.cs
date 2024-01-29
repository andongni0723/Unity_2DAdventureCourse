using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BoarChaseState : BaseState<Boar>
{
    
    public override void OnEnter(Boar enemy)
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
        if (!currentEnemy.hasTarget)
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
