using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour 
{
    // this gameobject used in CreateBird in CharacterSpawner
    public GameObject obj;
    
    // attributes to the enemy
    public int direction;
    public float speed;
    public int blastRadius;
    public int bombsAtOnce;
    public int lives;
    public int killReward;
    public Vector3 position;
    public float currentSpeed;
    public GameObject bomb;
    
    // tiles next to enemy
    public Tile upTile;
    public Tile downTile;
    public Tile leftTile;
    public Tile rightTile;

     // all the states
    public IEnemyState currentState;
    public InitialState initialState;
    public NormalState normalState;
    public ChaseState chaseState;

   
    bool moveToPlayer = true;

    bool doOpposite = false;
    bool lookForSecond = false;
    Vector2 playerPosition2;
    int bombAmount = 0;
    Vector2 oldPosition;

    void Awake()
    {
        initialState = new InitialState(this);
        normalState = new NormalState(this);
        chaseState = new ChaseState(this);
    }

    void Start()
    {
        gameObject.tag = "Enemy";
        currentState = initialState;
    }

    public void HitEnemy()
    {
        Debug.Log("Hit enemy");
        Player.AddScore(killReward);
        lives--;
        if(lives == 0)
            Destroy(obj);
    }

    public Vector2 getLookingPosition(int dir)
    {
        switch(dir)
        {
            case 0: return new Vector2(0, 1);
            case 1: return new Vector2(0, -1);
            case 2: return new Vector2(-1, 0);
            case 3: return new Vector2(1, 0);
            default: return new Vector2(0, 0);
        }
    }

    public void DropBomb()
    {
        if(bombAmount < bombsAtOnce)
        {
            bombAmount++;
            bomb = new GameObject();
            Bomb b = bomb.AddComponent<Bomb>();
            b.pos = transform.position;
            b.blastRadius = blastRadius;
            Destroy(bomb, 3);
            StartCoroutine(WaitBomb());
        }
    }
    public bool BombAlive() => bomb ? true : false;
    
    // return opposite direction from lastDirection
    public int OppositeDirection(int last) => last = (last == 0 || last == 2) ? ++last : --last;

    public void GoOpposite() => direction = OppositeDirection(direction);

    // see if there is any bombs in the vision
    public int BombVision()
    {
        var hits = new List<RaycastHit2D>(); 
        hits.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0, 0.5f)), new Vector2(0, 1), 5f));
        hits.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0, -0.5f)), new Vector2(0, -1), 5f));
        hits.Add(Physics2D.Raycast((playerPosition2 + new Vector2(-0.7f, 0)), new Vector2(-1, 0), 5f));
        hits.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0.7f, 0)), new Vector2(1, 0), 5f));

        for(int i = 0; i < 4; i++)
            if(hits[i].collider != null)
                if(hits[i].collider.name == "bomb(Clone)")
                    return i;
        return -1;
    }

    
    // random free direction
    public int RandomRoute()
    {
        var routes = new List<int?>(4);
        routes.Add((upTile == null) ? 0 : (int?)null);
        routes.Add((downTile == null) ? 1 : (int?)null);
        routes.Add((leftTile == null) ? 2 : (int?)null);
        routes.Add((rightTile == null) ? 3 : (int?)null);

        return Choose(routes);
    }


    public bool DestructibleNear()
    {
        if  ((upTile != null && upTile.name == "Destructible") ||
            (downTile != null && downTile.name == "Destructible") ||
            (leftTile != null && leftTile.name == "Destructible") ||
            (rightTile != null && rightTile.name == "Destructible"))
        {
            return true; 
        }
        else
            return false;
    }
    public void GoPrimaryDirection()
    {
        direction = GoTowardsPlayer(true);
    }

    public void GoSecondaryDirection()
    {
        direction = GoTowardsPlayer(false);
    }

    // see if there is space on the primary direction of the player
    public bool PlayerDirectionEmpty()
    {
        return LookDirection(GoTowardsPlayer(true), true);
    }

    // see if there is space in the secondary direction of the player
    public bool PlayerSecondaryDirectionEmpty()
    {
        return LookDirection(GoTowardsPlayer(false), true);
    }

    public void GoRightOrLeft()
    {
        bool a = false, b = false;
        int num = Random.Range(0, 1);

        if(direction == 0 || direction == 1)
        {
            if(LookDirection(2, true)) a = true;
            if(LookDirection(3, true)) b = true;

            if(a && b) direction = (num == 0) ? 2 : 3;
            if(a) direction = 2;
            if(b) direction = 3;
        }
        else
        {
            if(LookDirection(0, true)) a = true;
            if(LookDirection(1, true)) b = true;

            if(a && b) direction = (num == 0) ? 0 : 1;
            if(a) direction = 0;
            if(b) direction = 1;
        }
    }

    public bool LookDirection(int dir, bool wall)
    {
        switch(dir)
        {
            case 0: return checkDirection(upTile, wall);
            case 1: return checkDirection(downTile, wall);
            case 2: return checkDirection(leftTile, wall);
            case 3: return checkDirection(rightTile, wall);
            
            default: return false;
        }
    }
    
    bool checkDirection(Tile tile, bool wall)
    {
        if(tile != null)
        {
            if(wall)
            {
                if(tile.name == "Wall" || tile.name == "Destructible")
                    return false;
            }
            else
            {
                if(tile.name == "Destructible")
                    return false;
            }
        }
        return true;
    }   

    int GoTowardsPlayer(bool x)
    {
        float dUp = PlayerController.playerPos.y - position.y;
        float dDown = -(PlayerController.playerPos.y - position.y); 
        float dLeft =  -(PlayerController.playerPos.x - position.x);
        float dRight =  PlayerController.playerPos.x - position.x;

        // find the largest value
        var largest = (dUp > dDown) ? (dUp > dLeft) ? (dUp > dRight) ? dUp 
        : dRight : (dLeft > dRight) ? dLeft : dRight : (dDown > dLeft) ? (dDown > dRight) 
        ? dDown : dRight : (dLeft > dRight) ? dLeft : dRight;

        if(x)
        {
            // move towards largest distance
            if (largest == dUp)
                return 0;
            else if (largest == dDown)
                return 1;
            else if (largest == dLeft)
                return 2;
            else if (largest == dRight)
                return 3;
            else
                return -1;
        }
        else
        {
            // get the second largest distance
            if(largest == dUp)
            {
                var sl = SecondLargest(dDown, dLeft, dRight);
                if(sl == dDown) return 1;
                else if(sl == dLeft) return 2;
                else return 3;
            }
            else if(largest == dDown)
            {
                var sl = SecondLargest(dUp, dLeft, dRight);
                if(sl == dUp) return 0;
                else if(sl == dLeft) return 2;
                else return 3;
            }
            else if(largest == dLeft)
            {
                var sl = SecondLargest(dUp, dDown, dRight);
                if(sl == dUp) return 0;
                else if(sl == dDown) return 1;
                else return 3;
            }
            else
            {
                var sl = SecondLargest(dUp, dDown, dLeft);
                if(sl == dUp) return 0;
                else if(sl == dDown) return 1;
                else return 2;
            }
        }
    }
    
    // return largest of three
    float SecondLargest(float a, float b, float c)
    {
        return a > b ? (a > c ? a : c) : (b > c ? b : c);
    }

    // DOES NOT WORK!
    /*
    int GetSecondLargest(float a, float b, float c)
    {
        var sl = SecondLargest(a, b, c);
        if(sl == dUp) return 0;
        else if(sl == dDown) return 1;
        else if(sl == dLeft) return 2;
        else return 3;
    }
    */


    // choose random number from list
    int Choose(List<int?> list)
    {
        int rnd = Random.Range(0, 4);
        if(list[rnd] != null)
            return rnd;
        else
            return Choose(list);
    }

    Tile TargetTile(Vector2 ownPosition, Vector2 lookingPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition, 0.5f);
            
        if(hit.collider != null)
        {
            // getting the tile
            Vector3Int target = GameMap.TilemapTop.WorldToCell(hit.point);
            Tile tile = GameMap.TilemapTop.GetTile<Tile>(target);

            return tile;
        }
        return null;
    }

    IEnumerator WaitBomb()
    {
        yield return new WaitForSeconds(2);
        bombAmount--;
    }
    
    // enemy's movement functions
    void MoveUp()
    {
        transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
    }
    void MoveDown()
    {
        transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
    }
    void MoveLeft()
    {
        transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
    }
    void MoveRight()
    {
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
    }

    
    // update closest tiles
    void Tiles()
    {
        upTile = TargetTile(new Vector2(0, 0.5f), new Vector2(0, 1));
        downTile = TargetTile(new Vector2(0, -0.5f), new Vector2(0, -1));
        leftTile = TargetTile(new Vector2(-0.7f, 0), new Vector2(-1, 0));
        rightTile = TargetTile(new Vector2(0.7f, 0), new Vector2(1, 0));
    } 

    void FixedUpdate()
    {
        position = transform.position;
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
        playerPosition2 = position;

        Tiles();

        currentState.UpdateState();
        
        // main movement
        switch(direction)
        {
            case 0: MoveUp(); break;
            case 1: MoveDown(); break;
            case 2: MoveLeft(); break;
            case 3: MoveRight(); break;
        }
    }

} 