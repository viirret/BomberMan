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
        double multiplier = 5.0;
        multiplier -= GameTexts.totalTime / 100;
        int FinalScore = (int)multiplier * Player.score;
        score.text = "Final score: " + FinalScore;
    }

    void Main()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
