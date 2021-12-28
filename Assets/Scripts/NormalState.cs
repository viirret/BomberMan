using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IEnemyState
{
    EnemyController enemy;
    bool rightleft = true;

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
            if(enemy.BombVision() != -1)
            {
                rightleft = true;

                // if not space on the opposite direction of the bomb
                // meaning enemy already going back
                if(!enemy.LookDirection(enemy.OppositeDirection(enemy.BombVision()), true))
                {
                    if(rightleft)
                    {
                        // escape the bomb
                        enemy.GoRightOrLeft();
                        rightleft = false;
                    }
                }
            }

            // if there is destructible tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, false))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
            }

            // if there is any other tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, true))
            {
                enemy.GoRightOrLeft();
            }

            // if enemy sees other enemy
            if(enemy.SeeOtherEnemy() == enemy.direction)
            {
                enemy.GoOpposite();
            }

        }
        
        // if bomb straigh ahead
        else if(enemy.BombVision() == enemy.direction)
        {
            enemy.GoOpposite();
        }
 
        // player straight ahead of player
        else if(enemy.SeePlayer() == enemy.direction && enemy.BombVision() != enemy.OppositeDirection(enemy.direction))
        {
            enemy.GoOpposite();
        }
        
        // the player is free
        else
        {
            if(enemy.TileInDirection(enemy.direction))
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
