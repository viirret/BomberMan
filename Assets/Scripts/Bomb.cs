using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector2 pos;
    GameObject bombPrefab;
    GameObject gm; 
    MapDestroyer mapdes;
    GameObject b;
    void Start()
    {
        bombPrefab = Resources.Load<GameObject>("bomb");
        b = Instantiate(bombPrefab, pos, Quaternion.identity);
        gm = GameObject.Find("Grid");
        mapdes = gm.GetComponent<MapDestroyer>();
        StartCoroutine(WaitTwo());
    }

    IEnumerator WaitTwo()
    {
        yield return new WaitForSeconds(2);
        GoOff();
    }

    public void GoOff()
    {
        mapdes.Explode(pos);
        Debug.Log("Explosion!");
        Destroy(b);
    }
}
