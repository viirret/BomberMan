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
        
        /*
        // if bomb straight ahead
        if(enemy.BombVision(5f) == enemy.direction)
        {
            enemy.GoOpposite();
        }

        if(enemy.SeeOtherEnemy(2f) == enemy.direction)
        {
            Debug.Log("I see other player");
            enemy.GoOpposite();
        }
 
        // player straight ahead of player
        if(enemy.SeePlayer(5f) == enemy.direction)
        {
            Debug.Log("I see player");
            enemy.GoOpposite();
        }
        */
        
        if(enemy.DestructibleLeftOrRight(enemy.direction))
        {
            if(enemy.LookDirection(enemy.direction, false, false))
            {
                if(!enemy.BombAlive())
                {
                    enemy.DropBomb();
                    enemy.GoOpposite();
                }
            }
            else if(enemy.LookDirection(enemy.direction, true, false))
            {
                if(!enemy.BombAlive())
                {
                    enemy.DropBomb();
                    enemy.GoOpposite();
                }
            }
            else
            {
                enemy.DropBomb();
            }
        }

        // if enemy sees powerup
        if(enemy.SeePowerUp(5f) != -1)
        {
            Debug.Log("enemy sees powerup");
            enemy.direction = enemy.SeePowerUp(5f);
        }
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
    public void ToInitialState() {}
    public void ToNormalState() {}
}
