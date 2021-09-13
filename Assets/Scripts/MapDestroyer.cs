using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{
    public Tilemap tilemap;

    public void Explode(Vector2 worldPos)
    {
        Vector3Int origincell = tilemap.WorldToCell(worldPos);

    }

    void ExplodeCell(Vector3Int cell)
    {
        TileBase tilebase = tilemap.GetTile(cell);
        // continue here
    }   
}
