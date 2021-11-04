using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{
    public static List <GameObject> enemyControllers = new List<GameObject>();
    public int blastRadius;
    Tile wallTile;
    Tile destructibleTile;
    GameObject explosionPrefab;
    
    // Unity doesn't allow to destroy Prefab
    GameObject tempBomb;

    void Start()
    {
        wallTile = GameMap.Wall;
        destructibleTile = GameMap.Destructible;
        explosionPrefab = Resources.Load<GameObject>("Explosion");
    }

    // list of enemies
    public static void AddMe(GameObject obj) => enemyControllers.Add(obj);

    public void Explode(Vector2 worldPos)
    {
        Vector3Int origincell = GameMap.TilemapTop.WorldToCell(worldPos);

        ExplodeCell(origincell);

        // bomb explosion logic
        for(int i = 0; i < blastRadius; i++)
        {                
            if(ExplodeCell(origincell + new Vector3Int(i, 0, 0)))
                continue;
            else
                break;
        }
        for(int i = 0; i < blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int(-i, 0, 0)))
                continue;
            else
                break;
        }
        for(int i = 0; i < blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int(0, i, 0)))
                continue;
            else
                break;
        }
        for(int i = 0; i < blastRadius; i++)
        {
            if(ExplodeCell(origincell + new Vector3Int(0, -i, 0)))
                continue;
            else
                break;
        }
   }

    bool ExplodeCell(Vector3Int cell)
    {
        // getting the tile in position
        Tile tile = GameMap.TilemapTop.GetTile<Tile>(cell);

        // if hit player
        if(cell == PlayerController.playerPosition)
            Player.RemoveLife();

        // if hit any of the enemies
        for(int i = 0; i < enemyControllers.Count; i++)
            if(enemyControllers[i] != null)
                if(enemyControllers[i].GetComponent<EnemyController>().playerPosition == cell)
                    enemyControllers[i].GetComponent<EnemyController>().HitEnemy();

        // destoying a destructible tile
        if(tile == destructibleTile)
            GameMap.TilemapTop.SetTile(cell, null);

        // returning false if wall, and exploding wont't continue in this direction
        if(tile == wallTile)
            return false;
        

        // create the explosion effect
        Vector3 pos = GameMap.TilemapTop.GetCellCenterWorld(cell);
        tempBomb = Instantiate(explosionPrefab, pos, Quaternion.identity);
        Destroy(tempBomb, 1);
        return true;        
    }   
}