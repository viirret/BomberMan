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

        // bomb explosion logic
        for(int i = 1; i < Player.blastRadius; i++)
        {                
            if(ExplodeCell(origincell + new Vector3Int(i, 0, 0)))
                continue;
            else
                break;
        }
        for(int i = 1; i < Player.blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int((0-i), 0, 0)))
                continue;
            else
                break;
        }
        for(int i = 1; i < Player.blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int(0, i, 0)))
                continue;
            else
                break;
        }
        for(int i = 1; i < Player.blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int(0, (0-i), 0)))
                continue;
            else
                break;
        }
   }

    bool ExplodeCell(Vector3Int cell)
    {
        Tile tile = tilemap.GetTile<Tile>(cell);

        if(tile == wallTile)
            return false;

        if(tile == destructibleTile)
            tilemap.SetTile(cell, null);

        if(cell == PlayerController.playerPosition)
            Player.RemoveLife();

        // create the explosion
        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        temp = Instantiate(explosionPrefab, pos, Quaternion.identity);
        Destroy(temp, 1);
        return true;        
    }   
}