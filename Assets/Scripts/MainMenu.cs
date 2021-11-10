using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    Button PlayButton;
    Button OptionsButton;
    Button ExtraButton;
    Button QuitButton;
    Button BackButtonOptions;

    void Start()
    {
        // define buttons
        PlayButton = GameObject.Find("PlayButton").GetComponent<Button>(); 
        OptionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
        ExtraButton = GameObject.Find("ExtrasButton").GetComponent<Button>();
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        // listeners for buttons
        PlayButton.onClick.AddListener(StartGame);

        OptionsButton.onClick.AddListener(() => { PauseMenu.PauseGame(); });
        //ExtraButton.onClick.AddListener();
        QuitButton.onClick.AddListener(QuitGame);

        // play theme
        AudioSource s = Audio.LoadSound("sounds/theme", "theme", gameObject);
        s.Play();
    }


    void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    void QuitGame()
    {
        Debug.Log("Quit from game");
        Application.Quit();
    }
}
