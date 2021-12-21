using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTexts : MonoBehaviour
{
    Text score;
    Text time;
    Text lives;
    Text level;
    public static int totalTime = 0;
    void Start()
    {
        score = GameObject.Find("score").GetComponent<Text>();
        time = GameObject.Find("time").GetComponent<Text>();
        lives = GameObject.Find("lives").GetComponent<Text>();
        level = GameObject.Find("level").GetComponent<Text>();
        StartCoroutine(AddTime());
    }

    // add to time every second game is not paused
    IEnumerator AddTime()
    {
        for(;;)
        {
            if(!PauseMenu.gameIsPaused)
                totalTime++;
            yield return new WaitForSeconds(1);
        }
    }

    void Update()
    {
        time.text = "Time: " + totalTime;
        score.text = "Score: " + Player.score;
        lives.text = "Lives: " + Player.lives;
        level.text = "Level: " + Levels.level;
    }
}
