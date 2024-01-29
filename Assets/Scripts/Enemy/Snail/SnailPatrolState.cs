using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BaseState<Snail>
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public override void OnEnter(Snail enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isWalk", true); 
    }

    public override void LogicUpdate()
    {
        bool isTouchWall = (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || 
                           (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0);
        
        // Prepare to turn back
        if ((isTouchWall || !currentEnemy.physicsCheck.isGround) && !currentEnemy.isWait)
        {
            currentEnemy.waitTimer.StartTimer(currentEnemy.waitTime);
            Debug.Log("Walk");
            currentEnemy.animator.SetBool("isWalk", false);  
        }
        else if (!currentEnemy.isWait)
        {
            currentEnemy.animator.SetBool("isWalk", true);
        }
        
        if (currentEnemy.FoundPlayer() || currentEnemy.isHurt)
        {
            currentEnemy.SwitchState(NPCState.Skill); 
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
