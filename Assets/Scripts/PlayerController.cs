using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    // find the current position for player
    private float speed = 2f;
    void Update() 
    {
        if(Input.GetKey(KeyCode.W))
        {
            // this works. Check the Tiles
            transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        }

        /*
        if(Input.GetKey(KeyCode.Space))
        {
            MapDestroyer.Explode();
        }
        */





    }


}
