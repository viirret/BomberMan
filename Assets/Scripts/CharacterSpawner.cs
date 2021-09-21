using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    
    GameObject blueBird;
    void Start()
    {
        Vector3Int[] spawnPoints = new Vector3Int[4];
        blueBird = Resources.Load<GameObject>("Blue bird");
        blueBird.transform.localScale = new Vector3(0.06f, 0.04f, 1f);
        CreateSpawnPoints(spawnPoints);
        Instantiate(blueBird, spawnPoints[1], Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateSpawnPoints(Vector3Int[] spawnPoints)
    {
        spawnPoints[0].x = -9;
        spawnPoints[0].y = -6;
        spawnPoints[0].z = -0;

        spawnPoints[1].x = -9;
        spawnPoints[1].y = 4;
        spawnPoints[1].z = 0;

        spawnPoints[2].x = 7;
        spawnPoints[2].y = -6;
        spawnPoints[2].z = 0;

        spawnPoints[3].x = 7;
        spawnPoints[3].y = 4;
        spawnPoints[3].z = 0;
    }
}
