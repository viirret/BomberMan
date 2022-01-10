using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    static GameObject Pause;
    static public bool gameIsPaused;
    static Button Main;
    static bool created = false;
    static AudioSource clickSound;
    static GameObject title;
    static GameObject PlayButton;
    static GameObject OptionsButton;
    static GameObject QuitButton;
    void Start()
    {
        if(!created)
        {
            clickSound = Audio.LoadSound("sounds/clicksound", "effects");
            DontDestroyOnLoad(this.gameObject);
            // ui elements
            Pause = GameObject.Find("PauseMenuObj");
            Button Resume = GameObject.Find("Resume").GetComponent<Button>();
            Button Quit = GameObject.Find("Quit").GetComponent<Button>();
            Main = GameObject.Find("ToMain").GetComponent<Button>();
            
            // listeners
            Resume.onClick.AddListener(ResumeGame);
            Quit.onClick.AddListener(Application.Quit);
            Main.onClick.AddListener(MainMenu);
            
            gameIsPaused = false;
            Pause.SetActive(false);
            Main.gameObject.SetActive(false);
            created = true;

        }
        else
            Destroy(this.gameObject);
        
        // get elemets from main menu
        title = GameObject.Find("Title");
        QuitButton = GameObject.Find("QuitButton");
        PlayButton = GameObject.Find("PlayButton");
        OptionsButton = GameObject.Find("OptionsButton");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(gameIsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void ResumeGame()
    {
        clickSound.Play();
        Pause.SetActive(false);
        gameIsPaused = false;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "game")
            Time.timeScale = 1f;
        if(scene.name == "MainMenu")
            Active(true);

    }

    public static void PauseGame()
    {
        clickSound.Play();
        Pause.SetActive(true);
        gameIsPaused = true;
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "game")
        {
            Time.timeScale = 0f;
            Main.gameObject.SetActive(true);
        }
        if(scene.name == "MainMenu")
            Active(false);
    }
    void MainMenu()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        Main.gameObject.SetActive(false);
        Pause.SetActive(false);
    }
    static void Active(bool active)
    {
        title.SetActive(active);
        QuitButton.SetActive(active);
        PlayButton.SetActive(active);
        OptionsButton.SetActive(active);
    }
}