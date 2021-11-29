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

    public InitialState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        MoveToDestructible();
        if(doNextFunction)
            NextMovement(dir);
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
            // and the bird thinks it has three directions insted of just two. 
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
                        enemy.direction = enemy.OppositeDirection(enemy.direction);
                        nextFunction = true;
                    }
                }
                else
                {
                    Debug.Log("FAILED SPAWN!");
                    initialMovement = true;
                }
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
        }

        // start normalstate if enemy is in correct place
        if(enemy.currentSpeed == 0)
        {
            if(enemy.DestructibleNear())
                ToNormalState();
            else
                nextMovement = true;
        }
    }


    public void ToNormalState()
    {
        enemy.currentState = enemy.normalState;
    }

    public void ToChaseState() {}
    public void ToInitialState() {}

}