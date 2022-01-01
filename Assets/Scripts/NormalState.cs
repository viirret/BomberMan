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
        Play();   
    }

    void Play()
    {
        // every time enemy "stops"
        if(enemy.currentSpeed == 0)
        {
            // see if there is bomb near by
            if(enemy.BombVision(5f) != -1)
            {
                // if not space on the opposite direction of the bomb
                // meaning enemy already going back
                if(!enemy.LookDirection(enemy.OppositeDirection(enemy.BombVision(5f)), true, true))
                {
                    // escape the bomb
                    enemy.GoRightOrLeft();
                }
            }

            // if there is destructible tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, false, true))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
            }

            // if there is any other tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, true, true))
            {
                enemy.GoRightOrLeft();
            }

            // if enemy sees other enemy
            if(enemy.SeeOtherEnemy(5f) == enemy.direction)
            {
                enemy.GoOpposite();
            }
        }
        
        // if bomb straight ahead
        if(enemy.BombVision(5f) == enemy.direction)
        {
            enemy.GoOpposite();
        }

        if(enemy.SeeOtherEnemy(2f) == enemy.direction)
        {
            enemy.GoOpposite();
        }
 
        // player straight ahead of player
        if(enemy.SeePlayer(5f) == enemy.direction)
        {
            enemy.GoOpposite();
        }
        
        // no tiles in front (with distance) 
        if(enemy.LookDirection(enemy.direction, true, false))
        {
            if(enemy.LookDirection(enemy.direction, false, false))
            {
                if(enemy.DestructibleLeftOrRight(enemy.direction))
                {
                    enemy.DropBomb();
                }
            }
        }
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
    public void ToInitialState() {}
    public void ToNormalState() {}
}
