using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;

    bool primary = false;
    bool secondary = false;

    bool rightleft = true;

    bool dropper = true;
    
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
        
        if(enemy.currentSpeed == 0)
        {
            // see if there is bomb near by
            if(enemy.BombVision() != -1)
            {
                rightleft = true;

                // otherwise the enemy is already moving backwards from the bomb
                if(enemy.currentSpeed == 0)
                {
                    // if not space on the opposite direction of the bomb
                    if(!enemy.LookDirection(enemy.OppositeDirection(enemy.BombVision()), true))
                    {
                        if(rightleft)
                        {
                            enemy.GoRightOrLeft();
                            rightleft = false;
                            Debug.Log("Going either left or right");
                        }
                    }
                }
            }


            // if there is destructible tile in enemy's direction
            else if(!enemy.LookDirection(enemy.direction, false))
            {
                if(dropper)
                {
                    dropper = false;
                    enemy.DropBomb();
                    enemy.GoOpposite();
                    Debug.Log("going opposite direction");
                }
            }


            else
            {
                if(!enemy.DestructibleNear())
                {
                    primary = true;
                    Debug.Log("going primary direction");
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
