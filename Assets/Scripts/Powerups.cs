using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Powerups : MonoBehaviour 
{
    static GameObject extraLife;
    Tilemap tilemap;

    void Start()
    {
        extraLife = Resources.Load("1-up-powerup") as GameObject;
        tilemap = GameMap.TilemapTop;
    }

    public static void CreatePowerUps()
    {

        //extraLife = Instantiate(extraLife, new Vector3(2, 3, 0), Quaternion.identity);
        
        int num = Random.Range(10,50);
        //Debug.Log(num); 
    }    
}
