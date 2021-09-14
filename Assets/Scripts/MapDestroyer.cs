using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wallTile;
    public Tile destructibleTile;
    public GameObject explosionPrefab;

    // Unity doesn't allow to destroy Prefab
    GameObject temp;

    public void Explode(Vector2 worldPos)
    {
        Vector3Int origincell = tilemap.WorldToCell(worldPos);

        ExplodeCell(origincell);
    }

    void ExplodeCell(Vector3Int cell)
    {
        Tile tile = tilemap.GetTile<Tile>(cell);

        if(tile == wallTile)
            return;

        if(tile == destructibleTile)
        {
            tilemap.SetTile(cell, null);
        }

        // create the explosion
        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        temp = Instantiate(explosionPrefab, pos, Quaternion.identity);
        Destroy(temp, 1);
        
    }   
}
