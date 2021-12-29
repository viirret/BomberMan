using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;

    bool primary = false;
    bool secondary = false;

    bool rightleft = true;
    bool goPrimary = true;

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
            else if(!enemy.LookDirection(enemy.direction, false))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
            }

            // if there is any other tile in enemy's direction
            else if(!enemy.LookDirection(enemy.direction, true))
            {
                enemy.GoRightOrLeft();
            }

            // if enemy sees other enemy
            else if(enemy.SeeOtherEnemy() == enemy.direction)
            {
                enemy.GoOpposite();
            }

            // the enemy is free now
            else
            {
                goPrimary = true;
                Debug.Log("enemy going primary");
                primary = true;
                if(enemy.currentSpeed == 0)
                {
                    if(goPrimary)
                    {
                        enemy.GoOpposite();
                        primary = false;
                        goPrimary = false;
                    }
                }
            }

        }
        
        // if bomb straight ahead
        else if(enemy.BombVision() == enemy.direction)
        {
            enemy.GoOpposite();
        }

        // enemy sees the player
        else if(enemy.SeePlayer(0.25f) == enemy.direction)
        {
            enemy.DropBomb();
            enemy.GoOpposite();
        }
 
        // the player is free
        /*
        else
        {
            // if tile in enemy's direction
            if(!enemy.TileInDirection(enemy.direction))
            {
                Debug.Log("going primary");
                primary = true;
                if(enemy.currentSpeed == 0)
                {
                    
                    primary = false;
                    enemy.GoOpposite();
                }
            }
        }
        */
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
