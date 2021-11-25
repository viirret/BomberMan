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

    }

    public void ToNormalState()
    {

    }

    public void ToInitialState() {}
    public void ToChaseState() {}
}
