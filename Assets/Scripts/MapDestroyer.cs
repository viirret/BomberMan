using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{
    Tilemap tilemap;
    Tile wallTile;
    Tile destructibleTile;
    GameObject explosionPrefab;

    // Unity doesn't allow to destroy Prefab
    GameObject temp;
    
    void Start()
    {
        // defining components
        tilemap = GameObject.Find("TilemapTop").GetComponent<Tilemap>();
        wallTile = Resources.Load<Tile>("GameTiles/Wall");
        destructibleTile = Resources.Load<Tile>("GameTiles/Destructible");
        explosionPrefab = Resources.Load<GameObject>("Explosion");
    }

    public void Explode(Vector2 worldPos)
    {
        Vector3Int origincell = tilemap.WorldToCell(worldPos);

        // standard values make these react to powerups
        ExplodeCell(origincell);
        if(ExplodeCell(origincell + new Vector3Int(1, 0, 0)))
        {
            ExplodeCell(origincell + new Vector3Int(2, 0, 0));
        }
        if(ExplodeCell(origincell + new Vector3Int(0, 1, 0)))
        {
            ExplodeCell(origincell + new Vector3Int(0, 2, 0));
        }
        if(ExplodeCell(origincell + new Vector3Int(-1, 0, 0)))
        {
            ExplodeCell(origincell + new Vector3Int(-2, 0, 0));
        }
        if(ExplodeCell(origincell + new Vector3Int(0, -1, 0)))
        {
            ExplodeCell(origincell + new Vector3Int(0, -2, 0));
        }
    }

    bool ExplodeCell(Vector3Int cell)
    {
        Tile tile = tilemap.GetTile<Tile>(cell);

        if(tile == wallTile)
            return false;

        if(tile == destructibleTile)
        {
            tilemap.SetTile(cell, null);
        }

        // create the explosion
        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        temp = Instantiate(explosionPrefab, pos, Quaternion.identity);
        Destroy(temp, 1);
        return true;        
    }   
}