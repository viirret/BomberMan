using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    public GameObject obj;
    public float speed;
    public int blastRadius;
    public int bombsAtOnce;
    public int lives;
    public static Vector3Int playerPosition;
    void Start()
    {
        Debug.Log(playerPosition);
    }

    void FixedUpdate()
    {
        playerPosition = GameMap.TilemapTop.WorldToCell(transform.position);
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        
    }

    public void RemoveLife()
    {
        /* 
        lives -= 1;
        if(lives == 0)
        {
            Destroy(transform.parent.gameObject);
        }
        */
        Debug.Log("Hit enemy");
        Destroy(obj);
    }
}
