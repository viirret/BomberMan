using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;

    bool primary = false;
    bool secondary = false;
    bool rightleft = true;
    bool bombStraightAhead = false;

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
        primary = false;
        secondary = false;

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
                        Debug.Log("enemy escaping the bomb");
                    }
                }
            }

            // if there is destructible tile in enemy's direction
            else if(!enemy.LookDirection(enemy.direction, false))
            {
                primary = false;
                enemy.DropBomb();
                enemy.GoOpposite();
                Debug.Log("destructible in enemy's way");
            }

            // if there is any other tile in enemy's direction
            else if(!enemy.LookDirection(enemy.direction, true))
            {
                enemy.GoRightOrLeft();
                Debug.Log("any other tile in enemy's way");
            }

            // if enemy sees other enemy
            else if(enemy.SeeOtherEnemy() == enemy.direction)
            {
                enemy.GoOpposite();
                Debug.Log("enemy sees other enemy");
            }

            // enemy sees the player
            else if(enemy.SeePlayer(2.0f) == enemy.direction)
            {
                enemy.DropBomb();
                enemy.GoOpposite();
                Debug.Log("enemy runnning away from the player");
            }

            // the enemy has no options after stop
            else
            {
                Debug.Log("enemy has no options after stop");
            } 
        }
        
        // if bomb straight ahead
        else if(enemy.BombVision() == enemy.direction)
        {
            enemy.GoOpposite();
            Debug.Log("Bomb straight ahead");
            bombStraightAhead = true;
        }

        // enemy loses its vision to the bomb
        else if(enemy.BombVision() == -1)
        {
            bombStraightAhead = false;
        }

        // enemy sees the player
        // why this doesn't work
        else if(enemy.SeePlayer(2.25f) == enemy.direction)
        {
            enemy.DropBomb();
            enemy.GoOpposite();
            Debug.Log("enemy sees other enemy in movement");
        }

        // enemy sees destructible tile
        else if(!enemy.LookDirection(enemy.direction, false))
        {
            // if there is bomb ahead
            if(bombStraightAhead)
            {
                enemy.GoOpposite();
                enemy.DropBomb();
                bombStraightAhead = false;
                Debug.Log("going back from destructible");
            }
        }
        
        // same with normal wall except no planting of the bomb 
        else if(!enemy.LookDirection(enemy.direction, false))
        {
            if(bombStraightAhead)
            {
                enemy.GoOpposite();
                bombStraightAhead = false;
                Debug.Log("going back from normal tile");
            }
        }

        else
        {
            Debug.Log("going primary");
            primary = true;
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
