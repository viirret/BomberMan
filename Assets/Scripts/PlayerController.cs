using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    // find the current position for player
    float speed = 2f;
    Tilemap tilemap;
    Tile wallTile;
    Tile destructibleTile;

    GameObject bombPrefab;

    public static Vector2 bombposition;
    void Start() 
    {
        tilemap = GameObject.Find("TilemapTop").GetComponent<Tilemap>();   
        wallTile = Resources.Load<Tile>("GameTiles/Wall");
        destructibleTile = Resources.Load<Tile>("GameTiles/Destructible");
        bombPrefab = Resources.Load<GameObject>("bomb");
    }

    void FixedUpdate() 
    {
        Vector3Int pos = tilemap.WorldToCell(transform.position);
        Tile currentTile = tilemap.GetTile<Tile>(pos);

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            bombposition = transform.position;
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
}