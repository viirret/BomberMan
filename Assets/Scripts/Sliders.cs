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


    void Start()
    {
        myMixer = Resources.Load<AudioMixer>("myMixer");

        //sliders
        gameSlider = GameObject.Find("gameSlider").GetComponent<Slider>();
        themeSlider = GameObject.Find("themeSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("effectsSlider").GetComponent<Slider>();

        // percentages
        gamePercent = GameObject.Find("gameSliderPercent").GetComponent<Text>();
        themePercent = GameObject.Find("themeSliderPercent").GetComponent<Text>();
        sfxPercent = GameObject.Find("effectsSliderPercent").GetComponent<Text>();

        // default value for sliders
        gameSlider.value = 0.5f;
        //themeSlider.value = 0.75f;
        sfxSlider.value = 0.5f;
        
        Preferences();
        // listeners
        gameSlider.onValueChanged.AddListener(delegate { SetGameVolume(); });
        themeSlider.onValueChanged.AddListener(delegate { SetThemeVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });
   
    }

    void Preferences()
    {
        themeSlider.value = PlayerPrefs.GetFloat("myVolume", 0.5f);
        themePercent.text = PlayerPrefs.GetString("myPercent", "50%");
        float vol = PlayerPrefs.GetFloat("testing", 0f);
        
        myMixer.SetFloat("themeVolume", vol);
     
    }

    void SetThemeVolume()
    {
        float vol = (1 - Mathf.Sqrt(themeSlider.value)) * -80f;
        string percent = Mathf.RoundToInt(themeSlider.value * 100) + "%";

        PlayerPrefs.SetFloat("myVolume", themeSlider.value);
        PlayerPrefs.SetString("myPercent", percent);
        // workaround for GetFloat not working
        PlayerPrefs.SetFloat("testing", vol);

        themePercent.text = percent;
        myMixer.SetFloat("themeVolume", vol);
    }


    // this all could be made as one function. Might make it later
    void SetGameVolume()
    {
        gamePercent.text = Mathf.RoundToInt(gameSlider.value * 100) + "%";
        myMixer.SetFloat("gameVolume", Mathf.Log10(gameSlider.value) * 20);
    }

    void SetSfxVolume()
    {
        sfxPercent.text = Mathf.RoundToInt(sfxSlider.value * 100) + "%";
        myMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);
    }

}
