using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour 
{
    public GameObject obj;
    public float speed;
    public int blastRadius;
    public int bombsAtOnce;
    public int lives;
    public int killReward;
    public Vector3 playerPosition;
    
    Vector3 topView;
    Vector3 bottomView;
    Vector3 leftView;
    Vector3 rightView;
    int bombAmount = 0;
    float currentSpeed;
    Vector2 oldPosition;
    string lastDirection;

    bool randomOn = false;

    public void HitEnemy()
    {
        Debug.Log("Hit enemy");
        Player.AddScore(killReward);
        RemoveLife();
        if(lives == 0)
            Destroy(obj);
    }

    // all the actions for enemy
    void MoveUp()
    {
        transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        Debug.Log("moving up");
        lastDirection = "up";
    }
    void MoveDown()
    {
        transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        Debug.Log("moving down");
        lastDirection = "down";
    }
    void MoveRight()
    {
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        Debug.Log("moving right");
        lastDirection = "right";
    }
    void MoveLeft()
    {
        transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
        Debug.Log("moving left");
        lastDirection = "left";
    }
    void DropBomb()
    {
        if(bombAmount < bombsAtOnce)
        {
            bombAmount++;
            var bomb = new GameObject();
            Bomb b = bomb.AddComponent<Bomb>();
            b.pos = transform.position;
            b.blastRadius = blastRadius;
            Destroy(bomb, 3);
            StartCoroutine(WaitBomb());
        }
    }

    void FixedUpdate()
    {
        playerPosition = transform.position;
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
        
        // the view of the enemy
        topView = (playerPosition + new Vector3(0, 0.1f, 0));
        bottomView = (playerPosition + new Vector3(0, -0.1f, 0));
        rightView = (playerPosition + new Vector3(0.1f, 0, 0));
        leftView = (playerPosition + new Vector3(-0.1f, 0, 0));

        // tiles nearby enemy
        Tile topTile = GameMap.TilemapTop.GetTile<Tile>(Vector3Int.FloorToInt(topView));
        Tile bottomTile = GameMap.TilemapTop.GetTile<Tile>(Vector3Int.FloorToInt(bottomView));
        Tile rightTile = GameMap.TilemapTop.GetTile<Tile>(Vector3Int.FloorToInt(rightView));
        Tile leftTile = GameMap.TilemapTop.GetTile<Tile>(Vector3Int.FloorToInt(leftView));

        if(topTile == GameMap.Wall)
            Debug.Log("Wall ahead");
        if(bottomTile == GameMap.Wall)
            Debug.Log("Wall ahead");
        if(rightTile == GameMap.Wall)
            Debug.Log("Wall to the right");
        if(leftTile == GameMap.Wall)
            Debug.Log("Wall to the left");


        // main logic
        // just trying out stuff
        if(rightTile == GameMap.Destructible)
        {
            Debug.Log("enemy dropping bomb");
            if(bombAmount == 0)
            {
                DropBomb();
                randomOn = false;
            }
            else
            {
                OppositeDirection(lastDirection);
            }
        }
        else
        {
            if(!randomOn)
            {
                randomMovement();
                randomOn = true;
            }
        }
        
    }

    void OppositeDirection(string lastDirection)
    {
        switch(lastDirection)
        {
            case "up": MoveDown(); break;
            case "down": MoveUp(); break;
            case "left": MoveRight(); break;
            case "right": MoveLeft(); break;
            default: break;
        }
    }

    void randomMovement()
    {
        int dir = randonNum();
        switch(dir)
        {
            case 1: MoveUp(); break;
            case 2: MoveDown(); break;
            case 3: MoveLeft(); break;
            case 4: MoveRight(); break;
            default: break;
        }
    }

    static int randonNum() => Random.Range(1, 4); 

    void RemoveLife() => lives--;
    

    IEnumerator WaitBomb()
    {
        yield return new WaitForSeconds(2);
        bombAmount--;
    }

}
