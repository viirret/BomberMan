using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour 
{
    public IEnemyState currentState;
    public InitialState initialState;
    public NormalState normalState;
    public ChaseState chaseState;

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
    Tile upTile;
    Tile downTile;
    Tile leftTile;
    Tile rightTile;

    bool moveToPlayer = true;

    int direction = -1;
    bool doOpposite = false;
    bool lookForSecond = false;

    private void Awake()
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


    // all the actions for enemy
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
            StartCoroutine(WaitBomb(1));
        }
    }
    IEnumerator WaitBomb(int action)
    {
        yield return new WaitForSeconds(2);
        switch(action)
        {
            case 1: bombAmount--; break;
            case 2: if(currentSpeed == 0){moveToPlayer = true;} break;
            case 3: if(currentSpeed == 0){lookForSecond = true;} break;
            default: break;
        }
    }

    // return opposite direction from lastDirection
    int OppositeDirection(int last) => last = (last == 0 || last == 2) ? ++last : --last;


   // enemy vision to closest objects
    Tile TargetTile(Vector2 ownPosition, Vector2 lookingPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition, 0.5f);
            
        if(hit.collider != null)
        {
            if(hit.collider.name == "Blue Bird(Clone)")
            {
                DropBomb();
            }

            if(hit.collider.name == "bomb(Clone)")
            {
                //Debug.Log("I see my own bomb");
                // this doesn't recognize it's own bombs after deployment
                //direction = OppositeDirection(direction);
                //StartCoroutine(WaitBomb(0));
            }
            
            if(hit.transform.tag == "Enemy")
            {
                // if you see the enemy
                //direction = OppositeDirection(direction);
                //StartCoroutine(WaitBomb(0));
            }

            // getting the tile
            Vector3Int target = GameMap.TilemapTop.WorldToCell(hit.point);
            Tile tile = GameMap.TilemapTop.GetTile<Tile>(target);

            return tile;
        }
        return null;
    }

   // update closest tiles
    void Tiles()
    {
        upTile = TargetTile(new Vector2(0, 0.5f), new Vector2(0, 1));
        downTile = TargetTile(new Vector2(0, -0.5f), new Vector2(0, -1));
        leftTile = TargetTile(new Vector2(-0.7f, 0), new Vector2(-1, 0));
        rightTile = TargetTile(new Vector2(0.7f, 0), new Vector2(1, 0));
    } 

    
    // random free direction
    int RandomRoute()
    {
        List<int?> routes = new List<int?>(4);
        routes.Add((upTile == null) ? 0 : (int?)null);
        routes.Add((downTile == null) ? 1 : (int?)null);
        routes.Add((leftTile == null) ? 2 : (int?)null);
        routes.Add((rightTile == null) ? 3 : (int?)null);

        return Choose(routes);
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

    // check if closest tiles are destructible and act accordingly
    void StartLooking()
    {
        if  ((upTile != null && upTile.name == "Destructible") ||
            (downTile != null && downTile.name == "Destructible") ||
            (leftTile != null && leftTile.name == "Destructible") ||
            (rightTile != null && rightTile.name == "Destructible"))
        {
            DropBomb();
        }
    }

    void LookDirection(int dir)
    {
        switch(dir)
        {
            case 0: if(upTile != null && upTile.name == "Destructible") 
            {
                DropBomb();
                doOpposite = true;
                StartCoroutine(WaitBomb(2));
                //direction = OppositeDirection(direction);    
            }
            break;
            case 1: if(downTile != null && downTile.name == "Destructible")
            {
                DropBomb();
                doOpposite = true;
                StartCoroutine(WaitBomb(2));
                //direction = OppositeDirection(direction);
            }
            break;
            case 2: if(leftTile != null && leftTile.name == "Destructible")
            {
                DropBomb();
                doOpposite = true;
                StartCoroutine(WaitBomb(2));
                //direction = OppositeDirection(direction);
            }
            break;
            case 3: if(rightTile != null && rightTile.name == "Destructible")
            {
                DropBomb();
                doOpposite = true;
                StartCoroutine(WaitBomb(2));
                //direction = OppositeDirection(direction);    
            }
            break;
            default: break;
        }
    }
    

    int GoTowardsPlayer(bool x)
    {
        float dUp = PlayerController.playerPos.y - playerPosition.y;
        float dDown = -(PlayerController.playerPos.y - playerPosition.y); 
        float dLeft =  -(PlayerController.playerPos.x - playerPosition.x);
        float dRight =  PlayerController.playerPos.x - playerPosition.x;

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

    float SecondLargest(float a, float b, float c)
    {
        return (a > b && a > c) ? ((b > c) ? b : c) : ((b > c) ? ((a > c) ? a : c) : ((a > b) ? a : b));
    }

    void BirdMovement()
    {
        // testing only for one bird
        if(obj.name == "Eagle(Clone)")
        {
            if(moveToPlayer && !lookForSecond)
            {
                direction = GoTowardsPlayer(true);
            
                if(currentSpeed == 0)
                {
                    moveToPlayer = false;
                    if(currentSpeed == 0)
                        StartCoroutine(WaitBomb(3));
                }
            }
            else if(!moveToPlayer && !lookForSecond)
            {
                LookDirection(direction);

                if(doOpposite)
                {
                    direction = OppositeDirection(direction);
                    doOpposite = false;
                }
                // if enemy stands still for two seconds start loop again
                if(currentSpeed == 0)
                {
                    StartCoroutine(WaitBomb(2));
                }
                
            }
            
            else if(lookForSecond)
            {
                direction = GoTowardsPlayer(false);
                if(currentSpeed == 0)
                {
                    lookForSecond = false;
                }
            }
            else
            {
                direction = RandomRoute();
                if(currentSpeed == 0)
                {
                    moveToPlayer = true;
                }
            }
            

            // main movement
            switch(direction)
            {
                case 0: MoveUp(); break;
                case 1: MoveDown(); break;
                case 2: MoveLeft(); break;
                case 3: MoveRight(); break;
                default: break;
            }
                
        }

    }

    void FixedUpdate()
    {
        playerPosition = transform.position;
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
        playerPosition2 = playerPosition;

        currentState.UpdateState();
        
        //Tiles();

        //BirdMovement();
    }
}
// Idea for longer vision
/*
    bool LongVision(Vector2 ownPosition, Vector2 lookingPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerPosition2 + ownPosition), lookingPosition, 15f);
        if(hit.collider != null)
        {
            //Vector3Int target = GameMap.TilemapTop.WorldToCell(hit.point);
            //Tile tile = GameMap.TilemapTop.GetTile<Tile>(target);

            if(hit.collider.name == "bomb(Clone)")
                return true;
            //return tile;

            // see if this is the whole range or just this one place
            //if(tile == null)
            //    if(hit.collider.name == "bomb(Clone)")
                    //return true;
            //else
            //    return false;
            
        }
        return false;
        // dont know if this is needed
        //return null;
    }
    void Vision()
    {
        upVision = LongVision(new Vector2(0, 0.5f), new Vector2(0, 1));
        downVision = LongVision(new Vector2(0, -0.5f), new Vector2(0, -1));
        leftVision = LongVision(new Vector2(-0.7f, 0), new Vector2(-1, 0));
        rightVision = LongVision(new Vector2(0.7f, 0), new Vector2(1, 0));
    }
    */
 