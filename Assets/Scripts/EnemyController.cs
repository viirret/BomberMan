using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    public float speed;
    public int blastRadius;
    public int bombsAtOnce;
    public int lives;

    public Vector3Int playerPosition;

    void Start()
    {
    
    }
    void FixedUpdate()
    {
        playerPosition = GameMap.TilemapTop.WorldToCell(transform.position);
        transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
    }

    public void RemoveLife()
    {
        Debug.Log("Hit enemy");
        Player.AddScore(100);
    }
}
