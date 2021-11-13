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

    // float for percentage
    float gameSliderPercentage;
    float themeSliderPercentage;
    float sfxSliderPercentage;


    void Start()
    {
        myMixer = Resources.Load<AudioMixer>("myMixer");

        gameSlider = GameObject.Find("gameSlider").GetComponent<Slider>();
        themeSlider = GameObject.Find("themeSlider").GetComponent<Slider>();
        sfxSlider = GameObject.Find("effectsSlider").GetComponent<Slider>();

        // percentages
        gamePercent = GameObject.Find("gameSliderPercent").GetComponent<Text>();
        themePercent = GameObject.Find("themeSliderPercent").GetComponent<Text>();
        sfxPercent = GameObject.Find("effectsSliderPercent").GetComponent<Text>();
    }

    public void SetGameVolume(float slider)
    {
        Debug.Log("value changed");
        gamePercent.text = Mathf.RoundToInt(slider * 100) + "%";
        gameSliderPercentage = Mathf.RoundToInt(slider * 100);

        gameSliderPercentage = gameSlider.value;
        myMixer.SetFloat("game", Mathf.Log10(slider) * 20);
    }

}
