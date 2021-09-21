using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Powerups : MonoBehaviour 
{
    GameObject extraLife;
    Tilemap tilemap;

    void Start()
    {
        extraLife = Resources.Load<GameObject>("1-up-powerup");
        tilemap = GameMap.TilemapTop;
    }

    public void CreatePowerUps()
    {
        int num = Random.Range(10,50);
        Debug.Log(num); 
    }    
}
