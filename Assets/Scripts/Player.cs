using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
    public static float speed = 5f;
    public static int blastRadius = 3;
    public static int bombsAtOnce = 1;
    public static int lives = 1;

    static void Dead()
    {
        Debug.Log("You died!");
    }

    public static void AddLife()
    {
        lives++;
    }

    public static void RemoveLife()
    {
        lives--;
        Debug.Log("Lives left: " + lives);
        if(lives == 0)
        {
            Dead();
        }
    }

    public static void AddSpeed()
    {
        speed += 1;
    }

    public static void AddBlastRadius()
    {
        blastRadius++;
    }
}
