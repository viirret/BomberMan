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
    public Tile extraDownTile;

    // tiles with some direction from the player
    public Tile dupTile;
    public Tile ddownTile;
    public Tile dleftTile;
    public Tile drightTile;


    // all the states
    public IEnemyState currentState;
    public InitialState initialState;
    public NormalState normalState;
    public ChaseState chaseState;

   
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

    // functions for powerups
    public void AddLife() => ++lives;
    public void AddSpeed(int amount) => speed += amount;
    public void AddBlastRadius() => ++blastRadius;
    

    public void HitEnemy()
    {
        Player.AddScore(killReward);
        lives--;
        if(lives == 0)
        {
            Game.enemies.Remove(obj);
            Destroy(obj);
        }
    }

    public void DropBomb()
    {
        if(bombAmount < bombsAtOnce)
        {
            bombAmount++;
            bomb = new GameObject();
            var b = bomb.AddComponent<Bomb>();
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
    public int BombVision(float distance)
    {
        var hits = new List<RaycastHit2D>(); 
        AddHits(hits, distance);

        for(int i = 0; i < hits.Count; i++)
            if(hits[i].collider)
                if(hits[i].collider.name == "bomb(Clone)")
                    return i;
        return -1;
    }

    // if other enemies in the vision
    public int SeeOtherEnemy(float distance)
    {
        var hits = new List<RaycastHit2D>();
        AddHits(hits, distance);

        for(int i = 0; i < hits.Count; i++)
            if(hits[i].collider)
                if(hits[i].collider.tag == "Enemy")
                    return i;
        return -1;
    }

    // if powerup in the vision
    public int SeePowerUp(float distance)
    {
        var hits = new List<RaycastHit2D>();
        AddHits(hits, distance);

        for(int i = 0; i < hits.Count; i++)
            if(hits[i].collider)
                if(hits[i].collider.tag == "powerup")
                    return i;
        return -1;
    }

    // player in the vision
    public int SeePlayer(float distance)
    {
        var hits = new List<RaycastHit2D>();
        AddHits(hits, distance);

        for(int i = 0; i < hits.Count; i++)
            if(hits[i].collider)
                if(hits[i].collider.name == "Blue Bird(Clone)")
                    return i;
        return -1;
    }

    // destructible tile left or right from the player
    public bool DestructibleLeftOrRight(int dir)
    {
        string s = "Destructible";
        switch(dir)
        {
            case 0: return (
            (leftTile != null && leftTile.name == s) || 
            (rightTile != null && rightTile.name == s));
            
            case 1: return (
            (leftTile != null && leftTile.name == s) || 
            (rightTile != null && rightTile.name == s));

            case 2: return (
            (upTile != null && upTile.name == s) || 
            (extraDownTile != null && extraDownTile.name == s));

            case 3: return (
            (upTile != null && upTile.name == s) ||
            (extraDownTile != null && extraDownTile.name == s));

            default: return false;
        }
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

    public void GoPrimaryDirection() => direction = GoTowardsPlayer(true);
    public void GoSecondaryDirection() => direction = GoTowardsPlayer(false);
    public int LargestDirection() => GoTowardsPlayer(true);
    public int SecondLargestDirection() => GoTowardsPlayer(false);



    public void GoRightOrLeft()
    {
        bool a = false, b = false;
        int num = Random.Range(0, 1);

        if(direction == 0 || direction == 1)
        {
            if(LookDirection(2, true, true)) a = true;
            if(LookDirection(3, true, true)) b = true;

            if(a && b) direction = (num == 0) ? 2 : 3;
            if(a) direction = 2;
            if(b) direction = 3;
        }
        else
        {
            if(LookDirection(0, true, true)) a = true;
            if(LookDirection(1, true, true)) b = true;

            if(a && b) direction = (num == 0) ? 0 : 1;
            if(a) direction = 0;
            if(b) direction = 1;
        }
    }

    public bool LookDirection(int dir, bool wall, bool close)
    {
        if(close) 
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
        else
        {
            switch(dir)
            {
                case 0: return checkDirection(dupTile, wall);
                case 1: return checkDirection(ddownTile, wall);
                case 2: return checkDirection(dleftTile, wall);
                case 3: return checkDirection(drightTile, wall);
                default: return false;
           }           
        }
    }

    bool checkDirection(Tile tile, bool wall)
    {
        if(tile != null)
        {
            if(wall)
            {
                // if something breaks with checkDirection this next line is probably the reason
                if(tile.name == "Wall")
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


    // choose random number from list
    int Choose(List<int?> list)
    {
        int rnd = Random.Range(0, 4);
        if(list[rnd] != null)
            return rnd;
        else
            return Choose(list);
    }
    
    void AddHits(List<RaycastHit2D> list, float distance)
    {
        list.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0.5f, 0)), new Vector2(0, 1), distance));
        list.Add(Physics2D.Raycast((playerPosition2 + new Vector2(-0.5f, 0)), new Vector2(0, -1), distance));
        list.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0, 0.5f)), new Vector2(-1, 0), distance));
        list.Add(Physics2D.Raycast((playerPosition2 + new Vector2(0, -0.5f)), new Vector2(1, 0), distance));
    }


    Tile TargetTile(Vector2 ownPosition, Vector2 lookingPosition, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition, distance);
            
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
        upTile = TargetTile(new Vector2(0, 0.7f), new Vector2(0, 1), 0.5f);
        downTile = TargetTile(new Vector2(0, -0.7f), new Vector2(0, -1), 0.5f);
        leftTile = TargetTile(new Vector2(-0.7f, 0), new Vector2(-1, 0), 0.5f);
        rightTile = TargetTile(new Vector2(0.7f, 0), new Vector2(1, 0), 0.5f);
        // enemy bird moves so close to the wall
        extraDownTile = TargetTile(new Vector2(0, -1.7f), new Vector2(0, -1), 0.5f);

        dupTile = TargetTile(new Vector2(0, 0.7f), new Vector2(0, 1), 2f);
        ddownTile = TargetTile(new Vector2(0, -0.7f), new Vector2(0, -1), 2f);
        dleftTile = TargetTile(new Vector2(-0.7f, 0), new Vector2(-1, 0), 2f);
        drightTile = TargetTile(new Vector2(0.7f, 0), new Vector2(1, 0), 2f);
    }

    void FixedUpdate()
    {
        position = transform.position;
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
        playerPosition2 = position;

        // update tiles
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