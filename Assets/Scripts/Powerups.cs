using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Powerups : MonoBehaviour 
{
    public static Powerups instance;
    public GameObject extraLife;
    GameObject obj;
    
    void Start()
    {
        instance = this;
    }

    public void CreatePowerUps()
    {
        obj = Instantiate(extraLife, new Vector3(2, 3, 0), Quaternion.identity);
    }    
}
