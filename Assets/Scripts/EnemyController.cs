using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    public float speed;
    public int blastRadius;
    public int bombsAtOnce;
    public int lives;
    public static Vector3Int playerPosition;
    void Start()
    {

    }

    void FixedUpdate()
    {
        playerPosition = GameMap.TilemapTop.WorldToCell(transform.position);

    }

    public static void RemoveLife()
    { 
        lives -= 1;
        if(lives == 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
