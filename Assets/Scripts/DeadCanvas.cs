using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadCanvas : MonoBehaviour
{
    static GameObject obj;
    Button main;
    Button quit;
    Button playAgain;
    AudioSource clickSound;
    void Start()
    {
        clickSound = Audio.LoadSound("sounds/clicksound", "effects", gameObject);

        // get elements
        obj = GameObject.Find("DeadObj");
        main = GameObject.Find("ToMain").GetComponent<Button>();
        quit = GameObject.Find("Quit").GetComponent<Button>();
        playAgain = GameObject.Find("PlayAgain").GetComponent<Button>();

        // listeners
        main.onClick.AddListener(Main);
        playAgain.onClick.AddListener(PlayAgain);
        quit.onClick.AddListener(Application.Quit);

        // make everything not active
        obj.SetActive(false);
    }

    // play again with "enter"
    void Update()
    {
        if(obj.activeSelf)
            if(Input.GetKeyDown(KeyCode.Return))
                PlayAgain();
    }

    // this function is played from Game.cs
    public static void PlayWhenDead()
    {
        obj.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.gameIsPaused = true;
    }

    void Main()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    void PlayAgain()
    {
        clickSound.Play();
        obj.SetActive(false);
        SceneManager.LoadScene("game");
    }
}
