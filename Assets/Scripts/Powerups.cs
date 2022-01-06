using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Powerups : MonoBehaviour 
{
    List<Vector3> spawnPoints = new List<Vector3>();
    public static Powerups instance;
    public GameObject extraLife;
    public GameObject lightning;
    public GameObject fire;
    public GameObject bomb;
    public GameObject star;
    
    void Start()
    {
        instance = this;
    }

    public void CreatePowerUps()
    {
        // destroy existing powerups
        foreach(GameObject powerup in GameObject.FindGameObjectsWithTag("powerup"))
            if(powerup)
                Destroy(powerup);

        // create spawnpoints for powerups
        AddAllSpawns();

        // make the powerups
        MakePowerUp(extraLife);
        MakePowerUp(lightning);
        MakePowerUp(fire);
        MakePowerUp(bomb);
        MakePowerUp(star);        
    }

    void MakePowerUp(GameObject obj)
    {
        int rnd = Random.Range(0, spawnPoints.Count);
        Powerup(obj, spawnPoints[rnd]);
        spawnPoints.RemoveAt(rnd);
    }

    void Powerup(GameObject prefab, Vector3 position)
    {
        GameObject obj;
        obj = Instantiate(prefab, position, Quaternion.identity);
        if(prefab != star)
            obj.tag = "powerup";
        obj.AddComponent<BoxCollider2D>();
        SpriteRenderer SR = obj.GetComponent<SpriteRenderer>();
        SR.sortingOrder = 1;
        Rigidbody2D RB = obj.AddComponent<Rigidbody2D>().GetComponent<Rigidbody2D>();
        RB.gravityScale = 0;

        // change size according to prefab
        if(prefab == lightning)
            obj.transform.localScale = new Vector3(0.1f, 0.08f, 1f);
        if(prefab == extraLife)
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        if(prefab == fire)
            obj.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        if(prefab == bomb || prefab == star)
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
    }

    void AddAllSpawns()
    {
        spawnPoints.Clear();
        
        // up row
        spawnPoints.Add(new Vector3(-6, 3, 0));
        spawnPoints.Add(new Vector3(-4, 3, 0));
        spawnPoints.Add(new Vector3(-2, 3, 0));
        spawnPoints.Add(new Vector3(0, 3, 0));
        spawnPoints.Add(new Vector3(2, 3, 0));
        spawnPoints.Add(new Vector3(4, 3, 0));
        spawnPoints.Add(new Vector3(6, 3, 0));

        // down row
        spawnPoints.Add(new Vector3(-6, -3, 0));
        spawnPoints.Add(new Vector3(-4, -3, 0));
        spawnPoints.Add(new Vector3(-2, -3, 0));
        spawnPoints.Add(new Vector3(0, -3, 0));
        spawnPoints.Add(new Vector3(2, -3, 0));
        spawnPoints.Add(new Vector3(4, -3, 0));
        spawnPoints.Add(new Vector3(6, -3, 0));

        // sides
        spawnPoints.Add(new Vector3(6, 1, 0));
        spawnPoints.Add(new Vector3(6, -1, 0));
        spawnPoints.Add(new Vector3(-6, 1, 0));
        spawnPoints.Add(new Vector3(-6, -1, 0));
    }
}
 