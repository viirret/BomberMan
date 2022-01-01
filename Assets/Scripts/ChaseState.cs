using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;

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
        // every time enemy "stops"
        if(enemy.currentSpeed == 0)
        {
            // see if there is bomb near by
            if(enemy.BombVision(0.5f) != -1)
            {
                // if not space on the opposite direction of the bomb
                // meaning the enemy cannot go back from the bomb
                if(!enemy.LookDirection(enemy.OppositeDirection(enemy.BombVision(0.25f)), true, true))
                {
                    // escape the bomb
                    enemy.GoRightOrLeft();
                    Debug.Log("enemy escaping the bomb");
                }
            }

            // if there is destructible tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, false, true))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
                Debug.Log("destructible in enemy's way");
            }

            // if there is any other tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, true, true))
            {
                enemy.GoRightOrLeft();
                Debug.Log("any other tile in enemy's way");
            }

            // if enemy sees other enemy
            if(enemy.SeeOtherEnemy(0.25f) == enemy.direction)
            {
                enemy.GoOpposite();
                Debug.Log("enemy sees other enemy");
            }

            // see the player
            if(enemy.SeePlayer(0.25f) == enemy.direction)
            {
                enemy.DropBomb();
                enemy.GoOpposite();
            }
        }
        
        // if bomb straight ahead
        if(enemy.BombVision(0.5f) == enemy.direction)
        {
            enemy.GoOpposite();
            Debug.Log("Bomb straight ahead");
        }

        // if enemy straight ahead
        if(enemy.SeeOtherEnemy(0.25f) == enemy.direction)
        {
            Debug.Log("enemy straight ahead");
            enemy.GoOpposite();
        }
        
        // if player straight ahead
        if(enemy.SeePlayer(0.25f) == enemy.direction)
        {
            Debug.Log("player straight ahead");
            enemy.DropBomb();
            enemy.GoOpposite();
        }

        // following player
        /*
        if(!enemy.LookDirection(enemy.LargestDirection(), false))
        {
            enemy.GoPrimaryDirection();
        }
        */
        
        // no bombs in the main direction of the player
        if(!(enemy.BombVision(5f) == enemy.LargestDirection()))
        {
           // if not wall tiles in the way
           if(enemy.LookDirection(enemy.LargestDirection(), true, true))
           {
               // if there is destructible tiles on the way
               if(enemy.LookDirection(enemy.LargestDirection(), false, true))
               {
                   enemy.GoPrimaryDirection();
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
