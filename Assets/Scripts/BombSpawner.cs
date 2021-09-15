using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
    Tilemap tilemap;

    GameObject bombPrefab;

    void Start() 
    {
        tilemap = GameObject.Find("TilemapTop").GetComponent<Tilemap>();
        bombPrefab = Resources.Load<GameObject>("bomb");
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);   
            Vector3Int cell = tilemap.WorldToCell(worldpos);
            Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cell);

            Instantiate(bombPrefab, cellCenterPos, Quaternion.identity);
        }        
    }
}
