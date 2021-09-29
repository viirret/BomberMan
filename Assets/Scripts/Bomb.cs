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

    // create the bomb
    void Start()
    {
        bombPrefab = Resources.Load<GameObject>("bomb");
        bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        gm = GameObject.Find("Grid");
        mapdes = gm.GetComponent<MapDestroyer>();
        mapdes.blastRadius = blastRadius;
        StartCoroutine(WaitTwo());
    }

    IEnumerator WaitTwo()
    {
        yield return new WaitForSeconds(2);
        GoOff();
    }

    // destroy the bomb
    public void GoOff()
    {
        mapdes.Explode(pos);
        Debug.Log("Explosion!");
        Destroy(bomb);
    }
}
