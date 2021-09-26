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

    bool level1 = true;

    void Start()
    {
        CreateSpawnPoints(spawnPoints);
        
        blueBird = Resources.Load<GameObject>("Blue bird");
        chicken = Resources.Load<GameObject>("Chicken");
        eagle = Resources.Load<GameObject>("Eagle");
        owl = Resources.Load<GameObject>("Owl");
        yellowBird = Resources.Load<GameObject>("Yellow Bird");
         
        // create the player first
        int spawn1 = Random.Range(0, 4);
        GameObject player = CreateBird(blueBird, spawnPoints[spawn1], false);
        spawnPoints.RemoveAt(spawn1);

        if(level1)
        {
            GameObject enemy1 = CreateBird(yellowBird, spawnPoints[0], true);
            GameObject ememy2 = CreateBird(yellowBird, spawnPoints[1], true);
            GameObject enemy3 = CreateBird(yellowBird, spawnPoints[2], true);
        }

    }

    // making the bird
    GameObject CreateBird(GameObject bird, Vector3 spawn, bool enemy)
    {
        GameObject obj;
        bird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
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
            MapDestroyer.AddMe(obj);
            EnemyController ec = obj.AddComponent<EnemyController>();
            EnemyController EC = obj.GetComponent<EnemyController>();
            
            switch(bird.name)
            {
                case "Yellow Bird":
                    EC.speed = 4f;
                    EC.blastRadius = 2;
                    EC.bombsAtOnce = 1;
                    EC.lives = 1;
                    EC.killReward = 100;
                break;
                // rest of the birds
                default: break;
            }
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
