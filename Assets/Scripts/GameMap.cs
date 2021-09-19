using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameMap : MonoBehaviour
{
    public static Tile Wall;
    public static Tile Dirt;
    public static Tile Destructible;
    public static Tilemap TilemapTop;
    public static Tilemap TilemapBottom;
    void Start()
    {
        // loading tiles from resources
        Wall = Resources.Load<Tile>("GameTiles/Wall");
        Dirt = Resources.Load<Tile>("GameTiles/Dirt");
        Destructible = Resources.Load<Tile>("GameTiles/Destructible");
        
        // creating grid and bottom tilemap
        var g = new GameObject("Grid");
        var grid = g.AddComponent<Grid>();
        var mapdes = g.AddComponent<MapDestroyer>();
        var go1 = new GameObject("TilemapBase");
        var tm1 = go1.AddComponent<Tilemap>();
        tm1.transform.position = new Vector3(0.5f, 0, 0);
        var tr1 = go1.AddComponent<TilemapRenderer>();
        tr1.sortingOrder = 1;
        TilemapBottom = go1.GetComponent<Tilemap>();

        // creating top tilemap with colliders
        var go2 = new GameObject("TilemapTop");
        var tm2 = go2.AddComponent<Tilemap>();
        tm2.transform.position = new Vector3(0.5f, 0, 0);
        var tc2D = go2.AddComponent<TilemapCollider2D>();
        var cc2D = go2.AddComponent<CompositeCollider2D>();
        Rigidbody2D body = go2.GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        var tr2 = go2.AddComponent<TilemapRenderer>();
        tr2.sortingOrder = 2;
        TilemapTop = go2.GetComponent<Tilemap>();

        go1.transform.SetParent(grid.transform);
        go2.transform.SetParent(grid.transform);

        // making one map for now
        SetBottonLayer(tm1);
        SetTopLayer(tm2);
    }

    void SetBottonLayer(Tilemap t)
    {
        for(int i = -9; i < 8; i++)
            for(int j = -6; j < 5; j++)
                t.SetTile(new Vector3Int(i, j, 0), Dirt);

    }

    void SetTopLayer(Tilemap t)
    {
        // WALL TILES

        // up and down row
        for(int i = -10; i < 9; i++)
        {
            t.SetTile(new Vector3Int(i,5,0), Wall);
            t.SetTile(new Vector3Int(i,-7,0), Wall);
        }

        // left and right column
        for(int i = -6; i < 5; i++)
        {
            t.SetTile(new Vector3Int(-10, i, 0), Wall);
            t.SetTile(new Vector3Int(8, i, 0), Wall);
        }
        
        // single wall tiles in bottom layer
        for(int i = -9; i < 7; i++)
            if(i % 2 == 0)
                for(int j = -5; j < 4; j++)
                    if(j % 2 != 0)
                        t.SetTile(new Vector3Int(i, j, 0), Wall);
        
        
        //  DESTRUCTIBLE TILES
        // first two and last two rows
        for(int i = -7; i < 6; i++)
        {
            t.SetTile(new Vector3Int(i, 4, 0), Destructible);
            t.SetTile(new Vector3Int(i, -6, 0), Destructible);
            if(i % 2 != 0)
            {
                t.SetTile(new Vector3Int(i, 3, 0), Destructible);
                t.SetTile(new Vector3Int(i, -5, 9), Destructible);
            }
        }

        // middle full rows
        for(int i = -9; i < 8; i++)
            for(int j = -4; j < 3; j++)
                if(j % 2 == 0)
                    t.SetTile(new Vector3Int(i, j, 0), Destructible);

        // single tiles in the middle
        for(int i = -9; i < 9; i++)
        {
            if(i % 2 != 0)
            {
                t.SetTile(new Vector3Int(i, 1, 0), Destructible);
                t.SetTile(new Vector3Int(i, -1, 0), Destructible);
                t.SetTile(new Vector3Int(i, -3, 0), Destructible);
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
