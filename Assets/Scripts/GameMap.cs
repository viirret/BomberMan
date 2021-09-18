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
        
        // creating grid and tilemaps
        var grid = new GameObject("Grid").AddComponent<Grid>();
        var go = new GameObject("Tilemap");
        var tm = go.AddComponent<Tilemap>();
        var tr = go.AddComponent<TilemapRenderer>();
        go.transform.SetParent(grid.transform);

        tm.SetTile(new Vector3Int(0,0,0), Wall); // Or use SetTiles() for multiple tiles.
        

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
