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
        //Debug.Log(enemy.direction);

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
                        Debug.Log("enemy is going right or left");
                    }
                }
            }

            // if there is destructible tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, false))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
                Debug.Log("going opposite direction");
            }

            // if there is any other tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, true))
            {
                Debug.Log("going going left or right direction");
                enemy.GoRightOrLeft();
            }

            // if enemy sees other enemy
            if(enemy.SeeOtherEnemy() == enemy.direction)
            {
                Debug.Log("I see other enemy"); 
                enemy.GoOpposite();
            }

        }
        
        // if bomb straigh ahead
        else if(enemy.BombVision() == enemy.direction)
        {
            Debug.Log("I see bomb");
            enemy.GoOpposite();
        }
 
        // player straight ahead of player
        else if(enemy.SeePlayer() == enemy.direction && enemy.BombVision() != enemy.OppositeDirection(enemy.direction))
        {
            Debug.Log("I see the player");
            enemy.GoOpposite();
        }
        
        // the player is free
        else
        {
            Debug.Log("I am free");
            if(enemy.DestructibleLeftOrRight(enemy.direction))
            {
                enemy.DropBomb();
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
