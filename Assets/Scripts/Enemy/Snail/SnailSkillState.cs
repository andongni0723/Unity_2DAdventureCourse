using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSkillState : BaseState<Snail>
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public override void OnEnter(Snail enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isHide", true);
        currentEnemy.character.isInvulnerable = true;
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.hasTarget)
        {
            currentEnemy.animator.SetTrigger("isWake");
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isHide", false);
        currentEnemy.character.isInvulnerable = false;
    }
}
