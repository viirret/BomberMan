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
    Vector2 playerPosition2;
    
    Vector3 topView;
    Vector3 bottomView;
    Vector3 leftView;
    Vector3 rightView;
    int bombAmount = 0;
    float currentSpeed;
    Vector2 oldPosition;
    string lastDirection;

    Tile upTile;
    Tile downTile;
    Tile leftTile;
    Tile rightTile;

    // for the update
    bool getNewRandom = true;
    int dir;

    public void HitEnemy()
    {
        Debug.Log("Hit enemy");
        Player.AddScore(killReward);
        lives--;
        if(lives == 0)
            Destroy(obj);
    }

    // all the actions for enemy
    void MoveUp()
    {
        transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        lastDirection = "up";
    }
    void MoveDown()
    {
        transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        lastDirection = "down";
    }
    void MoveRight()
    {
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        lastDirection = "right";
    }
    void MoveLeft()
    {
        transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
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
        playerPosition2 = playerPosition;
        
        BirdMovement();

        upTile = targetTile(new Vector2(0, 0.28f), new Vector2(0, 1));
        downTile = targetTile(new Vector2(0, -0.28f), new Vector2(0, -1));
        leftTile = targetTile(new Vector2(-0.5f, 0), new Vector2(-1, 0));
        rightTile = targetTile(new Vector2(0.5f, 0), new Vector2(1, 0));

        if(upTile != null){Debug.Log("Uptile: " + upTile.name);}
        if(downTile != null){Debug.Log("Downtile: " + downTile.name);}
        if(leftTile != null){Debug.Log("Lefttile: " + leftTile.name);}
        if(rightTile != null){Debug.Log("Righttile: " + rightTile.name);}
       
    }

    Tile targetTile(Vector2 ownPosition, Vector2 lookingPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition);
            
        if(hit.collider != null)
        {
            Vector3Int target = GameMap.TilemapTop.WorldToCell(hit.point);
        
            Tile tile = GameMap.TilemapTop.GetTile<Tile>(target); 

            return tile;
        }

        return null;
    }

    void BirdMovement()
    {
        if(getNewRandom)
        {
            dir = Random.Range(1, 5);
            getNewRandom = false;
        }

        randomMovement(dir);
        if(currentSpeed == 0)
        {
            getNewRandom = true;
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

    void randomMovement(int num)
    {
        switch(num)
        {
            case 1: MoveUp(); break;
            case 2: MoveDown(); break;
            case 3: MoveLeft(); break;
            case 4: MoveRight(); break;
            default: break;
        }
    }

    IEnumerator WaitBomb()
    {
        yield return new WaitForSeconds(2);
        bombAmount--;
    }

}
