using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public static float speed = 5f;
    public static int blastRadius = 3;
    public static int bombsAtOnce = 1;
    public static int lives = 1;

    void Update()
    {
        if(lives == 0)
            Debug.Log("You died!");
    }
     
    
}
