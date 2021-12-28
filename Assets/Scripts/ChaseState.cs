using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;

    bool primary = false;
    bool secondary = false;

    bool rightleft = true;

    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    { 
        HuntPlayer();
    }

    void HuntPlayer()
    {
        if(primary)
        {
            Primary();
        }

        if(secondary)
        {
            Secondary();
        }
           
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

    void Primary()
    {
        enemy.GoPrimaryDirection();
    }

    void Secondary()
    {
        enemy.GoSecondaryDirection();
    }

    public void ToNormalState()
    {
    }

    public void ToInitialState() {}
    public void ToChaseState() {}
}
