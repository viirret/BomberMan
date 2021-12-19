using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    // all the prefabs
    GameObject blueBird;
    GameObject chicken;
    GameObject eagle;
    GameObject owl;
    GameObject yellowBird;

    List<Vector3> spawnPoints = new List<Vector3>(4);
    AudioSource level1;
    AudioSource currentSong;

    void Start()
    {
        CreateSpawnPoints(spawnPoints);
        LoadMedia();
        
        owl = Resources.Load<GameObject>("Owl");
        eagle = Resources.Load<GameObject>("Eagle");
        chicken = Resources.Load<GameObject>("Chicken");
        blueBird = Resources.Load<GameObject>("Blue bird");
        yellowBird = Resources.Load<GameObject>("Yellow Bird");
         
        // create the player first in random spawnpoint
        int spawn1 = Random.Range(0, 4);
        CreateBird(blueBird, spawnPoints[spawn1], false);
        spawnPoints.RemoveAt(spawn1);

        PauseMenu.gameIsPaused = false;
    }

    void Update()
    {
        if(Levels.StartNewLevel)
        {
            Levels.StartNewLevel = false;
            switch(Levels.GetCurrentLevel())
            {
                case 1:
                Debug.Log("Level 1");
                Player.lives = 1;
                CreateBird(owl, spawnPoints[0], true);
                //CreateBird(chicken, spawnPoints[1], true);
                //CreateBird(eagle, spawnPoints[2], true);
                currentSong = level1;
                currentSong.Play();
                Powerups.CreatePowerUps();
                break;
                case 2:
                CreateBird(eagle, spawnPoints[0], true);
                break;
                default: break;
            }   
        }

        if(PauseMenu.gameIsPaused)
            currentSong.Pause();
        else
           currentSong.UnPause(); 

        if(Player.lives < 1)
            DeadCanvas.PlayWhenDead();
    }

    void LoadMedia()
    {
        level1 = Audio.LoadSound("sounds/game", "game", gameObject);
        level1.loop = true;
        /*
        if(!mediaLoaded)
        {
            DontDestroyOnLoad(level1);
            mediaLoaded = true;
        }
        else
            Destroy(level1);
        */
    }

    // making the bird
    void CreateBird(GameObject bird, Vector3 spawn, bool enemy)
    {
        GameObject obj;
        bird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
        obj = Instantiate(bird, spawn, Quaternion.identity);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        Tilemap tm = obj.AddComponent<Tilemap>();
        Rigidbody2D b = obj.AddComponent<Rigidbody2D>();
        b.angularDrag = 0;
        b.gravityScale = 0;
        b.freezeRotation = true;
        BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
        if(enemy)
        {
            // hitting characters happens in Mapdestroyer
            MapDestroyer.AddMe(obj);
            // adding controller and values
            var EC = obj.AddComponent<EnemyController>().GetComponent<EnemyController>(); 
            EC.obj = obj;

            switch(bird.name)
            {
                case "Yellow Bird":
                    EC.speed = 4;
                    EC.blastRadius = 3;
                    EC.bombsAtOnce = 1;
                    EC.lives = 1;
                    EC.killReward = 100;
                break;
                case "Eagle":
                    EC.speed = 3;
                    EC.blastRadius = 2;
                    EC.bombsAtOnce = 1;
                    EC.lives = 10;
                    EC.killReward = 200;
                break;
                case "Owl":
                    EC.speed = 3;
                    EC.blastRadius = 3;
                    EC.bombsAtOnce = 1;
                    EC.lives = 5;
                    EC.killReward = 500;
                break;
                case "Chicken":
                    EC.speed = 6;
                    EC.blastRadius = 3;
                    EC.bombsAtOnce = 1;
                    EC.lives = 2;
                    EC.killReward = 200;
                break;
                default: break;
            }
        }
        else
        {
            obj.AddComponent<PlayerController>();
        }
    }
    
    
    // all the spawnpoints in the corners of the map
    void CreateSpawnPoints(List<Vector3> spawnPoints)
    {
        spawnPoints.Add(new Vector3(-8, -5.5f, 0));
        spawnPoints.Add(new Vector3(-8, 4.5f, 0));
        spawnPoints.Add(new Vector3(8, -5.5f, 0));
        spawnPoints.Add(new Vector3(8, 4.5f, 0));
    }
}
