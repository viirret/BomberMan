using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTexts : MonoBehaviour
{
    Text score;
    Text time;
    Text lives;
    public System.DateTime startTime;
    int totalTime = 0;
    void Start()
    {
        score = GameObject.Find("score").GetComponent<Text>();
        time = GameObject.Find("time").GetComponent<Text>();
        lives = GameObject.Find("lives").GetComponent<Text>();
        startTime = System.DateTime.UtcNow;
    }

    void Update()
    {
        System.TimeSpan ts = System.DateTime.UtcNow - startTime;
        totalTime = ts.Hours + ts.Minutes * 60 + ts.Seconds;
        
        score.text = "Score: " + Player.score;
        time.text = "Time: " + totalTime;
        lives.text = "Lives: " + Player.lives;
    }
}
