using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector2 pos;
    float countdown = 2f;
    GameObject bombPrefab;
    GameObject gm; 
    MapDestroyer mapdes;
    void Start()
    {
        bombPrefab = Resources.Load<GameObject>("bomb");
        // get elements from scene
        gm = GameObject.Find("Grid");
        mapdes = gm.GetComponent<MapDestroyer>();
    }
    void Update()
    {
        countdown -= Time.deltaTime;
        // getting the position where player plants the bomb
        pos = PlayerController.bombposition;

        if(countdown <= 0f)
        {
            mapdes.Explode(pos);
            Debug.Log("Explosion!");
            Destroy(gameObject);
            // decrease the amount of bombs currently played
            PlayerController.bombAmount--;
        }
    }
}
