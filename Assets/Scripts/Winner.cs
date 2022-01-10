using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Winner : MonoBehaviour
{
    static GameObject obj;
    Button main;
    Button newGame;
    static TMP_Text score;
    AudioSource clickSound;
    void Start()
    {
        clickSound = Audio.LoadSound("souds/clicksound", "effect", gameObject);

        // get elements
        obj = GameObject.Find("winObj");
        main = GameObject.Find("ToMain").GetComponent<Button>();
        newGame = GameObject.Find("PlayAgain").GetComponent<Button>();
        score = GameObject.Find("GameScore").GetComponent<TMP_Text>();

        main.onClick.AddListener(Main);
        newGame.onClick.AddListener(StartAgain);
        obj.SetActive(false);
    }

    // play again with enter
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            StartAgain();
    }

    public static void Win()
    {
        obj.SetActive(true);
        PauseMenu.gameIsPaused = true;
        Time.timeScale = 0f;

        // give final score
        double multiplier = 10 - (double)GameTexts.totalTime / 30.0;
        double FinalScore = multiplier * Player.score;

        // minimum
        if(multiplier < 1.0)
            multiplier = 1.0;

        score.text = 
        "Bomber score: " + Player.score + 
        "\nTime bonus: " + multiplier.ToString("0.##") +
        "\nFinal score: " + (int)FinalScore; 
    }

    void Main()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    void StartAgain()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("game");
    }
}
