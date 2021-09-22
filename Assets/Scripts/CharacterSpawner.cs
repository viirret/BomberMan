using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterSpawner : MonoBehaviour
{
    GameObject blueBird;

    void Start()
    {
        Vector3[] spawnPoints = new Vector3[4];
        blueBird = Resources.Load<GameObject>("Blue bird");
        blueBird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
        CreateSpawnPoints(spawnPoints);
        
        GameObject player = CreateBird(blueBird, spawnPoints[0]);
        PlayerController pc = player.AddComponent<PlayerController>();

    }

    GameObject CreateBird(GameObject bird, Vector3 spawn)
    {
        GameObject obj;
        obj = Instantiate(bird, spawn, Quaternion.identity);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        Tilemap tm = obj.AddComponent<Tilemap>();
        Rigidbody2D b = obj.AddComponent<Rigidbody2D>();
        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        body.angularDrag = 0;
        body.gravityScale = 0;
        body.freezeRotation = true;
        BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
        return obj;
    }
    
    
    void Update()
    {
        
    }

    void CreateSpawnPoints(Vector3[] spawnPoints)
    {
        spawnPoints[0].x = -8;
        spawnPoints[0].y = -5.5f;
        spawnPoints[0].z = 0;

        spawnPoints[1].x = -8;
        spawnPoints[1].y = 4.5f;
        spawnPoints[1].z = 0;

        spawnPoints[2].x = 8;
        spawnPoints[2].y = -5.5f;
        spawnPoints[2].z = 0;

        spawnPoints[3].x = 8;
        spawnPoints[3].y = 4.5f;
        spawnPoints[3].z = 0;
    }
}
