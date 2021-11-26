using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : IEnemyState
{   
    EnemyController enemy;
    int droppedBombs;
    bool initialMovement;
    bool goBackOnce;
    bool nextMovement = true;
    int dir;
    
    bool drop1 = true;

    bool checker = true;
    bool failedSpawn = false;
    bool y = false;

    public InitialState(EnemyController enemy)
    {
        this.enemy = enemy;
        droppedBombs = 0;
        initialMovement = true;
        goBackOnce = true;
    }


    public void UpdateState()
    {
        if(MoveToDestructible())
        {
            if(BackToStart())
            {
                if(NextMovement(dir))
                {

                }
            }
        }
        

        // end the Initial state
        if(droppedBombs >= 2)
        {
            ToNormalState();
        }
    }

    bool BackToStart()
    {
        if(goBackOnce)
        {
            goBackOnce = false;
            if(enemy.currentSpeed == 0)
            {
                enemy.direction = enemy.OppositeDirection(enemy.direction);
            }
            return (enemy.currentSpeed == 0) ? true : false;
        }
        if(enemy.currentSpeed == 0)
        {
            y = true;
        }

        return (enemy.currentSpeed == 0) ? true : false;
    }

    bool NextMovement(int x)
    {
        if(nextMovement)
        {
            nextMovement = true;
            if(enemy.currentSpeed == 0)    
            {
                if(y)
                {
                    switch(x)
                    {
                        
                        case 0:
                        if(enemy.leftTile != null)
                        {
                            enemy.direction = 3;
                        }
                        else
                        {
                            enemy.direction = 2;
                        }
                        break;
                    }
                }
            }
        }

        return (enemy.currentSpeed == 0) ? true : false;
    }

    bool MoveToDestructible()
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
                    if(drop1)
                    {
                        enemy.DropBomb();
                        drop1 = false;
                        checker = false;
                    }
                }
                else
                {
                    Debug.Log("FAILED SPAWN!");
                    initialMovement = true;
                    failedSpawn = true;
                }
            }
            return true;
        }
        else
            return false;
        
    }

    public void ToNormalState()
    {
        enemy.currentState = enemy.normalState;
    }

    public void ToChaseState() {}
    public void ToInitialState() {}

}