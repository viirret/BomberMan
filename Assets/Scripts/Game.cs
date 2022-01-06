using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public static List<GameObject> enemies = new List<GameObject>();
    // all the prefabs
    GameObject blueBird;
    GameObject chicken;
    GameObject eagle;
    GameObject owl;
    GameObject yellowBird;

    List<Vector3> spawnPoints = new List<Vector3>(4);
    PlayerController pc;
    
    AudioSource level1;
    AudioSource level2;
    AudioSource level3;
    AudioSource currentSong;
    BoxCollider2D player;

    void Awake()
    {
        CreateSpawnPoints(spawnPoints);
        LoadMedia();

        // initial settings
        PauseMenu.gameIsPaused = false;
        Time.timeScale = 1f;
        GameTexts.totalTime = 0;
        Levels.level = 1;
        Levels.StartNewLevel = true;
        Player.score = 0;
        Player.lives = 1;
        PlayerSettingsNormal();


        // create player in random place
        int spwn = Random.Range(0, 3);
        CreateBird(blueBird, spawnPoints[spwn], false);
        spawnPoints.RemoveAt(spwn);

        player = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(Levels.StartNewLevel)
        {
            Levels.StartNewLevel = false;
            ClearLevel();
            enemies.Clear();
            
            // move player to random spawn point
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
                PlayerSettingsNormal();
                Powerups.instance.CreatePowerUps();
                CreateBird(owl, spawnPoints[0], true);
                CreateBird(owl, spawnPoints[1], true);
                CreateBird(owl, spawnPoints[2], true);
                currentSong = level1;
                break;
                
                case 2:
                Debug.Log("Level 2");
                PlayerSettingsNormal();
                Powerups.instance.CreatePowerUps();
                CreateBird(owl, spawnPoints[0], true);
                //CreateBird(owl, spawnPoints[1], true);
                //CreateBird(owl, spawnPoints[2], true);
                level1.Stop();
                currentSong = level2;
                break;
                
                case 3:
                Debug.Log("Level 3");
                PlayerSettingsNormal();
                Powerups.instance.CreatePowerUps();
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
        if(enemies.Count < 1)
        {
            if(Levels.level == 3)
                Winner.Win();
            else   
                Levels.NewLevel();
        }

        // handling powerups
        HandlePowerUps();
        
    }

    // making the bird
    void CreateBird(GameObject bird, Vector3 spawn, bool enemy)
    {
        GameObject obj;
        bird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
        obj = Instantiate(bird, spawn, Quaternion.identity);
        obj.AddComponent<BoxCollider2D>();
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        Rigidbody2D b = obj.AddComponent<Rigidbody2D>();
        b.angularDrag = 0;
        b.gravityScale = 0;
        b.freezeRotation = true;
        if(enemy)
        {
            enemies.Add(obj);
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
            pc = obj.AddComponent<PlayerController>().GetComponent<PlayerController>();
            obj.tag = "Player";
        }
    }

    // destroy remaining prefabs from level
    void ClearLevel()
    {
        foreach(GameObject bomb in GameObject.FindGameObjectsWithTag("bomb"))
            if(bomb)
                Destroy(bomb);
        foreach(GameObject explosion in GameObject.FindGameObjectsWithTag("explosion"))
            if(explosion)
                Destroy(explosion);
    }
    void LoadMedia()
    {
        // load game music
        level1 = Audio.LoadSound("sounds/game0", "game", gameObject);
        level2 = Audio.LoadSound("sounds/game1", "game", gameObject);
        level3 = Audio.LoadSound("sounds/game2", "game", gameObject);
        level1.loop = true;
        level2.loop = true;
        level3.loop = true;

        // load bird prefabs
        owl = Resources.Load<GameObject>("Owl");
        eagle = Resources.Load<GameObject>("Eagle");
        chicken = Resources.Load<GameObject>("Chicken");
        blueBird = Resources.Load<GameObject>("Blue bird");
        yellowBird = Resources.Load<GameObject>("Yellow Bird");
    }
    
    // normal settings for player before every round
    void PlayerSettingsNormal()
    {
        Player.speed = 5f;
        Player.blastRadius = 3;
        Player.lives = 1;
        Player.bombsAtOnce = 1;
    }

    // all the spawnpoints in the corners of the map
    void CreateSpawnPoints(List<Vector3> spawnPoints)
    {
        spawnPoints.Clear();
        spawnPoints.Add(new Vector3(8, 4.5f, 0));
        spawnPoints.Add(new Vector3(-8, 4.5f, 0));
        spawnPoints.Add(new Vector3(8, -5.5f, 0));
        spawnPoints.Add(new Vector3(-8, -5.5f, 0));
    }


    // there probably is nicer way to do this but I dont' have infinite time and this works
    void HandlePowerUps()
    {
        GameObject oneUp = GameObject.Find("oneUp(Clone)");
        GameObject lightning = GameObject.Find("lightning(Clone)");
        GameObject fire = GameObject.Find("fire1(Clone)");
        GameObject bomb = GameObject.Find("bombimage(Clone)");
        GameObject star = GameObject.Find("star(Clone)");

        if(oneUp)
        {
            BoxCollider2D bc = oneUp.GetComponent<BoxCollider2D>();

            // if player gets the powerup
            if(bc.IsTouching(player))
            {
                Player.AddLife();
                Destroy(oneUp);
            }

            // if enemy gets the powerup
            for(int i = 0; i < enemies.Count; i++)
            {
                if(bc.IsTouching(enemies[i].gameObject.GetComponent<BoxCollider2D>()))
                {
                    enemies[i].GetComponent<EnemyController>().AddLife();
                    Destroy(oneUp);
                }
            }
        }
        
        if(lightning)
        {
            BoxCollider2D bc = lightning.GetComponent<BoxCollider2D>();
            if(bc.IsTouching(player))
            {
                Player.AddSpeed(5);
                Destroy(lightning);
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                if(bc.IsTouching(enemies[i].gameObject.GetComponent<BoxCollider2D>()))
                {
                    enemies[i].GetComponent<EnemyController>().AddSpeed(5);
                    Destroy(lightning);
                }
            }
        }

        if(fire)
        {
            BoxCollider2D bc = fire.GetComponent<BoxCollider2D>();
            if(bc.IsTouching(player))
            {
                Player.AddBlastRadius();
                Destroy(fire);
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                if(bc.IsTouching(enemies[i].gameObject.GetComponent<BoxCollider2D>()))
                {
                    enemies[i].GetComponent<EnemyController>().AddBlastRadius();
                    Destroy(fire);
                }
            }
        }

        if(bomb)
        {
            BoxCollider2D bc = bomb.GetComponent<BoxCollider2D>();
            if(bc.IsTouching(player))
            {
                Player.AddBombsAtOnce();
                Destroy(bomb);
            }

            for(int i = 0; i < enemies.Count; i++)
            {
                if(bc.IsTouching(enemies[i].gameObject.GetComponent<BoxCollider2D>()))
                {
                    enemies[i].GetComponent<EnemyController>().AddBombsAtOnce();
                    Destroy(bomb);
                }
            }
        }

        if(star)
        {
            BoxCollider2D bc = star.GetComponent<BoxCollider2D>();
            if(bc.IsTouching(player))
            {
                Player.AddScore(1000);
                Destroy(star);
            }

            for(int i = 0; i < enemies.Count; i++)
                if(bc.IsTouching(enemies[i].gameObject.GetComponent<BoxCollider2D>()))
                    Destroy(star);
        }
    }
}
