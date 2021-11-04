using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTexts : MonoBehaviour
{
    Text score;
    void Start()
    {
        score = GameObject.Find("score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + Player.score;
    }
}
