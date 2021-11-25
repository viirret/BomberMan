using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IEnemyState
{
    EnemyController enemy;

    public NormalState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {

    }

    public void ToChaseState()
    {

    }
    public void ToInitialState() {}
    public void ToNormalState() {}
    
}
