using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        var grid = new GameObject("Grid").AddComponent<Grid>();
        var tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
        tilemap.transform.SetParent(grid.gameObject);

        Tilemaps.TileBase tile; // Assign a tile asset to this.
        tilemap.SetTile(new Vector3Int(0,0,0), tile); // Or use SetTiles() for multiple tiles.
        */


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
