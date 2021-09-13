using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float countdown = 2f;
    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0f)
        {
            Debug.Log("Explosion!");
            Destroy(gameObject);
        }

    }
}
