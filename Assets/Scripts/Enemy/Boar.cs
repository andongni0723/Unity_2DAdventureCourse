using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    protected override void Move()
    {
        base.Move();
        
        if(!isWait)
            animator.SetBool("isWalk", true);
    }
}
