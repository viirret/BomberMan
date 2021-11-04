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
    GameObject OptionsPanel;

    void Start()
    {
        // define buttons
        PlayButton = GameObject.Find("PlayButton").GetComponent<Button>(); 
        OptionsButton = GameObject.Find("OptionsButton").GetComponent<Button>();
        ExtraButton = GameObject.Find("ExtrasButton").GetComponent<Button>();
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        BackButtonOptions = GameObject.Find("BackButtonOptions").GetComponent<Button>();

        // define canvases
        OptionsPanel = GameObject.Find("OptionsPanel");
        
        // listeners for buttons
        PlayButton.onClick.AddListener(StartGame);
        OptionsButton.onClick.AddListener(() => { ShowPanel(OptionsPanel); });
        BackButtonOptions.onClick.AddListener(() => { UnShowPanel(OptionsPanel); });
        //ExtraButton.onClick.AddListener();
        QuitButton.onClick.AddListener(QuitGame);

        // set panel unactive
        OptionsPanel.SetActive(false);
    }

    void ShowPanel(GameObject UI)
    {
        UI.SetActive(true);
    }

    void UnShowPanel(GameObject UI)
    {
        UI.SetActive(false);
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
