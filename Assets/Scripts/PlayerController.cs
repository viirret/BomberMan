using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed;
    public static Vector3Int playerPosition;
    public static Vector3 playerPos;
    public static int bombAmount = 0;

    // vectors used in raycasting
    Vector2 lookingPosition = new Vector2(0, 0);
    Vector2 extraPosition = new Vector2(0, 0);

    void Update() 
    {
        speed = Player.speed;

        // z value is -6 by default so changing to match to map
        Vector3Int playerPositionorig = GameMap.TilemapTop.WorldToCell(transform.position);
        playerPosition = playerPositionorig;
        playerPosition.z = 0;
        playerPos = transform.position;
    
        Vector2 playerPosition2 = playerPos;

        // movement
        if(Input.GetKey(KeyCode.W) || Input.GetKey("up") || Input.GetKey(KeyCode.K))
        {
            transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
            lookingPosition = new Vector2(0, 1);
            extraPosition = new Vector2(0, 0.28f);
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey("down") || Input.GetKey(KeyCode.J))
        {
            transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
            lookingPosition = new Vector2(0, -1);
            extraPosition = new Vector2(0, -0.28f);
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey("left") || Input.GetKey(KeyCode.H))
        {
            transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
            lookingPosition = new Vector2(-1, 0);
            extraPosition = new Vector2(-0.45f, 0);
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey("right") || Input.GetKey(KeyCode.L))
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

        RaycastHit2D vision = Physics2D.Raycast((playerPosition2 + extraPosition), lookingPosition, 0.1f);
        Vector3Int target = GameMap.TilemapTop.WorldToCell(vision.point);
        Tile tile = GameMap.TilemapTop.GetTile<Tile>(target);

        // walltile pushes back the same amount that player pushes walltile,
        // so the vibration stops
        if(tile == GameMap.Wall)
        {
            transform.position += ReverseVector(lookingPosition) * speed * Time.deltaTime;
        }

        // I have no idea why this doesn't work, it totally breaks the whole game.
        // this is the reason player has some vibration with destructible tiles
        if(tile == GameMap.Destructible)
        {
        }
    }

    // move player to point
    public void MoveToSpawn(Vector3 vec) => transform.position = vec;

    // return reverse of the vector
    Vector3 ReverseVector(Vector2 vector) => new Vector3(-(int)vector.x, -(int)vector.y);

    IEnumerator WaitBomb()
    {
        yield return new WaitForSeconds(2);
        bombAmount--;
    }
}