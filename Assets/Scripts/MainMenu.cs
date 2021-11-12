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
    AudioSource clickSound;

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
        ExtraButton.onClick.AddListener(ExtrasButton);
        QuitButton.onClick.AddListener(QuitGame);

        // play theme
        AudioSource s = Audio.LoadSound("sounds/theme", "theme", gameObject);
        s.Play();

        clickSound = Audio.LoadSound("sounds/clicksound", "effects");
    }

    void StartGame()
    {
        SceneManager.LoadScene("game");
        clickSound.Play();
    }

    void ExtrasButton()
    {
        clickSound.Play();
    }

    void QuitGame()
    {
        Debug.Log("Quit from game");
        Application.Quit();
    }
}
