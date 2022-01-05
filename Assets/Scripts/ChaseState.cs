using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    EnemyController enemy;
    bool wallInDirection = false;
    bool destructibleInDirection = false;
    bool bombInDirection = false;
    bool enemyInDirection = false;

    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    { 
        HuntPlayer();

        // if not the last player
        if(!(Game.enemies.Count < 2))
        {
            // if all attributes are same or better
            if( enemy.blastRadius >= Player.blastRadius &&
                enemy.bombsAtOnce >= Player.bombsAtOnce &&
                enemy.speed >= Player.speed )
                {
                    ToNormalState();
                }
        }
    }

    void HuntPlayer()
    {
        // every time enemy "stops"
        if(enemy.currentSpeed == 0)
        {
            // if there is destructible tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, false, true))
            {
                enemy.DropBomb();
                enemy.GoOpposite();
                //Debug.Log("destructible in enemy's way");
            }

            // if there is any other tile in enemy's direction
            if(!enemy.LookDirection(enemy.direction, true, true))
            {
                enemy.GoRightOrLeft();
                //Debug.Log("any other tile in enemy's way");
            }

            // if enemy sees other enemy
            if(enemy.SeeOtherEnemy(0.25f) == enemy.direction)
            {
                enemy.GoOpposite();
                //Debug.Log("enemy sees other enemy");
            }

            // see the player
            if(enemy.SeePlayer(0.25f) == enemy.direction)
            {
                enemy.DropBomb();
                enemy.GoOpposite();
            }
        }



        // if enemy straight ahead
        if(enemy.SeeOtherEnemy(0.25f) == enemy.direction)
        {
            //Debug.Log("enemy straight ahead");
            enemy.GoOpposite();
        }
        
        // if player straight ahead
        if(enemy.SeePlayer(0.25f) == enemy.direction)
        {
            //Debug.Log("player straight ahead");
            enemy.DropBomb();
            enemy.GoOpposite();
        }

        
        // things in the main direction
        
        // if main direction is covered by wall, works!
        /*
        if(!enemy.LookDirection(enemy.LargestDirection(), true, true))
        {   
            wallInDirection = true;
        }
        else
        {
            wallInDirection = false;
        }

        // if main direction is covered by destructible
        if(!enemy.LookDirection(enemy.LargestDirection(), false, true))
        {
            destructibleInDirection = true;
        }
        else
        {
            destructibleInDirection = false;
        }
        */

        if(enemy.LookTile(enemy.LargestDirection(), 0.5f))
        {
            wallInDirection = true;
        }
        else
        {
            wallInDirection = false;
        }

        // no bomb in the main direction, this only works for little distance
        if((enemy.BombVision(5f) == enemy.LargestDirection()))
        {
            bombInDirection = true;
        }
        else
        {
            bombInDirection = false;
        }

        if(enemy.SeePlayer(5f) == enemy.LargestDirection())
        {
            enemyInDirection = true;
        }
        else
        {
            enemyInDirection = false;
        }
        
        //Debug.Log("Wall in direction: " + wallInDirection);
        //Debug.Log("Destructible in direction: " + destructibleInDirection);
        //Debug.Log("Bomb in direction: " + bombInDirection);
        Debug.Log(enemy.LookTile(enemy.LargestDirection(), 0.5f));

        
        // if nothing in front of the enemy towards the largest distance of the player
        
        
        if((!bombInDirection) && (wallInDirection) && (!enemyInDirection))
        {
            enemy.direction = enemy.LargestDirection();
            Debug.Log("going to largest direction");
        }
        
        
    }



    public void ToNormalState() => enemy.currentState = enemy.normalState;
    public void ToInitialState() {}
    public void ToChaseState() {}
}
