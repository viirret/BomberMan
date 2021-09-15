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

    void Start() 
    {
        tilemap = GameObject.Find("TilemapTop").GetComponent<Tilemap>();   
        wallTile = Resources.Load<Tile>("GameTiles/Wall");
        destructibleTile = Resources.Load<Tile>("GameTiles/Destructible");
    }

    void Update() 
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
        
        //Vector2 newpos = new Vector2(pos.x, pos.y);
        
        //if(Input.GetKey(KeyCode.Space))
        //{
        //    MapDestroyer.Explode(currentTile);
        //}
        

    }

/*    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.name == "Wall")
        {
            Debug.Log("Hit Rock!");
            speed = 0;
        }
    }
*/
}