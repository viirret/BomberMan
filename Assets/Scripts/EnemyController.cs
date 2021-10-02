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
    
    int bombAmount = 0;
    float currentSpeed;
    Vector2 oldPosition;
    int lastDirection;
    // 1 -> up
    // 2 -> down
    // 3 -> left
    // 4 -> right


    Tile upTile;
    Tile downTile;
    Tile leftTile;
    Tile rightTile;

    // for the update
    bool seePlayer = false;
    bool getNewRandom = true;
    int dir;
    bool doRandom = true;

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
        lastDirection = 1;
    }
    void MoveDown()
    {
        transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        lastDirection = 2;
    }
    void MoveLeft()
    {
        transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
        lastDirection = 3;
    }
    void MoveRight()
    {
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        lastDirection = 4;
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

        upTile = TargetTile(new Vector2(0, 0.28f), new Vector2(0, 1));
        downTile = TargetTile(new Vector2(0, -0.28f), new Vector2(0, -1));
        leftTile = TargetTile(new Vector2(-0.5f, 0), new Vector2(-1, 0));
        rightTile = TargetTile(new Vector2(0.5f, 0), new Vector2(1, 0));

        //if(upTile != null){Debug.Log("Uptile: " + upTile.name);}
        //if(downTile != null){Debug.Log("Downtile: " + downTile.name);}
        //if(leftTile != null){Debug.Log("Lefttile: " + leftTile.name);}
        //if(rightTile != null){Debug.Log("Righttile: " + rightTile.name);}
       
    }

    void FreeDirections()
    {

    }

    Tile TargetTile(Vector2 ownPosition, Vector2 lookingPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition, 5);
            
        if(hit.collider != null)
        {
            if(hit.collider.name == "Blue Bird(Clone)")
            {
                seePlayer = true;
            }
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
        }

        if(seePlayer)
        {
            DropBomb();
            seePlayer = false;
        }

    }

    void OppositeDirection(int lastDirection)
    {
        switch(lastDirection)
        {
            case 1: MoveDown(); break;
            case 2: MoveUp(); break;
            case 3: MoveRight(); break;
            case 4: MoveLeft(); break;
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
