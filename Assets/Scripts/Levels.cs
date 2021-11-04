using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Levels
{
    public static bool StartNewLevel = true;
    static int level = 1;
    public static int GetCurrentLevel() => level;
    public static void NewLevel() 
    {
        StartNewLevel = true;
        level++;
    }
}
