using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy<Boar>
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    protected override void Awake()
    {
        base.Awake();
        enemyType = this; 
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
}
