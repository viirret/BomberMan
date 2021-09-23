using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed;
    GameObject bombPrefab;
    public static Vector2 bombposition;
    static Vector3Int playerPositionorig;
    public static Vector3Int playerPosition;
    public static int bombAmount = 0;
    float destroyAfter = 2f;

    bool spawned = false;
    void Start() 
    {
        speed = Player.speed;
        bombPrefab = Resources.Load<GameObject>("bomb");
    }

    void FixedUpdate() 
    {
        // z value is -6 by default so changing to match to map
        playerPositionorig = GameMap.TilemapTop.WorldToCell(transform.position);
        playerPosition = playerPositionorig;
        playerPosition.z = 0;

        // movement
        if(Input.GetKey(KeyCode.W))
            transform.position += new Vector3(0, 1) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.S))
            transform.position += new Vector3(0, -1) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
            transform.position += new Vector3(-1, 0) * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D))
            transform.position += new Vector3(1, 0) * speed * Time.deltaTime;
        
        // placing bomb
        if(Input.GetKeyDown(KeyCode.Space) && bombAmount < Player.bombsAtOnce && spawned == false)
        {
            spawned = true;
            bombAmount++;
            var bomb = new GameObject();
            Bomb b = bomb.AddComponent<Bomb>();
            b.pos = transform.position;
/*
            destroyAfter -= Time.fixedDeltaTime;
            Debug.Log(destroyAfter);
            if(destroyAfter <= 0f)
            {
                Debug.Log("GameObject should be destroyed");
                Destroy(bomb);
                bombAmount--;
            }
            */
            WaitTwo(bomb);


        }
    }

    IEnumerator WaitTwo(GameObject bomb)
    {
        yield return new WaitForSeconds(2);
        Debug.Log("GameObject should be destroyed");
        Destroy(bomb);
        bombAmount--;
    }
}