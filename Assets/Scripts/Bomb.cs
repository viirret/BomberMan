using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector2 pos;
    public int blastRadius;
    GameObject bombPrefab;
    GameObject gm; 
    MapDestroyer mapdes;
    GameObject bomb;
    AudioSource bombsound;
    AudioSource lightup;

    // create the bomb
    void Start()
    {
        bombsound = Audio.LoadSound("sounds/bomb", "game", GameObject.Find("Game"));
        lightup = Audio.LoadSound("sounds/lightup", "game", GameObject.Find("Game"));
        
        bombPrefab = Resources.Load<GameObject>("bomb");
        bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        bomb.tag = "bomb";
        gm = GameObject.Find("Grid");
        mapdes = gm.GetComponent<MapDestroyer>();
        mapdes.blastRadius = blastRadius;
        StartCoroutine(WaitTwo());
    }

    IEnumerator WaitTwo()
    {
        // adding BoxCollider after 0.5s so bird won't get stuck
        lightup.Play();
        yield return new WaitForSeconds(0.5f);
        bomb.AddComponent<BoxCollider2D>();
        BoxCollider2D x = bomb.GetComponent<BoxCollider2D>();
        x.size = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(1.5f);
        GoOff();
    }

    // destroy the bomb
    public void GoOff()
    {
        if(bomb)
        {
            bombsound.Play();
            mapdes.Explode(pos);
            Destroy(bomb);
        }
    }
}
