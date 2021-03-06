using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    Button PlayButton;
    Button OptionsButton;
    Button QuitButton;
    AudioSource clickSound;

    void Start()
    {
        // define buttons
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        PlayButton = GameObject.Find("PlayButton").GetComponent<Button>(); 
        OptionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();

        // listeners for buttons
        QuitButton.onClick.AddListener(Application.Quit);
        PlayButton.onClick.AddListener(StartGame);
        OptionsButton.onClick.AddListener(PauseMenu.PauseGame);

        // play theme
        AudioSource s = Audio.LoadSound("sounds/theme", "theme", gameObject);
        s.Play();
        s.loop = true;

        clickSound = Audio.LoadSound("sounds/clicksound", "effects", gameObject);
    }

    void StartGame()
    {
        SceneManager.LoadScene("game");
        clickSound.Play();
    }
}
