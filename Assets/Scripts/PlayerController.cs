using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed;
    Tile wallTile;
    Tile destructibleTile;
    GameObject bombPrefab;
    public static Vector2 bombposition;
    public static Vector3Int playerPosition;
    public static Vector3 PlayerPos;
    public static int bombAmount = 0;
    void Start() 
    {
        speed = Player.speed;
        wallTile = GameMap.Wall;
        destructibleTile = GameMap.Destructible;
        bombPrefab = Resources.Load<GameObject>("bomb");
    }

    void FixedUpdate() 
    {
        // get the position of the player
        playerPosition = GameMap.TilemapTop.WorldToCell(transform.position);
        //Debug.Log(playerPosition);
        
        PlayerPos = transform.position;
        // movement
        if(Input.GetKey(KeyCode.W))
            transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.S))
            transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
            transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D))
            transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        
        // placing bomb
        if(Input.GetKeyDown(KeyCode.Space) && bombAmount < Player.bombsAtOnce)
        {
            bombAmount++;
            bombposition = transform.position;
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
}