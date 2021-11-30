using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IEnemyState
{
    EnemyController enemy;
    bool initialMovement = true;
    bool start = false;
    int dir;

    public NormalState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        StartNormal();
        
        if(start)
        {
            NormalGame();
        }
    }

    void NormalGame()
    {
        // if there is bomb near by
        if(enemy.BombVision() != -1)
        {
            // if there is space on the opposite direction of the bomb
            if(enemy.LookDirection(enemy.OppositeDirection(enemy.BombVision())))
            {
                Debug.Log("Space on the opposite direction");
                // go to opposite of the bomb
                enemy.direction = enemy.OppositeDirection(enemy.BombVision());
            }
            else
                enemy.direction = enemy.RandomRoute();
        }
        else
        {
            if(enemy.DestructibleNear())
            {
                enemy.DropBomb();
            }
            else
            {
                //ToChaseState();
            }
        }
    }

    // start doing the normal mode after the first bomb is exploded
    void StartNormal()
    {
        if(initialMovement)
            if(!enemy.BombAlive())
            {
                initialMovement = false;
                start = true;
            }
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
    public void ToInitialState() {}
    public void ToNormalState() {}
    
}
