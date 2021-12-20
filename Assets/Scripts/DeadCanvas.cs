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
    AudioSource clickSound;
    void Start()
    {
        clickSound = Audio.LoadSound("sounds/clicksound", "effects", gameObject);

        // get elements
        obj = GameObject.Find("DeadObj");
        main = GameObject.Find("ToMain").GetComponent<Button>();
        quit = GameObject.Find("Quit").GetComponent<Button>();

        // listeners
        main.onClick.AddListener(Main);
        quit.onClick.AddListener(Func.Quit);

        // check to pausing of the game
        obj.SetActive(false);
    }

    public static void PlayWhenDead()
    {
        obj.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.gameIsPaused = true;
        Levels.level = 1;
        Player.score = 0;
    }

    void Main()
    {
        clickSound.Play();
        Time.timeScale = 1f;
        obj.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        Levels.StartNewLevel = true;
    }
}
