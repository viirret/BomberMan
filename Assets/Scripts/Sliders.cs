using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Sliders : MonoBehaviour
{
    AudioMixer myMixer;

    // sliders
    Slider gameSlider;
    Slider themeSlider;
    Slider sfxSlider;

    // percentages
    Text gamePercent;
    Text themePercent;
    Text sfxPercent;

    void Start()
    {
        myMixer = Resources.Load<AudioMixer>("myMixer");

        // sliders
        gameSlider = GameObject.Find("gameSlider").GetComponent<Slider>();
        themeSlider = GameObject.Find("themeSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("effectsSlider").GetComponent<Slider>();

        // percentages
        gamePercent = GameObject.Find("gameSliderPercent").GetComponent<Text>();
        themePercent = GameObject.Find("themeSliderPercent").GetComponent<Text>();
        sfxPercent = GameObject.Find("effectsSliderPercent").GetComponent<Text>();

        // set preferences
        Preferences(sfxSlider, sfxPercent, "sfxValue", "sfxPercentage", "mySfxVolume", "sfxVolume");
        Preferences(gameSlider, gamePercent, "gameValue", "gamePercentage", "myGameVolume", "gameVolume"); 
        Preferences(themeSlider, themePercent, "themeValue", "themePercentage", "myThemeVolume", "themeVolume");

        // set listeners
        sfxSlider.onValueChanged.AddListener(delegate 
        { 
            SetVolume(sfxSlider, sfxPercent, "sfxValue", "sfxPercentage", "mySfxVolume", "sfxVolume"); 
        });

        gameSlider.onValueChanged.AddListener(delegate 
        { 
            SetVolume(gameSlider, gamePercent, "gameValue", "gamePercentage", "myGameVolume", "gameVolume"); 
        });

        themeSlider.onValueChanged.AddListener(delegate 
        { 
            SetVolume(themeSlider, themePercent, "themeValue", "themePercentage", "myThemeVolume", "themeVolume"); 
        });
    }

    // load preferences, if not set sliders have the same default value
    void Preferences(Slider slider, Text percentageText, string sliderValue, string percentage, string actualValue, string mixerName)
    {
        slider.value = PlayerPrefs.GetFloat(sliderValue, 0.75f);
        percentageText.text = PlayerPrefs.GetString(percentage, "75%");
        
        // problems with myMixer.GetFloat so this a little workaround
        float vol = PlayerPrefs.GetFloat(actualValue, -10.5f);
        myMixer.SetFloat(mixerName, vol);
    }

    void SetVolume(Slider slider, Text percentageText, string sliderValue, string percentage, string actualValue, string mixerName)
    {
        // make volume and percentage from slider
        float vol = (1 - Mathf.Sqrt(slider.value)) * -80f;
        string percent = Mathf.RoundToInt(slider.value * 100) + "%";

        // set preferences
        PlayerPrefs.SetFloat(sliderValue, slider.value);
        PlayerPrefs.SetString(percentage, percent);
        PlayerPrefs.SetFloat(actualValue, vol);

        // set text and mixer
        percentageText.text = percent;
        myMixer.SetFloat(mixerName, vol);
    }
}
