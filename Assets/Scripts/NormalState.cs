using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IEnemyState
{
    EnemyController enemy;
    bool seePowerup = false;
    bool seePlayer = false;

    public NormalState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        Play();

        /*
        // go to chase state if this is the last enemy
        if(Game.enemies.Count == 1)
            ToChaseState();

        // if any of the attributes is better
        if(enemy.blastRadius > Player.blastRadius)
        {
            seePowerup = false;
            ToChaseState();
        }
        if(enemy.bombsAtOnce > Player.bombsAtOnce)
        {
            seePowerup = false;
            ToChaseState();
        }
        if(enemy.speed > Player.speed)
        {
            seePowerup = false;
            ToChaseState();
        }
        */
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
        
        
        // if bomb straight ahead
        if(enemy.BombVision(5f) == enemy.direction)
        {
            enemy.GoOpposite();
        }

        // if enemy straight ahead
        if(enemy.SeeOtherEnemy(4f) == enemy.direction)
        {
            enemy.GoOpposite();
        }
 
        // player straight ahead of player
        if(enemy.SeePlayer(10f) == enemy.direction)
        {
            seePlayer = true;
            enemy.GoPrimaryDirection();
            
            // go drop bomb near player
            if(enemy.SeePlayer(0.5f) == enemy.direction)
            {
                seePlayer = false;
                enemy.DropBomb();
                enemy.GoOpposite();
            }
        }
        
        // drop bombs everywhere
        if(enemy.DestructibleLeftOrRight(enemy.direction))
        {
            if((!seePowerup) && (!seePlayer))
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
        }

        // if enemy sees powerup
        if(enemy.SeePowerUp(5f) != -1)
        {
            enemy.direction = enemy.SeePowerUp(5f);
            seePowerup = true;
        }

        // remove life from player if touching enemy
        if(enemy.GetComponent<BoxCollider2D>().IsTouching(Game.pc.GetComponent<BoxCollider2D>()))
            Player.RemoveLife();
    }

    public void ToChaseState() => enemy.currentState = enemy.chaseState;
    public void ToInitialState() {}
    public void ToNormalState() {}
}
