using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float countdown = 2f;
    public Vector2 pos;
    GameObject bombPrefab; 
    
    void Start()
    {
        bombPrefab = Resources.Load<GameObject>("bomb");
    }
    void Update()
    {
        countdown -= Time.deltaTime;

        pos = PlayerController.bombposition;

        if(countdown <= 0f)
        {
            FindObjectOfType<MapDestroyer>().Explode(pos);
            Debug.Log("Explosion!");
            Destroy(gameObject);
        }
    }
}
