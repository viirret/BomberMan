using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed;
    GameObject bombPrefab;
    public static Vector2 bombposition;
    public static Vector3Int playerPosition;
    public static Vector3 playerPos;

    public static int bombAmount = 0;

    // vectors used in raycasting
    Vector2 lookingPosition = new Vector2(0, 0);
    Vector2 extraPosition = new Vector2(0, 0);

    void Start() 
    {
        speed = Player.speed;
    }

    void Update() 
    {
        // z value is -6 by default so changing to match to map
        Vector3Int playerPositionorig = GameMap.TilemapTop.WorldToCell(transform.position);
        playerPosition = playerPositionorig;
        playerPosition.z = 0;
        playerPos = transform.position;
    
        Vector2 playerPosition2 = playerPos;

        // movement
        if(Input.GetKey(KeyCode.W) || Input.GetKey("up"))
        {
            transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
            lookingPosition = new Vector2(0, 1);
            extraPosition = new Vector2(0, 0.28f);
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey("down"))
        {
            transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
            lookingPosition = new Vector2(0, -1);
            extraPosition = new Vector2(0, -0.3f);
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
            lookingPosition = new Vector2(-1, 0);
            extraPosition = new Vector2(-0.45f, 0);
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey("right"))
        {
            transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
            lookingPosition = new Vector2(1, 0);
            extraPosition = new Vector2(0.45f, 0);
        }
        
        // placing bomb
        if(Input.GetKeyDown(KeyCode.Space) && bombAmount < Player.bombsAtOnce)
        {
            bombAmount++;
            var bomb = new GameObject();
            Bomb b = bomb.AddComponent<Bomb>();
            b.pos = transform.position;
            b.blastRadius = Player.blastRadius;
            Destroy(bomb, 3);
            StartCoroutine(WaitBomb());
        }

        // check if wall in ongoing direction to stop vibrating
        RaycastHit2D vision = Physics2D.Raycast((playerPosition2 + extraPosition), lookingPosition, 0.1f);
        Vector3Int target = GameMap.TilemapTop.WorldToCell(vision.point);
        Tile tile = GameMap.TilemapTop.GetTile<Tile>(target);
        if(tile == GameMap.Wall)
        {
            transform.position += ReverseVector(lookingPosition) * speed * Time.deltaTime;
        }

        // for some reason the destructible tiles show wrong
        if(tile == GameMap.Destructible)
        {
        }
    }

    public void MoveToSpawn(Vector3 vec) => transform.position = vec;

    Vector3 ReverseVector(Vector2 vector)
    {
        int x =  -(int)(vector.x);
        int y =  -(int)(vector.y);
        return new Vector3(x, y);
    }

    IEnumerator WaitBomb()
    {
        yield return new WaitForSeconds(2);
        bombAmount--;
    }
}