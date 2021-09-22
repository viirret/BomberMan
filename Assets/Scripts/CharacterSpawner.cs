using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterSpawner : MonoBehaviour
{
    // all the prefabs
    GameObject blueBird;
    GameObject chicken;
    GameObject eagle;
    GameObject owl;
    GameObject yellowBird;

    List<Vector3> spawnPoints = new List<Vector3>(4);
    void Start()
    {
        CreateSpawnPoints(spawnPoints);
        
        blueBird = Resources.Load<GameObject>("Blue bird");
        blueBird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
        
        // create the player first
        int spawn1 = Random.Range(0, 4);
        GameObject player = CreateBird(blueBird, spawnPoints[spawn1], false);
        spawnPoints.RemoveAt(spawn1);
    }

    // making the bird
    GameObject CreateBird(GameObject bird, Vector3 spawn, bool enemy)
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
        if(enemy)
        {
            // add the enemy controller
        }
        else
        {
            PlayerController pc = obj.AddComponent<PlayerController>();
        }
        return obj;
    }
    
    
    void Update()
    {
        
    }

    void CreateSpawnPoints(List<Vector3> spawnPoints)
    {
        spawnPoints.Add(new Vector3(-8, -5.5f, 0));
        spawnPoints.Add(new Vector3(-8, 4.5f, 0));
        spawnPoints.Add(new Vector3(8, -5.5f, 0));
        spawnPoints.Add(new Vector3(8, 4.5f, 0));
    }
}
