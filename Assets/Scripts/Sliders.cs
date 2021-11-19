using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Sliders : MonoBehaviour
{
    // set preferences from volume later
    
    AudioMixer myMixer;

    // sliders
    Slider gameSlider;
    Slider themeSlider;
    Slider sfxSlider;

    // percentages
    Text gamePercent;
    Text themePercent;
    Text sfxPercent;

    // float for percentage
    float gameSliderPercentage;
    float themeSliderPercentage;
    float sfxSliderPercentage;


    void Awake()
    {
        myMixer = Resources.Load<AudioMixer>("myMixer");

        //sliders
        gameSlider = GameObject.Find("gameSlider").GetComponent<Slider>();
        themeSlider = GameObject.Find("themeSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("effectsSlider").GetComponent<Slider>();

        // listeners
        gameSlider.onValueChanged.AddListener(delegate { SetGameVolume(); });
        themeSlider.onValueChanged.AddListener(delegate { SetThemeVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });


        // percentages
        gamePercent = GameObject.Find("gameSliderPercent").GetComponent<Text>();
        themePercent = GameObject.Find("themeSliderPercent").GetComponent<Text>();
        sfxPercent = GameObject.Find("effectsSliderPercent").GetComponent<Text>();

    }

    // this all could be made as one function. Might make it later
    void SetGameVolume()
    {
        gamePercent.text = Mathf.RoundToInt(gameSlider.value * 100) + "%";
        myMixer.SetFloat("gameVolume", Mathf.Log10(gameSlider.value) * 20);
    }

    void SetThemeVolume()
    {
       themePercent.text = Mathf.RoundToInt(themeSlider.value * 100) + "%";
       myMixer.SetFloat("themeVolume", Mathf.Log10(themeSlider.value) * 20); 
    }

    void SetSfxVolume()
    {
        sfxPercent.text = Mathf.RoundToInt(sfxSlider.value * 100) + "%";
        myMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);
    }

}
