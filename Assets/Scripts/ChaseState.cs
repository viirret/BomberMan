using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;
    
    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;   
    }

    public void UpdateState()
    {
        Debug.Log("in chase state");
    }

    public void ToNormalState()
    {
    }

    public void ToInitialState() {}
    public void ToChaseState() {}
}
