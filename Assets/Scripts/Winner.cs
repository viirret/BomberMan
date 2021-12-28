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
    static TMP_Text score;
    AudioSource clickSound;
    void Start()
    {
        clickSound = Audio.LoadSound("souds/clicksound", "effect", gameObject);

        // get elements
        obj = GameObject.Find("winObj");
        main = GameObject.Find("ToMain").GetComponent<Button>();
        score = GameObject.Find("GameScore").GetComponent<TMP_Text>();

        main.onClick.AddListener(Main);
        obj.SetActive(false);
    }

    public static void Win()
    {
        obj.SetActive(true);
        PauseMenu.gameIsPaused = true;
        Time.timeScale = 0f;

        // give final score
        double multiplier = 5 - (double)GameTexts.totalTime / 30.0;
        double FinalScore = multiplier * Player.score;

        score.text = 
        "Bomber score: " + Player.score + 
        "\n\nTime bonus: " + multiplier.ToString("0.##") +
        "\n\nFinal score: " + (int)FinalScore; 
    }

    void Main()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
