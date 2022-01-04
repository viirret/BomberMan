using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Powerups : MonoBehaviour 
{
    public static Powerups instance;
    public GameObject extraLife;
    public GameObject lightning;
    
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
        

        // create powerups
        Powerup(extraLife, new Vector3(1, 1, 0));
        Powerup(lightning, new Vector3(2, 3, 0));

    }

    void Powerup(GameObject prefab, Vector3 position)
    {
        GameObject obj;
        obj = Instantiate(prefab, position, Quaternion.identity);
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
    }
}
 