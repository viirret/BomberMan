public static class Levels
{
    public static bool StartNewLevel = true;
    public static int level = 1;
    public static void NewLevel() 
    {
        StartNewLevel = true;
        GameMap.RefreshMap(GameMap.TilemapTop);
        level++;
    }
}
