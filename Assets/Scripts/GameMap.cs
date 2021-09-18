using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameMap : MonoBehaviour
{
    Tile Wall;
    Tile Dirt;
    Tile Destructible;
    void Start()
    {
        // loading tiles from resources
        Wall = Resources.Load<Tile>("GameTiles/Wall");
        Dirt = Resources.Load<Tile>("GameTiles/Dirt");
        Destructible = Resources.Load<Tile>("GameTiles/Destructible");
        
        // creating grid and bottom tilemap
        var grid = new GameObject("Grid").AddComponent<Grid>();
        var go1 = new GameObject("TilemapBase");
        var tm1 = go1.AddComponent<Tilemap>();
        var tr1 = go1.AddComponent<TilemapRenderer>();
        tr1.sortingOrder = 1;
        
        // creating top tilemap with colliders
        var go2 = new GameObject("TilemapTop");
        var tm2 = go2.AddComponent<Tilemap>();
        var tc2D = go2.AddComponent<TilemapCollider2D>();
        var cc2D = go2.AddComponent<CompositeCollider2D>();
        Rigidbody2D body = go2.GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        var tr2 = go2.AddComponent<TilemapRenderer>();
        tr2.sortingOrder = 2;

        go1.transform.SetParent(grid.transform);
        go2.transform.SetParent(grid.transform);

        // making one map for now
        SetBottonLayer();
        SetTopLayer();
    }

    void SetBottonLayer()
    {
        //tm1.SetTile(new Vector3Int(0,0,0), Wall);

    }

    void SetTopLayer()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
