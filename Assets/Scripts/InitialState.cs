using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : IEnemyState
{   
    EnemyController enemy;

    public InitialState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        // when to update enemy to other state
    }

    public void ToNormalState()
    {
        
    }

    public void ToChaseState()
    {

    }

    public void ToInitialState() {}

}