using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{
    public static float speed = 5f;
    public static int blastRadius = 3;
    public static int bombsAtOnce = 1;
    public static int lives = 0;
    public static int score = 0;

    // functions for player
    public static void AddScore(int amount) => score += amount;
    public static void AddSpeed(int amount) => speed += amount;
    public static void AddBlastRadius() => blastRadius++;
    public static void AddBombsAtOnce() => bombsAtOnce++;
    public static void RemoveLife() => lives--;
    public static void AddLife() => ++lives;
}
