using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public static int enemyCount;
    // all the prefabs
    GameObject blueBird;
    GameObject chicken;
    GameObject eagle;
    GameObject owl;
    GameObject yellowBird;

    List<Vector3> spawnPoints = new List<Vector3>(4);
    List<AudioSource> gameSounds = new List<AudioSource>();
    PlayerController pc;
    
    AudioSource level1;
    AudioSource level2;
    AudioSource level3;
    AudioSource currentSong;
    Vector3 playerpos;
    void Start()
    {
        CreateSpawnPoints(spawnPoints);
        LoadMedia();
        PauseMenu.gameIsPaused = false; 

        // create player in random place
        int spwn = Random.Range(0, 3);
        CreateBird(blueBird, spawnPoints[spwn], false);
        spawnPoints.RemoveAt(spwn);
    }

    void Update()
    {
        if(Levels.StartNewLevel)
        {
            Levels.StartNewLevel = false;
            
            // move player to new spawn
            if(Levels.level != 1)
            {
                CreateSpawnPoints(spawnPoints);
                int spwn = Random.Range(0, 3);
                Vector3 moveTo = spawnPoints[spwn];
                pc.MoveToSpawn(moveTo);
                spawnPoints.RemoveAt(spwn);
            }
            
            // settings for current level
            switch(Levels.level)
            {
                case 1:
                Debug.Log("Level 1");
                Player.lives = 1;
                CreateBird(owl, spawnPoints[0], true);
                //CreateBird(owl, spawnPoints[1], true);
                //CreateBird(owl, spawnPoints[2], true);
                currentSong = level1;
                Powerups.CreatePowerUps();
                break;
                
                case 2:
                Debug.Log("Level 2");
                Player.lives = 1;
                CreateBird(owl, spawnPoints[0], true);
                //CreateBird(owl, spawnPoints[1], true);
                //CreateBird(owl, spawnPoints[2], true);
                level1.Stop();
                currentSong = level2;
                break;
                
                case 3:
                Debug.Log("Level 3");
                Player.lives = 1;
                CreateBird(owl, spawnPoints[0], true);
                //CreateBird(eagle, spawnPoints[1], true);
                //CreateBird(eagle, spawnPoints[2], true);
                level2.Stop();
                currentSong = level3;
                break;
                

                default: break;
            }
            currentSong.Play();
        }

        // handle music
        if(PauseMenu.gameIsPaused)
            currentSong.Pause();
        else
           currentSong.UnPause(); 

        // player dies
        if(Player.lives < 1)
            DeadCanvas.PlayWhenDead();
        
        // if all enemies are dead
        if(enemyCount < 1)
        {
            if(Levels.level == 3)
                Winner.Win();
            else   
                Levels.NewLevel();
        }
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
            enemyCount++;
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
                    EC.lives = 1;
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
            pc = obj.GetComponent<PlayerController>();
        }
    }

    void LoadMedia()
    {
        // load game music
        // could make list and loop through it later
        level1 = Audio.LoadSound("sounds/game0", "game", gameObject);
        level2 = Audio.LoadSound("sounds/game1", "game", gameObject);
        level3 = Audio.LoadSound("sounds/game2", "game", gameObject);
        level3.loop = true;
        level1.loop = true;
        level2.loop = true;

        // load bird prefabs
        owl = Resources.Load<GameObject>("Owl");
        eagle = Resources.Load<GameObject>("Eagle");
        chicken = Resources.Load<GameObject>("Chicken");
        blueBird = Resources.Load<GameObject>("Blue bird");
        yellowBird = Resources.Load<GameObject>("Yellow Bird");
    }
    
    // all the spawnpoints in the corners of the map
    void CreateSpawnPoints(List<Vector3> spawnPoints)
    {
        spawnPoints.Clear();
        spawnPoints.Add(new Vector3(-8, -5.5f, 0));
        spawnPoints.Add(new Vector3(-8, 4.5f, 0));
        spawnPoints.Add(new Vector3(8, -5.5f, 0));
        spawnPoints.Add(new Vector3(8, 4.5f, 0));
    }
}
