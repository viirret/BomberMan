using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : IEnemyState
{   
    EnemyController enemy;
    bool initialMovement = true;
    bool nextMovement = true;
    int dir;
    bool drop = true;
    bool checker = true;

    bool nextFunction = false;
    bool doNextFunction = false;
    bool ready = false;
    bool nextFunctionDone = false;

    public InitialState(EnemyController enemy)
    {
        this.enemy = enemy;
        Debug.Log("in inial state");
    }

    public void UpdateState()
    {
        MoveToDestructible();
        if(doNextFunction)
            NextMovement(dir);
        
        if(ready)
            ToChaseState();
    }


    void MoveToDestructible()
    {
        if(initialMovement)
        {
            initialMovement = false;
            dir = enemy.RandomRoute();
            enemy.direction = dir;
        }
        if(enemy.currentSpeed == 0)
        {
            // There is odd edge-case where the spawn fails,
            // and the bird thinks it has three directions instead of just two. 
            // With this checker we make sure the bird actually leaves spawn.
    
            if(checker)
            {
                if(enemy.DestructibleNear())
                {
                    if(drop)
                    {
                        enemy.DropBomb();
                        drop = false;
                        checker = false;
                        enemy.GoOpposite();
                        nextFunction = true;
                    }
                }
                else
                    initialMovement = true;
            }
        }
        if(enemy.currentSpeed == 0 && !enemy.DestructibleNear() && nextFunction)
        {
            nextFunction = false;
            doNextFunction = true;
        }
    }

    void NextMovement(int x)
    {
        if(nextMovement)
        {
            nextMovement = false;
            switch(x)
            {
                case 0: enemy.direction = (enemy.leftTile == null) ? 2 : 3; break;
                case 1: enemy.direction = (enemy.leftTile == null) ? 2 : 3; break;
                case 2: enemy.direction = (enemy.downTile == null) ? 1 : 0; break;
                case 3: enemy.direction = (enemy.downTile == null) ? 1 : 0; break;
            }
            nextFunctionDone = true;
        }

        // start other state if enemy is in correct place
        if(nextFunctionDone)
        {
            if(enemy.currentSpeed == 0)
                if(enemy.DestructibleNear())
                {
                    if(!enemy.BombAlive())
                        ready = true;
                }
                else
                    nextMovement = true;
        }
    }


    public void ToNormalState()
    {
        enemy.currentState = enemy.normalState;
    }

    public void ToChaseState() 
    {
        enemy.currentState = enemy.chaseState;
    }
    public void ToInitialState() {}

}