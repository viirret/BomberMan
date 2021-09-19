using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{
    Tile wallTile;
    Tile destructibleTile;
    GameObject explosionPrefab;

    // Unity doesn't allow to destroy Prefab
    GameObject temp;
    
    void Start()
    {
        // defining components
        wallTile = GameMap.Wall;
        destructibleTile = GameMap.Destructible;
        explosionPrefab = Resources.Load<GameObject>("Explosion");
    }

    public void Explode(Vector2 worldPos)
    {
        Vector3Int origincell = GameMap.TilemapTop.WorldToCell(worldPos);

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
        Tile tile = GameMap.TilemapTop.GetTile<Tile>(cell);
 
        // fix this
        if(cell == PlayerController.playerPosition)
            Player.RemoveLife();
       
        if(tile == destructibleTile)
            GameMap.TilemapTop.SetTile(cell, null);

        if(tile == wallTile)
            return false;
        

        // create the explosion
        Vector3 pos = GameMap.TilemapTop.GetCellCenterWorld(cell);
        temp = Instantiate(explosionPrefab, pos, Quaternion.identity);
        Destroy(temp, 1);
        return true;        
    }   
}